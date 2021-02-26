using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2_ED1.Models
{
    public class Client
    {
        public string name { get; set; }
        public string Adress { get; set; }
        public int NIT { get; set; }
        public int Total { get; set; }
        public ELineales.Lista<Medicine> medicines { get; set; }
    }
}
