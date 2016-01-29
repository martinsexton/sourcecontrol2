using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MartinAndOrlasWeddingApplication.Models
{
    public class LoginRequest
    {
        [Required]
        [DataType(DataType.Text)]
        public string Firstname { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Surname { get; set; }
        
        [HiddenInput]
        public string ReturnUrl { get; set; }
    }
}