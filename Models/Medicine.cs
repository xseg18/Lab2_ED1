﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2_ED1.Models
{
    public class Medicine
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string HManufact { get; set; }
        public int Qty  { get; set; }
        
    }
}
