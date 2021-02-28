using System;
using System.IO;
using System.Data;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Lab2_ED1.Models;
using Lab2_ED1.Models.Data;
using Microsoft.VisualBasic.FileIO;

namespace Lab2_ED1.Controllers
{
    public class HomeController : Controller
    {
        public static int PosList = 0;
        private IHostingEnvironment Environment;
        public HomeController(IHostingEnvironment _environment)
        {
            Environment = _environment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(IFormFile postedFile)
        {
            string Name = "", Description = "", House = "";
            decimal Price = 0;
            int ID = 0, Qty = 0;
            if (postedFile != null)
            {
                string path = Path.Combine(this.Environment.WebRootPath, "Uploads");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string fileName = Path.GetFileName(postedFile.FileName);
                string filePath = Path.Combine(path, fileName);
                using (TextFieldParser csvParser = new TextFieldParser(filePath))
                {
                    csvParser.CommentTokens = new string[] { "#" };
                    csvParser.SetDelimiters(new string[] { "," });
                    csvParser.HasFieldsEnclosedInQuotes = true;

                    csvParser.ReadLine();

                    while (!csvParser.EndOfData)
                    {
                        string[] fields = csvParser.ReadFields();
                        ID = Convert.ToInt32(fields[0]);
                        Name = fields[1].ToUpper();
                        Description = fields[2];
                        House = fields[3];
                        Price = Convert.ToDecimal(fields[4].Substring(1));
                        Qty = Convert.ToInt32(fields[5]);

                        if (Singleton.Instance3.Medicine.Count() > 0)
                        {
                            var esRepetido = Singleton.Instance3.Medicine.Find(x => x.Name == Name);
                        }
                        else
                        {
                            var newMedicine = new Medicine
                            {
                                ID = ID,
                                Name = Name,
                                Description = Description,
                                HManufact = House,
                                Price = Price,
                                Qty = Qty
                            };
                            Singleton.Instance3.Medicine.Add(newMedicine);
                            Singleton.Instance.Index.Add(Name, PosList);
                            PosList++;
                        }
                    }
                }
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult HPMedicaments()
        {
            return View();
        }

        public IActionResult Order()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
