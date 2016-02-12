using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BigDay.Models
{
    public class Guest
    {
        public int Id { get; set; }

        public int ReferenceIdentifier { set; get; }
        public Guest RelatedGuest { get; set; }

        [Display(Name = "Food Choice")]
        public string FoodChoice { set; get; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        [StringLength(100)]
        public string Email { get; set; }

        [Display(Name = "Firstname")]
        [Required(ErrorMessage = "Firstname is mandatory")]
        [StringLength(20)]
        public string Firstname { get; set; }

        [Display(Name = "Surname")]
        [Required(ErrorMessage = "Surname is mandatory")]
        [StringLength(20)]
        public string Surname { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Mobile Number")]
        [StringLength(50)]
        public string MobileNumber { get; set; }

        [Display(Name = "Status")]
        [Required(ErrorMessage = "Status is mandatory")]
        public string Status { get; set; }

        public string NickName { get; set; }
    }
}