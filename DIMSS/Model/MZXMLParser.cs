using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace DIMSS.Model
{
    class MZXMLParser
    {
        #region string constants

        /// <summary>
        /// Message to show when datafile was loaded succesfully
        /// </summary>
        public const string LoadSuccessMessage = "MzXml file loaded OK!";

        /// <summary>
        /// Message to show when the fragment of mzxml file could not be read or parsed
        /// </summary>
        public const string ReadErrorMessage = "Could not read mzXml file!";

        #endregion

        /// <summary>
        /// Entire string containing mzxml file XML-markup
        /// </summary>
        private string _xmlContent;

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
                return "MzXML file does not exist!";
            }

            _xmlContent = File.ReadAllText(mzxmlFilename);

            try
            {
                var doc = new XmlDocument();
                doc.LoadXml(_xmlContent);
            }
            catch (XmlException)
            {
                return "MzXML file is corrupted!";
            }

            // directly working with a string instead of XmlDocument is much faster
            // (although the code is less readable)
            var scanCountPos = _xmlContent.IndexOf("msRun scanCount=") + 17;
            var endPos = _xmlContent.IndexOf('"', scanCountPos);

            ScanCount = int.Parse(_xmlContent.Substring(scanCountPos, endPos - scanCountPos));

            var peaksPos = 0;
            for (int idx = 1; idx <= ScanCount; idx++)
            {
                peaksPos = _xmlContent.IndexOf("<peaks", peaksPos) + 7;
                peaksPos = _xmlContent.IndexOf(">", peaksPos) + 1;
                endPos = _xmlContent.IndexOf("</peaks>", peaksPos);

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
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
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

            using (MemoryStream outStream = new MemoryStream())
            {
                using (InflaterInputStream inf = new InflaterInputStream(source))
                {
                    inf.CopyTo(outStream);
                }
                result = outStream.ToArray();
            }

            return result;
        }
    }
}
