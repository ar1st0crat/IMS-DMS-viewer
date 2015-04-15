using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DIMSS
{
    class MZSpectrum
    {
        public double[] MZList;
        public double[] intensityList;

        public MZSpectrum()
        {
        }

        public int PeakCount()
        {
            return MZList.Count();
        }
    }
}
