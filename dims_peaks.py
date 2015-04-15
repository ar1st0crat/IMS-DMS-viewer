# -*- coding: utf-8 -*-
"""
At last!
At last I'm sittin' here 'n' codin' ))
"""

# conventional imports
import scipy as sp
import numpy as np
import matplotlib as mpl
import matplotlib.pyplot as plt

# csv helper
import csv

# simple gui stuff (deprecated though)
import Tkinter, tkFileDialog, tkMessageBox

# import signal processing tools
import scipy.signal

# import diff function for derivatives
# (actually for the 1st derivative I use my own implementation)
from numpy import diff

# I couldn't get the following thing work properly :(( ) 
#from scipy.signal import find_peaks_cwt



# ========================================== free parameters of the algorithm

MA_size = 15
distanceThreshold = 17

ADC_rate = 16
percentile = 10

lowerClipThreshold = 2**(ADC_rate - 1) / percentile             # 4000
upperClipThreshold = 2**(ADC_rate - 1) - lowerClipThreshold     # 27000

# ===========================================================================



# ----------------------------------------------------- extract data from csv

def getColumn(filename, column):
    with open(filename) as f:
        results = csv.reader(f, delimiter=";")
        return [result[column] for result in results]


# ------------------- obtain the 1st derivative curve and its smoothed version

def smoothDerivative(signal):

    size = len(signal)

    # ----------------------------------------- derivative
    # ----------------------------------------- d[0] = s[1] - s[0]
    # ----------------------------------------- d[N] = s[N] - s[N-1]
    # ----------------------------------------- d[i] = (s[i+1] - s[i-1] ) / 2
    deriv = np.zeros( size )
    
    deriv[0] = signal[1] - signal[0]
    
    for i in range(1,size-1):
        deriv[i] = (signal[i+1] - signal[i-1]) / 2

    deriv[size-1] = signal[ size-1 ] - signal[size-2]
    
    
    # ----------------------------------- smoothed derivative (moving average)
    smoothed_deriv = np.copy( deriv )
    
    for i in range(MA_size, size):
        for j in range(0, MA_size):
            smoothed_deriv[i-MA_size/2] += deriv[i-j]
        smoothed_deriv[i-MA_size/2] /= MA_size

#            smoothed_deriv[i - MA_size] += deriv[i-j]
#        smoothed_deriv[i - MA_size] /= MA_size

    # ------------------------------------------------------- or savgol
    deriv_savgol = sp.signal.savgol_filter( deriv, 11, 3 )
    
    #return deriv, deriv_savgol
    return deriv, smoothed_deriv
    
    
# --------------- fix the dims: move the clipped region up to the right place
    
def fix_prev( signal ):

    clips = []
    
    for i in range(1,len(signal)):
        if abs(signal[i]) - abs(signal[i-1]) < 100 and abs(signal[i]) > upperClipThreshold: #27000 :
            clips.append(i)
            
    clips = filterClips( signal, clips )

    for i in range( 0, len(clips), 2 ):
        if signal[clips[i]]*signal[clips[i-1]] < 0:
            for j in range( clips[i], clips[i+1] ):
                signal[j] = -signal[j]


def fix( signal ):
    for i in range( 1, len(signal) ):
        if signal[i] == -32768 and signal[i-1] > 0:
            signal[i] = 32767


# ---------------------------------------------- remove that annoying outlier

def removeOutlier(signal):

    size = len(signal)

    clippedSignal = np.copy(signal)

    for i in range(size):
        if ( abs(signal[i]) > lowerClipThreshold ):
            clippedSignal[i] = clippedSignal[i-1] #clipThreshold

    return clippedSignal    
    
    
# based on the analysis of spectral shapes we classify peaks as:
#   1. Spikes
#               2. Hills
#                           3. Clips (Plateau)    
    
def findSpikes(signal, deriv):
    spikes = []
    
    for i in range(2,len(signal)-2):
        if (abs(signal[i] - signal[i-2]) > lowerClipThreshold/3) and (abs(signal[i] - signal[i+2]) > lowerClipThreshold/3):
            if (signal[i] - signal[i-2]) * (signal[i] - signal[i+2]) > 0:
                spikes.append( i )    
    
    return spikes


def findClips(signal, deriv, maxValue = 32767.0):
    
    clips = []
    
    for i in range(len(deriv)):
        if abs(deriv[i]) < 5 and abs(signal[i]) > upperClipThreshold: #27000 :
            clips.append(i)
    
    return clips
    

def findHills(signal, deriv):
    
    minPeakValue = 32767    
    
    hills = []

    for i in range(1,len(deriv)):
        if deriv[i-1] * deriv[i] < 0 :
            hills.append(i)
            if signal[i] < minPeakValue:
                minPeakValue = signal[i]
    
    return hills, minPeakValue
    
    
# ---------------------------
# post-processing of peaks
# ---------------------------
    
def filterHills(signal, hills):
    
    for i in range(1,len(hills)-1):
        if abs(signal[hills[i]]) < lowerClipThreshold: #4000:
            if abs(signal[hills[i-1]]) < lowerClipThreshold and abs(signal[hills[i+1]]) < lowerClipThreshold:
                hills[i] = 0
                
        #if abs(signal[hills[i]]) > upperClipThreshold:
        #    hills[i] = 0
        
    hills = [x for x in hills if x != 0]    
        

def mergeHills(signal, hills):
    
    if len( hills ) == 0:
        return []
    
    mergedHills = [] #[ hills[0] ]
    
    maxPos = 0
    maxPeak = signal[hills[0]]    
    
    for i in range( len(hills) ):
                
        if hills[i] - hills[i-1] > distanceThreshold:
            mergedHills.append( hills[maxPos] )
            maxPos = i
            maxPeak = signal[hills[i]]
        elif abs(signal[hills[i]]) >= maxPeak:
            maxPos = i
            maxPeak = signal[ hills[i] ]
            
    return mergedHills
            

def filterClips(signal, clips):

    if len(clips) == 0:
        return []        
        
    clipMargins = [clips[0]]    
    
    for i in range(1, len(clips)):
        if clips[i] - clips[i-1] > 2:
            clipMargins.append(clips[i-1])
            clipMargins.append(clips[i])
    
    clipMargins.append( clips[i-1] )    
    
    return clipMargins

  

  
# -------------------------------------------------- open CSV file dialog

root = Tkinter.Tk()
root.withdraw()
filename = tkFileDialog.askopenfilename()

if filename == '':
    exit()

# --------------------------------------------------------- read csv file

full_info = getColumn( filename, 0 )

params = full_info[0]                       # measurement parameters go here

dims = np.zeros( len(full_info) - 1 )       # and here we create the int array
for i in range(1,len(full_info)-1):
    dims[i] = int(full_info[i])

plt.figure("Original DIMS")                 # and plot it
plt.plot( dims )



fix( dims )



# ------------------------------------------------- find peaks via cwt

#peaks_cwt = find_peaks_cwt( dims, np.arange(1,20) )
#print peaks_cwt


deriv, smoothed_deriv = smoothDerivative( dims )

spikes = findSpikes( dims, smoothed_deriv )
clips = findClips( dims, deriv )
hills, minPeakValue = findHills( dims, smoothed_deriv )


filterHills( dims, hills )
hills = mergeHills( dims, hills )


#clips = filterClips( dims, clips )





plt.figure("VComp/Intensity")                     # plot our DIMS

plt.subplot(311)
plt.xlabel("VComp(V)")
plt.ylabel("Intensity")
plt.plot( dims, 'b', label='dims' )


# ------------------------------------------- plot all kinds of peaks
plt.plot( spikes, dims[spikes], 'go', label='spikes' )
plt.plot( hills, dims[hills], 'ko', label='hills' )
plt.plot( clips, dims[clips], 'ro', label='clips' )

plt.legend()


# -------------------------------------- plot the 1st order derivative curve

smooth_deriv_diff = diff( dims )

plt.subplot(312)
#plt.plot( removeOutlier(smooth_deriv_diff), 'r', label='1st order derivative' )
plt.plot( removeOutlier(deriv), 'g', label='1st order derivative' )
plt.plot( removeOutlier(smoothed_deriv), 'b', label='1st order smoothed derivative' )
plt.legend()
plt.show()


# -------------------------------------- plot the 2nd order derivative curve

deriv_diff2 = diff( dims, n=2 )
plt.subplot(313)
plt.plot( removeOutlier(deriv_diff2) )


# ------------------------- Compute the histogram with numpy and then plot it
(n, bins) = np.histogram(dims, bins=2**12, normed=True)
plt.figure()
plt.plot( .5 * (bins[1:] + bins[:-1]), n )