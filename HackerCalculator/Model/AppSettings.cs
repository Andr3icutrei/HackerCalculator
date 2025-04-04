﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackerCalculator.Model
{
    public class AppSettings
    {
        public bool IsDigitGroupingActive { get; set; }
        public bool IsStandardMode { get; set; }
        public String FromBase { get; set; }
        public String ToBase { get; set; }

        public AppSettings()
        {
            IsDigitGroupingActive = false;
            IsStandardMode = true;
            FromBase = "Decimal";
            ToBase = "Decimal";
        }
    }
}
