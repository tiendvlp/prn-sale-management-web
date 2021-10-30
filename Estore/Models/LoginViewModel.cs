using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

using Microsoft.AspNetCore.Http;
using DataAccess.UnitOfWork;
using Estore.common;

namespace Estore.Models
{
    public class LoginViewModel
    {
        [BindProperty]
        public InputModel Input { get; set; }
        public string ErrorMessage { get; set; }

        public LoginViewModel()
        {

        }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
            [Required]
            [MinLength(5)]
            public string Password { get; set; }

            public InputModel()
            {

            }
        }


    }
}
