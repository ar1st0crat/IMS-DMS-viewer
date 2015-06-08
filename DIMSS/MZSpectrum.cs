using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DIMSS
{
    public class MZSpectrum
    {
        public double[] MZList { set; get; }
        public double[] IntensityList { set; get; }

        public MZSpectrum()
        {
        }

        public int PeakCount()
        {
            return MZList.Count();
        }
    }
}
