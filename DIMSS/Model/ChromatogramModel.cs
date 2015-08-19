using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;


namespace DIMSS.Model
{
    public class ChromatogramModel
    {
        // Perhaps, this component is no longer needed (seems like it can't decompress mz-spectral data).
        // We can use simple XmlReader / XmlDocument instead. However, currently we use some of MSDataFileReader.clsMzXMLFileAccessor features
        private MSDataFileReader.clsMzXMLFileAccessor mzXMLParser = new MSDataFileReader.clsMzXMLFileAccessor();

        // We start with the spectrum #1. The scan #0 contains metainfo
         private int curMzSpectrum = 1;
         public int CurrentMZSpectrum
         {
             set { curMzSpectrum = value; }
             get { return curMzSpectrum; }
         }

        // Message to show when datafile was loaded succesfully
        public const string LOAD_RESULT_OK = "mzXml file loaded OK";
        

        /// <summary>
        /// Close the file we were parsing
        /// </summary>
        public void CloseParser()
        {
            mzXMLParser.CloseFile();
        }


        /// <summary>
        /// Open mzXml file for parsing
        /// </summary>
        /// <param name="mzxmlFilename">The full name of the mzXml file to open</param>
        /// <returns></returns>
        public string OpenParser( string mzxmlFilename )
        {
            if ( !mzXMLParser.OpenFile(mzxmlFilename) )
            {
                return mzXMLParser.ErrorMessage;
            }
            return LOAD_RESULT_OK;
        }


        /// <summary>
        /// A helping function: decompress a byte array compressed with zlib (mzXML 3.0 format!) 
        /// </summary>
        /// <param name="source">A byte array compressed with zlib</param>
        /// <returns>Decompressed byte array</returns>
        public static byte[] DecompressZlib(Stream source)
        {
            byte[] result = null;

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


        /// <summary>
        /// Parse the region of an mzxml file corresponding to the spectrum with scanId = <paramref name="idx"/> 
        /// </summary>
        /// <param name="idx">ScanID (scan index)</param>
        /// <returns>MZ spectrum by the specified ScanId</returns>
        public MZSpectrum GetMZSpectrumByIndex(int idx)
        {
            MZSpectrum spec = new MZSpectrum();

            string xmlString = "";

            mzXMLParser.GetSourceXMLByScanNumber(idx, ref xmlString);

            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(xmlString);

            // find the encoded and compressed binary data in xml file: the <peaks> tag
            string peaks = doc.SelectSingleNode(@"//peaks").InnerText;

            // Firstly, decode byte array from base64 string
            byte[] byteArray = System.Convert.FromBase64String(peaks);

            // Secondly, decompress the decoded byte array using the SharpZipLib dll
            Stream stream = new MemoryStream(byteArray);

            byte[] decodedBytes = DecompressZlib(stream);

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
                spec.MZList[specPos] = System.BitConverter.ToDouble(mzPeak, 0);
                spec.IntensityList[specPos] = System.BitConverter.ToDouble(intensity, 0);
            }

            return spec;
        }


        public MZSpectrum GetCurrentMZSpectrum()
        {
            return GetMZSpectrumByIndex( CurrentMZSpectrum );
        }


        public int ScanCount()
        {
            return mzXMLParser.ScanCount;
        }
    }
}
