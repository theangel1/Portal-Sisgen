using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Portal_Sisgen.Utility;

namespace Portal_Sisgen.Areas.User.Controllers
{
    [Area("Client")]
    public class EstadoDteController : Controller
    {
        //[HttpPost]
        //[ValidateAntiForgeryToken]



        public IActionResult Index()//( string rut )
        {
            /*string query = "SELECT DATO from TABLA where DATO A COMPARAR'" + rut + "';";

            try
            {
                using (MySqlConnection conexion = new MySqlConnection(SD.chain))
                {
                    MySqlCommand cmd = new MySqlCommand(query);

                    cmd.Connection = conexion;
                    cmd.CommandType = CommandType.Text;
                    conexion.Open();
                    cmd.ExecuteNonQuery();

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    foreach (DataRow dr in dt.Rows)
                    {
                        //ViewData["response"] = "ID de su Contribuyente: " + dr["sis_contribuyente_id"].ToString();
                    }
                    conexion.Close();
                }
            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex.ToString();
            }*/



            return View();
        }
    }
}