﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cactus.Models
{
    public class VersionModel
    {
        public string Version { get; set; }
        public bool IsAlsoExpansion { get; set; }

        // The order in the patch history this version is in.
        public int Order { get; set; }
    }
}