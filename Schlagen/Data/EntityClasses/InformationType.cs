﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Schlagen.Data.EntityClasses
{
    [Serializable]
    public class InformationType
    {
        public int InformationTypeId { get; set; }
        public string Name { get; set; }
    }
}
