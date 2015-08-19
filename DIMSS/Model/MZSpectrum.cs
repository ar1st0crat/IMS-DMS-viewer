﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DIMSS.Model
{
    public class MZSpectrum
    {
        public double[] MZList { set; get; }
        public double[] IntensityList { set; get; }

        public int PeakCount()
        {
            return MZList.Count();
        }
    }
}