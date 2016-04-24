using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BigDay.Models
{
    public class WeddingGuest
    {
        public int Id { get; set; }

        public bool IncludeGuest { get; set; }

        [Display(Name = "Firstname")]
        [Required(ErrorMessage = "Firstname is mandatory")]
        [StringLength(20)]
        public string Firstname { get; set; }

        [Display(Name = "Surname")]
        [Required(ErrorMessage = "Surname is mandatory")]
        [StringLength(20)]
        public string Surname { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Special Dietry Requirements")]
        public string DietComment { get; set; }

        [Display(Name = "Status")]
        [Required(ErrorMessage = "Status is mandatory")]
        public string Status { get; set; }

        [Display(Name = "Name of Guest")]
        [StringLength(20)]
        public string GuestName { get; set; }
    }
}