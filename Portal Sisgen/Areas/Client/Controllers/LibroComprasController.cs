using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Portal_Sisgen.Models;
using Portal_Sisgen.Utility;

namespace Portal_Sisgen.Areas.Client.Controllers
{
    [Area("Client")]
    public class LibroComprasController : Controller
    {
        public LibroComprasController()
        {
            /*
             *Comentarios del programador 
             Controlador creado para pruebas con mysql
             */
        }

        public IActionResult Index()
        {
            var query = "select * from libro_compra";
            var listaLibros = new List<LibroCompra>();
            try
            {
                using (MySqlConnection conexion = new MySqlConnection(SD.conexionMySql))
                {
                    var cmd = new MySqlCommand(query);
                    cmd.Connection = conexion;
                    cmd.CommandType = CommandType.Text;
                    conexion.Open();
                    cmd.ExecuteNonQuery();
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    foreach (DataRow dr in dt.Rows)
                    {
                        LibroCompra _book = new LibroCompra();

                        _book.Libro_compra_ano = int.Parse(dr["libro_compra_ano"].ToString());
                        _book.Libro_compra_trackID = dr["libro_compra_trackid"].ToString();
                        listaLibros.Add(_book);
                    }


                    conexion.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return View(listaLibros);
        }
    }
}