using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Lab2_ED1.Models
{
    public class Client
    {
        [Display(Name = "Nombre")]
        [Required]
        public string name { get; set; }
        [Display(Name = "Dirección")]
        [Required]
        public string Adress { get; set; }
        [Display(Name = "NIT")]
        [Required]
        public int NIT { get; set; }
        public int Total { get; set; }
        public ELineales.Lista<Medicine> Medicines { get; set; }
    }
}
