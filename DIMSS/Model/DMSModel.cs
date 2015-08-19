﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;


namespace DIMSS.Model
{
    /*
     * The CSV file structure should be as follows:
     *          
     *          1) MetaInfo; spec1; spec2; spec3; spec4; ...;
     *          
     *          or
     * 
     *          2) MetaInfo;; spec1; x1; spec2; x2; spec3; x3; ...;
     *          
     * 
     *  This version of DIMSS deals with constant spectrum dimension which we define in SPECSIZE
     */

    /// <summary>
    /// Model class for the DMS main form
    /// </summary>
    public class DMSModel
    {
        // by default the size of a spectrum is 2^11 = 2048 samples
        public const int SPECSIZE = 2048;

        // here we store measurement parameters
        private List<string> measureParams = new List<string>();
        public List<string> MeasureParams
        {
            get { return measureParams; }
        }

        // here we store all our spectra (vector of vectors)
        private List<List<int>> spectra = new List<List<int>>();
        public List<List<int>> Spectra
        {
            get { return spectra; }
        }

        // here we store x-coordinates of spectral points (in some cases they're not given)
        private List<List<float>> spectralPoints = new List<List<float>>();
        public List<List<float>> SpectralPoints
        {
            get { return spectralPoints; }
        }
       

        /// <summary>
        // Fix the dims spectrum: invert the sign of corrupted samples 
        /// </summary>
        /// <param name="spectrum">DIMS spectrum list</param>
        public void FixSpectrum(List<int> spectrum)
        {
            for (int i = 1; i < spectrum.Count; i++)
            {
                if ((spectrum[i] == -32768) && (spectrum[i - 1] > 0))
                {
                    spectrum[i] = 32767;
                }
            }
        }


        /// <summary>
        /// Iterate across the files in a given directory
        /// </summary>
        /// <param name="dir">Directory name</param>
        /// <returns>Two collections of strings: file names and file infos</returns>
        private List<string>[] FileWalk( string dir )
        {
            // allocate memory for the array of two stringlists
            var fileDescriptions = new List<string>[2];

            // ... and for each stringlist
            fileDescriptions[0] = new List<string>();
            fileDescriptions[1] = new List<string>();


            // FILTER CSV: 
            // select only csv files in the directory chosen by user
            var files = Directory.EnumerateFiles(dir).Where(t => t.EndsWith("csv"));

            // ITERATE:
            // checking each file in the "dir" directory
            foreach (string file in files)
            {
                // preprocess the full filename
                string fileDescription = String.Format("{0} [ {1} ]", Path.GetFileNameWithoutExtension(file), dir.Remove(0, dir.LastIndexOf('\\') + 1));
                
                // add result to the first stringlist we'll return from the method
                fileDescriptions[0].Add( fileDescription );

                // simple parsing of the csv file
                var csv_samples = File.ReadAllText(file).Split(';');

                // add info about the measurements parameters (it is stored in the first row of the csv file)
                measureParams.Add(csv_samples.ElementAt(0));

                // add this info to the second stringlist
                fileDescriptions[1].Add( csv_samples.ElementAt(0) );

                // create new lists for new spectrum
                List<int> spec = new List<int>();
                List<float> spec_points = new List<float>();

                // if there is only one column of data in csv file (i.e. the second element isn't empty)
                if (csv_samples.ElementAt(1) != "")
                {
                    // fill current spectrum except x-axis values
                    for (int j = 0; j < SPECSIZE; j++)
                    {
                        spec.Add(Int32.Parse(csv_samples.ElementAt(j + 1)));
                        spec_points.Add(int.MaxValue);
                    }
                }
                // otherwise include the x-axis values into spectral information
                else
                {
                    // fill current spectrum
                    for (int j = 0; j < SPECSIZE; j++)
                    {
                        spec.Add(Int32.Parse(csv_samples.ElementAt(j * 2 + 2)));
                        spec_points.Add(float.Parse(csv_samples.ElementAt(j * 2 + 3),
                                                            System.Globalization.CultureInfo.InvariantCulture));
                    }
                }

                // fix
                FixSpectrum(spec);

                // add spectrum to the list of spectra
                spectra.Add(spec);
                spectralPoints.Add(spec_points);
            }

            return fileDescriptions;
        }


        /// <summary>
        /// Iterate across the files in a given directory and its sub-directories
        /// and load all DMS contents found in csv files
        /// </summary>
        /// <param name="path">The directory name</param>
        /// <returns>Two collections of strings: file names and file infos</returns>
        public List<string>[] LoadFolderContent( string path )
        {
            // get all child directories of the directory specified by user
            var dirs = Directory.EnumerateDirectories( path );

            // firstly, iterate through files in chosen directory
            var fileDescriptions = FileWalk(path);

            // secondly, read data from all files in each child directory
            foreach (string dir in dirs)
            {
                var subfolderFileDescriptions = FileWalk(dir);
                fileDescriptions[0].AddRange( subfolderFileDescriptions[0] );
                fileDescriptions[1].AddRange( subfolderFileDescriptions[1] );
            }

            return fileDescriptions;
        }


        /// <summary>
        /// Parse the parameters of measurements
        /// </summary>
        /// <param name="nSpectrum">The ordinal number of the DMS spectrum to work with</param>
        /// <param name="fromV">The 1st parsed value</param>
        /// <param name="toV">The 2nd parsed value</param>
        public void ParseMeasureParams( int nSpectrum, ref float fromV, ref float toV )
        {
            // parse measureParams using RegExp
            var col = Regex.Matches( measureParams[nSpectrum], @"(?<key>\s*\w+[,\.]*\w+\s*)=(?<val>\s*\d*[,\.]?\d+\s*)" );

            foreach (Match m in col)
            {
                if (m.Groups["key"].Value.Contains("From,V"))
                {
                    fromV = float.Parse(m.Groups["val"].Value.Replace('.', ','));
                }

                if (m.Groups["key"].Value.Contains("To,V"))
                {
                    toV = float.Parse(m.Groups["val"].Value.Replace('.', ','));
                }
            }
        }
    }
}