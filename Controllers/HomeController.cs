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
        public static bool hayCliente = false;
        public static string medName, clientName, clientAdress, clientNIT, Recorrido;
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
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
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

                        if (Singleton.Instance2.Medicine.Count() > 0)
                        {
                            var esRepetido = Singleton.Instance2.Medicine.Find(x => x.Name == Name);
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
                                Singleton.Instance2.Medicine.Add(newMedicine);
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
                            Singleton.Instance2.Medicine.Add(newMedicine);
                            Singleton.Instance.Index.Add(Name, PosList);
                            PosList++;
                        }
                    }
                    ViewData["Message"] = "La carga de la lista se realizó correctamente.";
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
            ViewData["Nombre"] = clientName;
            ViewData["Dirección"] = clientAdress;
            ViewData["NIT"] = clientNIT;
            return View(Singleton.Instance3.Order);
        }

        public IActionResult Cliente()
        {
            if (hayCliente)
            {
                return RedirectToAction(nameof(Order));
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cliente(IFormCollection collection)
        {
            try
            {
                clientName = collection["Name"];
                clientAdress = collection["Adress"];
                clientNIT = collection["NIT"];
                hayCliente = true;
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
                    if (Singleton.Instance2.Medicine[medPos].Qty >= medQty)
                    {
                        return RedirectToAction(nameof(Med));
                    }
                    else
                    {
                        ViewData["QTY"] = "No hay suficientes existencias para satisfacer esta búsqueda.";
                        return View();
                    }
                }
                else
                {
                    ViewData["Index"] = "El medicamento no se encontró en el índice.";
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
            var viewMed = Singleton.Instance2.Medicine[medPos];
            return View(viewMed);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add()
        {
            var Medicine = Singleton.Instance2.Medicine[medPos];

            var newOrder = new Medicine
            {
                ID = Medicine.ID,
                Name = Medicine.Name,
                Description = Medicine.Description,
                HManufact = Medicine.HManufact,
                Price = Medicine.Price,
                Qty = medQty
            };

            Singleton.Instance3.Order.Add(newOrder);
            Singleton.Instance2.Medicine[medPos].Qty -= medQty;

            if (Singleton.Instance2.Medicine[medPos].Qty == 0)
            {
                Singleton.Instance1.ReStock.Add(Singleton.Instance.Index.Find(medName));
                Singleton.Instance.Index.Delete(medName);
            }
            return RedirectToAction(nameof(Order));
        }

        public IActionResult MedRestock()
        {
            @ViewData["Restock"] = Singleton.Instance1.ReStock.Count();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Restock()
        {
            Random random = new Random();
            foreach (var item in Singleton.Instance1.ReStock)
            {
                Singleton.Instance2.Medicine[item].Qty = random.Next(1, 16);
                Singleton.Instance.Index.Add(Singleton.Instance2.Medicine[item].Name, item);
            }
            Singleton.Instance1.ReStock.Clear();

            if (hayCliente)
            {
                return RedirectToAction(nameof(Order));
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Finalizar()
        {
            decimal totalPay = 0;
            ViewData["Nombre"] = clientName;
            ViewData["Dirección"] = clientAdress;
            ViewData["NIT"] = clientNIT;
            ViewData["Cantidad"] = Singleton.Instance3.Order.Count();
            foreach (var item in Singleton.Instance3.Order)
            {
                totalPay += item.Qty * item.Price;
            }
            ViewData["Total"] = totalPay;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Confirmar()
        {
            Singleton.Instance3.Order.Clear();
            hayCliente = false;
            return RedirectToAction(nameof(Index));
        }

        public IActionResult printIndex(string recorrido)
        {
            if (recorrido == "Pre")
            {
                Recorrido = Singleton.Instance.Index.PreOrder(Singleton.Instance.Index.Root);
            }
            else if (recorrido == "Post")
            {
                Recorrido = Singleton.Instance.Index.PostOrder(Singleton.Instance.Index.Root);
            }
            else if (recorrido == "In")
            {
                 Recorrido = Singleton.Instance.Index.InOrder(Singleton.Instance.Index.Root);
            }
            Singleton.Instance.Index.Order = "";
            ViewData["Recorrido"] = Recorrido;
            return View();
        }

        public IActionResult Print()
        {
            StreamWriter writer = new StreamWriter("Index_File.txt");
            writer.Write(Recorrido);
            writer.Close();
            return RedirectToAction(nameof(Order));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
