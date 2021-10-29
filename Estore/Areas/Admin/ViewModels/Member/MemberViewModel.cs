using System;
using System.ComponentModel.DataAnnotations;

namespace Estore.Areas.Admin.ViewModels
{
    public class MemberViewModel
    {
        [Required]
        [MinLength(2)]
        public String Name { get; set; }
        [Required]
        [EmailAddress]
        public String Email { get; set; }
        [Required]
        [MinLength(5)]
        public String Password { get; set; }
        [Required]
        [MinLength(2)]
        public String City { get; set; }
        [Required]
        [MinLength(2)]
        public String Country { get; set; }
        [Required]
        [MinLength(2)]
        public String CompanyName { get; set; }

        public MemberViewModel()
        {

        }

        public MemberViewModel(string country, string city, string password, string email, string name, string companyName)
        {
            Country = country;
            City = city;
            Password = password;
            Email = email;
            Name = name;
            CompanyName = companyName;
        }
    }
}
