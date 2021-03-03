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
        public static string medName;
        public static int PosList = 0, medPos, medQty;

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
                            if (esRepetido == null)
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

        public IActionResult Order()
        {
            return View(Singleton.Instance4.Order);
        }

        public IActionResult Cliente()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cliente(IFormCollection collection)
        {
            try
            {
                var newClient = new Models.Client
                {
                    name = collection["name"],
                    Adress = collection["Adress"],
                    NIT = Convert.ToInt32(collection["NIT"])
                };
                return RedirectToAction(nameof(Order));
            }
            catch
            {
                return View();
            }
        }

        public IActionResult Search()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Search(IFormCollection collection)
        {
            try
            {
                medName = collection["Name"].ToString().ToUpper();
                medPos = Singleton.Instance.Index.Find(medName);
                if (medPos > -1)
                {
                    medQty = Convert.ToInt32(collection["Qty"]);
                    if (Singleton.Instance3.Medicine[medPos].Qty >= medQty)
                    {
                        return RedirectToAction(nameof(Med));
                    }
                    else
                    {
                        //No hay ecistencia
                        return View();
                    }
                }
                else
                {
                    //No se encuentra en el índice
                    return View();
                }
            }
            catch
            {
                return View();
            }            
        }

        public IActionResult Med()
        {
            var viewMed = Singleton.Instance3.Medicine[medPos];
            return View(viewMed);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add()
        {
            var Medicine = Singleton.Instance3.Medicine[medPos];

            var newOrder = new Medicine
            {
                ID = Medicine.ID,
                Name = Medicine.Name,
                Description = Medicine.Description,
                HManufact = Medicine.HManufact,
                Price = Medicine.Price,
                Qty = medQty
            };

            Singleton.Instance4.Order.Add(newOrder);
            Singleton.Instance3.Medicine[medPos].Qty -= medQty;

            if (Singleton.Instance3.Medicine[medPos].Qty == 0)
            {
                Singleton.Instance.Index.Delete(medName);
            }
            return RedirectToAction(nameof(Order));
        }

        public IActionResult Restock()
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
