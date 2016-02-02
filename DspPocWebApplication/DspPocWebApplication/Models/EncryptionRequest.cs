using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DspPocWebApplication.Models
{
    public class EncryptionRequest
    {
        public EncryptionRequest()
        {
            //Default Value
            PassSecret = "DPS_Test";
        }
        [Required(ErrorMessage = "Pass Secret Must Be Provided")]
        public string PassSecret { get; set; }
        [Required(ErrorMessage = "Month Day Must Be Provided")]
        public int MonthDay { get; set; }
        [Required(ErrorMessage = "Month Must Be Provided")]
        public int Month { get; set; }
        [Required(ErrorMessage = "Doc Contract Must Be Provided")]
        public string DocContract { get; set; }
        [Display(Name = "Generated Hash")]
        public string EncryptedText { get; set; }
    }
}