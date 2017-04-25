namespace DIMSS.Model
{
    public class Chromatogram
    {
        /// <summary>
        /// The object that does all the heavy-lifting related to parsing mzxml files
        /// </summary>
        private MZXMLParser _mzXMLParser = new MZXMLParser();

        /// <summary>
        /// Index of an MZ spectrum in chromatogram currently chosen for consideration
        /// </summary>
        public int CurrentMZSpectrum { get; set; }


        public Chromatogram()
        {
            // We start with the spectrum #1 (the scan #0 contains metainfo)
            CurrentMZSpectrum = 1;
        }

        #region MZXMLParser proxy functions

        public int ScanCount
        {
            get { return _mzXMLParser.ScanCount; }
        }

        public MZSpectrum GetCurrentMZSpectrum()
        {
            return _mzXMLParser.GetMZSpectrumByIndex(CurrentMZSpectrum);
        }

        public MZSpectrum GetMZSpectrumByIndex(int idx)
        {
            return _mzXMLParser.GetMZSpectrumByIndex(idx);
        }

        /// <summary>
        /// Load mzXml file into model and create mzxml parser object
        /// </summary>
        /// <param name="mzxmlFilename">The full name of the mzXml file to open</param>
        /// <returns>Success or error message</returns>
        public string Load(string mzxmlFilename)
        {
            return _mzXMLParser.Open(mzxmlFilename);
        }

        #endregion
    }
}