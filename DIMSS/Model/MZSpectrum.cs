namespace DIMSS.Model
{
    /// <summary>
    /// MZ spectrum is essentially described by its list of mzs and intensities
    /// </summary>
    public class MZSpectrum
    {
        public double[] MZList { set; get; }
        public double[] IntensityList { set; get; }

        public int PeakCount 
        {
            get { return MZList.Length; }
        }
    }
}
