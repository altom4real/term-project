using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SQL_DENEME.Models;

namespace SQL_DENEME.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<Calısanlar> CalisanList = new List<Calısanlar>();

            using (SqlConnection conn = new SqlConnection("Server=MSI;Database=Calısanlar;Trusted_Connection=True;"))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT Id,Name,Surname,City,Photo,Email,Position,GitHub,LinkedIn FROM Calısanlar", conn))
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        CalisanList.Add(new Calısanlar
                        {
                            Id = (int)dr["Id"],
                            Name = (string)dr["Name"],
                            Surname = (string)dr["Surname"],
                            City = (string)dr["City"],
                            Photo = (string)dr["Photo"],
                            Email = (string)dr["Email"],
                            Position = (string)dr["Position"],
                            GitHub = (string)dr["GitHub"],
                            LinkedIn = (string)dr["LinkedIn"]
                        });
                    }
                }
            }

            return View(CalisanList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Details(int id)
        {
            Calısanlar calisan = GetCalisanById(id);

            if (calisan == null)
            {
                return NotFound();
            }

            return View(calisan);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Calısanlar calisan)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection conn = new SqlConnection("Server=MSI;Database=Calısanlar;Trusted_Connection=True;"))
                {
                    conn.Open();

                    
                    string sql = "INSERT INTO Calısanlar (Id, Name, Surname, City, Photo, Email, Position, GitHub, LinkedIn) " +
                                 "VALUES (@Id, @Name, @Surname, @City, @Photo, @Email, @Position, @GitHub, @LinkedIn)";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        
                        cmd.Parameters.AddWithValue("@Id", calisan.Id);
                        cmd.Parameters.AddWithValue("@Name", calisan.Name);
                        cmd.Parameters.AddWithValue("@Surname", calisan.Surname);
                        cmd.Parameters.AddWithValue("@City", calisan.City);
                        cmd.Parameters.AddWithValue("@Photo", calisan.Photo);
                        cmd.Parameters.AddWithValue("@Email", calisan.Email);
                        cmd.Parameters.AddWithValue("@Position", calisan.Position);
                        cmd.Parameters.AddWithValue("@GitHub", calisan.GitHub);
                        cmd.Parameters.AddWithValue("@LinkedIn", calisan.LinkedIn);

                        cmd.ExecuteNonQuery();
                    }
                }

                return RedirectToAction("Index");
            }

            return View(calisan);
        }




        public IActionResult Edit(int id)
        {
            Calısanlar calisan = GetCalisanById(id);

            if (calisan == null)
            {
                return NotFound();
            }

            return View(calisan);
        }

        [HttpPost]
        public IActionResult Edit(Calısanlar calisan)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection("Server=MSI;Database=Calısanlar;Trusted_Connection=True;"))
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand("UPDATE Calısanlar SET Name = @Name, Surname = @Surname, City = @City, Email = @Email, Position = @Position WHERE Id = @Id", conn))
                        {
                            cmd.Parameters.AddWithValue("@Id", calisan.Id);
                            cmd.Parameters.AddWithValue("@Name", calisan.Name);
                            cmd.Parameters.AddWithValue("@Surname", calisan.Surname);
                            cmd.Parameters.AddWithValue("@City", calisan.City);
                            cmd.Parameters.AddWithValue("@Email", calisan.Email);
                            cmd.Parameters.AddWithValue("@Position", calisan.Position);

                            int affectedRows = cmd.ExecuteNonQuery();

                            if (affectedRows > 0)
                            {
                                
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                
                                _logger.LogError("Update in the database failed.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"An error occurred during the update process: {ex.Message}");
                    return View(calisan);
                }
            }

            return View(calisan);
        }




        public IActionResult Delete(int id)
        {
            Calısanlar calisan = GetCalisanById(id);

            if (calisan == null)
            {
                return NotFound();
            }

            return View(calisan);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            using (SqlConnection conn = new SqlConnection("Server=MSI;Database=Calısanlar;Trusted_Connection=True;"))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("DELETE FROM Calısanlar WHERE Id = @Id", conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }

            return RedirectToAction("Index");
        }

        private Calısanlar GetCalisanById(int id)
        {
            using (SqlConnection conn = new SqlConnection("Server=MSI;Database=Calısanlar;Trusted_Connection=True;"))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT Id,Name,Surname,City,Photo,Email,Position,GitHub,LinkedIn FROM Calısanlar WHERE Id = @Id", conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            return new Calısanlar
                            {
                                Id = (int)dr["Id"],
                                Name = (string)dr["Name"],
                                Surname = (string)dr["Surname"],
                                City = (string)dr["City"],
                                Photo = (string)dr["Photo"],
                                Email = (string)dr["Email"],
                                Position = (string)dr["Position"],
                                GitHub = (string)dr["GitHub"],
                                LinkedIn = (string)dr["LinkedIn"]
                            };
                        }
                    }
                }
            }

            return null;

        }

    }

}

