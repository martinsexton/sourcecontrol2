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
        public int ReferenceId { get; set; }

        public int PartnerId { get; set; }
        public int PartnerReferenceId { get; set; }

        public bool IncludeGuest { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Firstname")]
        [Required(ErrorMessage = "Firstname is mandatory")]
        [StringLength(20)]
        public string Firstname { get; set; }

        [Display(Name = "Surname")]
        [Required(ErrorMessage = "Surname is mandatory")]
        [StringLength(20)]
        public string Surname { get; set; }

        [Display(Name = "Status")]
        [Required(ErrorMessage = "Status is mandatory")]
        public string Status { get; set; }

        public string NickName { get; set; }

        [Display(Name = "Firstname")]
        [StringLength(20)]
        public string PartnerFirstname { get; set; }

        [Display(Name = "Surname")]
        [StringLength(20)]
        public string PartnerSurname { get; set; }
    }
}