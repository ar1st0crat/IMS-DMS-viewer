# -*- coding: utf-8 -*-

import csv
import Tkinter, tkFileDialog
import numpy as np
import matplotlib.pyplot as plt


############# free parameters of an algorithm ###############
MA_SIZE = 15
DISTANCE_THRESHOLD = 17
ADC_RATE = 16
PERCENTILE = 10
LOW_CLIP_THRESHOLD = 2**(ADC_RATE - 1) / PERCENTILE
HIGH_CLIP_THRESHOLD = 2**(ADC_RATE - 1) - LOW_CLIP_THRESHOLD
#############################################################


def get_column(filename, column):
    """ extract data from csv """
    with open(filename) as f:
        results = csv.reader(f, delimiter=";")
        return [result[column] for result in results]

def smooth_derivative(signal):
    """ obtain the 1st derivative curve and its smoothed version """
    size = len(signal)
    deriv = np.zeros( size )
    
    deriv[0] = signal[1] - signal[0]
    for i in range(1, size - 1):
        deriv[i] = (signal[i + 1] - signal[i - 1]) / 2
    deriv[size - 1] = signal[size - 1] - signal[size - 2]
    
    # smoothed derivative (1. Moving average)
    smoothderiv = np.copy(deriv)
    
    for i in range(MA_SIZE, size):
        for j in range(MA_SIZE):
            smoothderiv[i - MA_SIZE / 2] += deriv[i - j]
        smoothderiv[i - MA_SIZE / 2] /= MA_SIZE

    # Savitsky-Golay filter (2. savgol)
    # deriv_savgol = sp.signal.savgol_filter(deriv, 11, 3)
    # return deriv, deriv_savgol
    return deriv, smoothderiv
        
def fix(signal):
    """ fix DIMS: move the clipped region up to the right place """
    for i in range(1, len(signal)):
        if signal[i] == -32768 and signal[i - 1] > 0:
            signal[i] = 32767
            
def alternative_fix(signal):
    """ alternative method for fixing DIMS """
    clips = []
    for i in range(1, len(signal)):
        if abs(signal[i]) - abs(signal[i - 1]) < 100 and \
           abs(signal[i]) > HIGH_CLIP_THRESHOLD:
            clips.append(i)
            
    clips = filter_clips(signal, clips)

    for i in range(0, len(clips), 2):
        if signal[clips[i]] * signal[clips[i - 1]] < 0:
            for j in range(clips[i], clips[i + 1]):
                signal[j] = -signal[j]

def remove_outlier(signal):
    """ remove that annoying outlier """
    size = len(signal)
    clippedsignal = np.copy(signal)

    for i in range(size):
        if abs(signal[i]) > LOW_CLIP_THRESHOLD:
            clippedsignal[i] = clippedsignal[i - 1]

    return clippedsignal    
    
   
########################################################################
# based on the analysis of spectral shapes we classify peaks as:
#   1. Spikes
#                    2. Hills
#                                   3. Clips (Plateau)    
########################################################################
def find_spikes(signal, deriv):
    spikes = []
    for i in range(2, len(signal) - 2):
        if abs(signal[i] - signal[i - 2]) > LOW_CLIP_THRESHOLD / 3 and \
            abs(signal[i] - signal[i + 2]) > LOW_CLIP_THRESHOLD / 3:
            if (signal[i] - signal[i - 2]) * (signal[i] - signal[i + 2]) > 0:
                spikes.append(i)
    return spikes

def find_clips(signal, deriv, maxvalue=32767.0):
    clips = []
    for i in range(len(deriv)):
        if abs(deriv[i]) < 5 and abs(signal[i]) > HIGH_CLIP_THRESHOLD:
            clips.append(i)
    return clips

def find_hills(signal, deriv):
    min_peak_value = 32767    
    hills = []
    for i in range(1, len(deriv)):
        if deriv[i - 1] * deriv[i] < 0:
            hills.append(i)
            if signal[i] < min_peak_value:
                min_peak_value = signal[i]
    return hills, min_peak_value
    
    
##################################################################
#
#                   post-processing of peaks
#
##################################################################
def filter_hills(signal, hills):
    for i in range(1, len(hills) - 1):
        if abs(signal[hills[i]]) < LOW_CLIP_THRESHOLD:
            if abs(signal[hills[i - 1]]) < LOW_CLIP_THRESHOLD and \
               abs(signal[hills[i + 1]]) < LOW_CLIP_THRESHOLD:
                hills[i] = 0

    hills = [x for x in hills if x != 0]
        
def merge_hills(signal, hills):
    if len(hills) == 0:
        return []
    
    mergedhills = []
    maxpos = 0
    maxpeak = signal[hills[0]]    
    for i in range(len(hills)):
        if hills[i] - hills[i - 1] > DISTANCE_THRESHOLD:
            mergedhills.append(hills[maxpos])
            maxpos = i
            maxpeak = signal[hills[i]]
        elif abs(signal[hills[i]]) >= maxpeak:
            maxpos = i
            maxpeak = signal[hills[i]]
            
    return mergedhills

def filter_clips(signal, clips):
    if len(clips) == 0:
        return []        
        
    clipmargins = [clips[0]]    
    for i in range(1, len(clips)):
        if clips[i] - clips[i - 1] > 2:
            clipmargins.append(clips[i - 1])
            clipmargins.append(clips[i])
    
    clipmargins.append(clips[i - 1])
    return clipmargins


if __name__ == '__main__':
    # open CSV file dialog
    root = Tkinter.Tk()
    root.withdraw()
    filename = tkFileDialog.askopenfilename()
    if filename == '':
        exit()

    # read csv file
    fullinfo = get_column(filename, 0)
    # measurement parameters go here
    params = fullinfo[0]
    # and here we create the int array
    dims = np.array([int(fi) for fi in fullinfo[1:-1]])

    plt.figure("Original DIMS")
    plt.plot(dims)

    # fix spectrum
    fix(dims)

    # uncomment this if you want to compare your peaks with peaks found via cwt
    # peaks_cwt = find_peaks_cwt(dims, np.arange(1,20))
    # print(peaks_cwt)

    deriv, smoothderiv = smooth_derivative(dims)

    spikes = find_spikes(dims, smoothderiv)
    clips = find_clips(dims, deriv)
    hills, peakvalue = find_hills(dims, smoothderiv)

    filter_hills(dims, hills)
    hills = merge_hills(dims, hills)
    # uncomment this if you additionaly want to filter clips
    # clips = filter_clips( dims, clips )

    plt.figure("VComp/Intensity")
    # plot our DIMS
    plt.subplot(311)
    plt.xlabel("VComp(V)")
    plt.ylabel("Intensity")
    plt.plot(dims, 'b', label='dims')
    # plot all kinds of peaks
    plt.plot(spikes, dims[spikes], 'go', label='spikes')
    plt.plot(hills, dims[hills], 'ko', label='hills')
    plt.plot(clips, dims[clips], 'ro', label='clips')
    plt.legend()
    # plot derivatives    
    plt.subplot(312)
    plt.plot(remove_outlier(deriv), 'g', 
             label='1st order derivative')
    plt.plot(remove_outlier(smoothderiv), 'b', 
             label='1st order smoothed derivative')
    plt.legend()
    plt.show()
    # plot the 2nd order derivative curve
    deriv2 = np.diff(dims, n=2)
    plt.subplot(313)
    plt.plot(remove_outlier(deriv2))

    # Compute the histogram with numpy and then plot it
    (n, bins) = np.histogram(dims, bins=2**12, normed=True)
    plt.figure()
    plt.plot(.5 * (bins[1:] + bins[:-1]), n)