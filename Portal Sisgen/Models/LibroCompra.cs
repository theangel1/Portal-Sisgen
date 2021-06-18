using Portal_Sisgen.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portal_Sisgen.Models
{
    public class LibroCompra
    {        
        public int Sis_contribuyente_id { get; set; }
        public int Libro_compra_ano { get; set; }
        public int Libro_compra_mes { get; set; }
        public string Libro_compra_trackID { get; set; }
        public string Libro_compra_estado { get; set; }
    }
}
