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

namespace Lab2_ED1.Controllers
{
    public class HomeController : Controller
    {
        public static bool valid = false;
        public static string NameC = "";
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
            string ID = "", Name = "", Description = "", House = "";
            decimal Price = 0;
            int Qty = 0;
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
                string csvData = System.IO.File.ReadAllText(filePath);
                bool firstRow = true;
                foreach (string row in csvData.Split('\n'))
                {
                    if (!string.IsNullOrEmpty(row))
                    {
                        if (!string.IsNullOrEmpty(row))
                        {
                            if (firstRow)
                            {
                                firstRow = false;
                            }
                            else
                            {
                                int y = 0;
                                foreach (string cell in row.Split(','))
                                {
                                    if(y == 0)
                                    {
                                        ID = cell.Trim();
                                        y++;
                                    }
                                    else if(y == 1)
                                    {
                                        Name = cell.Trim().ToUpper();
                                        if (Singleton.Instance3.Medicine.Count() > 0)
                                        {
                                            var Repetido = Singleton.Instance3.Medicine.Find(x => x.Name == Name);
                                        }
                                        y++;
                                    }
                                    else if (y == 2)
                                    {
                                        Description = cell.Trim();
                                        y++;
                                    }
                                    else if (y == 3)
                                    {
                                        House = cell.Trim();
                                        y++;
                                    }
                                    else if (y == 4)
                                    {
                                        Price = Convert.ToDecimal(cell.Substring(0).Trim());
                                        y++;
                                    }
                                    else
                                    {
                                        Qty = Convert.ToInt32(cell.Trim());
                                        y++;
                                    }
                                }
                            }
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

        //↓↓↓↓ PROBARLO CUANDO SE PUEDA ↓↓↓↓
        public void  RepeatedSearcher (Medicine m)
        {
           
        }
    }
}
