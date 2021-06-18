using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Portal_Sisgen.Models
{
    public class ApplicationUser : IdentityUser
    {
        //ESTOS DATOS SON AGRAGADOS PARA EL REGISTRO DEL USUARIO
        [Required]
        public string Nombre { get; set; }
        public int ContribuyenteId { get; set; }
        public string RazonSocial { get; set; }        

    }
}
