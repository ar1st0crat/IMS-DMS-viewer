using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace DIMSS.Model
{
    /// <summary>
    /// Class repsonsible for efficient loading, parsing and accessing 
    /// particular spectra from large mzxml files
    /// </summary>
    class MZXMLParser
    {
        #region string constants

        /// <summary>
        /// Message to show when datafile was loaded succesfully
        /// </summary>
        public const string LoadSuccessMessage = "Mzxml file was loaded and parsed succesfully!";

        /// <summary>
        /// Message to show when the fragment of mzxml file could not be read or parsed
        /// </summary>
        public const string ReadErrorMessage = "Error: could not read mzXml file!";

        /// <summary>
        /// Message to show when an mzxml file does not exist
        /// </summary>
        public const string NotExistMessage = "Error: mzxml file does not exist!";

        /// <summary>
        /// Message to show when mzxml file violates its xml schema
        /// </summary>
        public const string FileCorruptedMessage = "Error: mzxml file is corrupted!";

        #endregion

        /// <summary>
        /// Entire string containing mzxml file XML-markup
        /// </summary>
        private string _xmlContent;

        /// <summary>
        /// Hash added for efficiency:
        /// 
        /// Key: scanID;
        /// Value: base64-coded string containing info about mz spectra (mz peaks and intensities);
        /// 
        /// Keys and values are extracted from an mzxml file.
        /// </summary>
        private Dictionary<int, string> _codedSpectraDictionary = new Dictionary<int, string>();

        /// <summary>
        /// Number of spectra in chromatogram (scans)
        /// </summary>
        public int ScanCount { get; private set; }

        /// <summary>
        /// Method tries to load the content of mzxml file and find out the ScanCount
        /// </summary>
        /// <param name="mzxmlFilename"></param>
        /// <returns>Error or success message</returns>
        public string Open(string mzxmlFilename)
        {
            if (!File.Exists(mzxmlFilename))
            {
                return NotExistMessage;
            }

            _xmlContent = File.ReadAllText(mzxmlFilename);

            try
            {
                var doc = new XmlDocument();
                doc.LoadXml(_xmlContent);
            }
            catch (XmlException)
            {
                return FileCorruptedMessage;
            }

            // directly working with a string instead of XmlDocument is much faster
            // (although the code is less readable)
            var countTag = "msRun scanCount=";
            var scanCountPos = _xmlContent.IndexOf(countTag) + countTag.Length + 1;
            var endPos = _xmlContent.IndexOf('"', scanCountPos);

            // set ScanCount right away
            ScanCount = int.Parse(_xmlContent.Substring(scanCountPos, endPos - scanCountPos));

            // iterate sequentially across all nodes containg info regarding mz peaks
            var peaksPos = 0;
            for (int idx = 1; idx <= ScanCount; idx++)
            {
                peaksPos = _xmlContent.IndexOf("<peaks", peaksPos) + 7;
                peaksPos = _xmlContent.IndexOf(">", peaksPos) + 1;
                endPos = _xmlContent.IndexOf("</peaks>", peaksPos);

                // add base64-coded peaks and intensities to hash
                _codedSpectraDictionary[idx] = _xmlContent.Substring(peaksPos, endPos - peaksPos);
            }

            return LoadSuccessMessage;
        }

        /// <summary>
        /// Parse the region of an mzxml file corresponding to the spectrum with scanId = <paramref name="idx"/> 
        /// </summary>
        /// <param name="idx">ScanID (scan index)</param>
        /// <returns>MZ spectrum by the specified ScanId</returns>
        public MZSpectrum GetMZSpectrumByIndex(int idx)
        {
            // try out all loadings and parsings
            try
            {
                // Retrieve base64-coded string efficiently from hash
                string peaks = _codedSpectraDictionary[idx];

                // Firstly, decode byte array from base64 string
                byte[] byteArray = Convert.FromBase64String(peaks);

                // Secondly, decompress the decoded byte array using the SharpZipLib dll
                Stream stream = new MemoryStream(byteArray);
                byte[] decodedBytes = DecompressZlib(stream);

                MZSpectrum spec = new MZSpectrum();
                // allocate memory for lists of m-z peaks positions and intensities
                spec.MZList = new double[decodedBytes.Count() / 16];
                spec.IntensityList = new double[decodedBytes.Count() / 16];

                // each value is stored as 'double' and little-endian
                // hence we allocate 8 bytes for each value
                byte[] mzPeak = new byte[8];
                byte[] intensity = new byte[8];

                for (int i = 0, specPos = 0; i < decodedBytes.Count(); i += 16, specPos++)
                {
                    // ... and rewrite bytes in reverse order
                    for (int j = 0; j < 8; j++)
                    {
                        mzPeak[j] = decodedBytes[7 + i - j];
                        intensity[j] = decodedBytes[15 + i - j];
                    }

                    // add new peak to spectrum info
                    spec.MZList[specPos] = BitConverter.ToDouble(mzPeak, 0);
                    spec.IntensityList[specPos] = BitConverter.ToDouble(intensity, 0);
                }

                return spec;
            }
            // not processing this error here: 
            // just set the resulting spectrum as null: the callee will handle this error more correctly
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// A helping function: decompress a byte array compressed with zlib (mzXML 3.0 format!) 
        /// </summary>
        /// <param name="source">A byte array compressed with zlib</param>
        /// <returns>Decompressed byte array</returns>
        private static byte[] DecompressZlib(Stream source)
        {
            byte[] result;

            using (var outStream = new MemoryStream())
            {
                using (var inf = new InflaterInputStream(source))
                {
                    inf.CopyTo(outStream);
                }
                result = outStream.ToArray();
            }

            return result;
        }
    }
}
