using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BigDay.Models
{
    public class LoginRequest
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "First Name")]
        public string Firstname { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name="Surname")]
        public string Surname { get; set; }

        [HiddenInput]
        public string ReturnUrl { get; set; }
    }
}