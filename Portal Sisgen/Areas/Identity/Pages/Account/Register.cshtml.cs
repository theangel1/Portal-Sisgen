using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using Portal_Sisgen.Models;
using Portal_Sisgen.Utility;
using Newtonsoft.Json;

namespace Portal_Sisgen.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager; //SE AGREGA PARA GENERAR ROLES

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager) //SE AGREGA PARA GENERAR ROLES
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager; //SE AGREGA PARA GENERAR ROLES
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            //DATOS INCLUIDOS EN EL FORMULARIO DE REGISTRO
            public string Nombre { get; set; }

            public string RazonSocial { get; set; }

            public string PhoneNumber { get; set; }


        }

        public void OnGet(string returnUrl = null)
        {
            //ACA SE RECUPERA EL OBJETO CONTRIBUYENTE DEL LA LISTA 
            ReturnUrl = returnUrl;
            ViewData["tempContribuyente"] = GetContribuyentes();            
        }


        public List<Contribuyente> GetContribuyentes()
        {   
            //ACA SE CREA LA LISTA DE CONTRIBUYENTES TRAIDA DESDE LA BASE DE DATOS MYSQL
            var query = "select * from sis_contribuyente";
            var listaContribuyente = new List<Contribuyente>();

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
                        Contribuyente _contri = new Contribuyente();
                        _contri.Sis_contribuyente_id = int.Parse(dr["sis_contribuyente_id"].ToString());
                        _contri.Sis_contribuyente_razon = dr["sis_contribuyente_razon"].ToString();
                        listaContribuyente.Add(_contri);
                    }
                    conexion.Close();

                    return listaContribuyente;
                }


            }
            catch (Exception)
            {

                throw;
            }

        }




        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    Nombre = Input.Nombre,
                    PhoneNumber = Input.PhoneNumber,
                    RazonSocial = Input.RazonSocial
                };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    //ingresar segun rol
                    if (!await _roleManager.RoleExistsAsync(SD.AdminUser))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(SD.AdminUser));
                    }

                    if (!await _roleManager.RoleExistsAsync(SD.ClientUser))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(SD.ClientUser));
                    }

                    //Esta linea configura el tipo de rol que toma el usuario al registrarce
                    await _userManager.AddToRoleAsync(user, SD.ClientUser);

                    _logger.LogInformation("User created a new account with password.");

                    //CONFIRMACION POR CORREO ELECTRONICO
                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //var callbackUrl = Url.Page(
                    //    "/Account/ConfirmEmail",
                    //    pageHandler: null,
                    //    values: new { userId = user.Id, code = code },
                    //    protocol: Request.Scheme);

                    //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    //ESTA LINEA SE SUBE??
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
