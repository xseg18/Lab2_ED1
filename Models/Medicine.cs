using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Lab2_ED1.Models
{
    public class Medicine
    {
        [Display(Name = "Identificador")]
        [Required]
        public int ID { get; set; }
        [Display(Name = "Nombre")]
        [Required]
        public string Name { get; set; }
        [Display(Name = "Precio")]
        [Required]
        public decimal Price { get; set; }
        [Display(Name = "Descripción")]
        [Required]
        public string Description { get; set; }
        [Display(Name = "Manufacturador")]
        [Required]
        public string HManufact { get; set; }
        [Display(Name = "Cantidad")]
        [Required]
        public int Qty  { get; set; }
        
    }
}
