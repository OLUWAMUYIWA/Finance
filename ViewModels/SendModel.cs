using System;
using System.ComponentModel.DataAnnotations;

namespace ListaccFinance.API.SendModel

{
    public class SyncLoginModel
    {
        [Required]
        public string ClientName { get; set; }
        [Required]
        public string ClientMacAddress { get; set; }
        [Required]
        public string ClientType { get; set; }

        [Required]
        public string Password {get; set;}

        [Required]
        [EmailAddress(ErrorMessage="Enter a valid email address.")]
        public string EmailAddress {get; set;}
    }

    public class UserLogin 
    {
        [Required]
        public string Password { get; set; }

        [Required]
        [EmailAddress(ErrorMessage="Enter a valid email address.")]
        public string EmailAddress { get; set; }
    }

    public class RegisterModel
    {
        [Required()]
        public string firstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        [Required]            
        public string Phone { get; set; }
        public string Address { get; set; }
        [Required]
        [EmailAddress(ErrorMessage="Enter a valid email address.")]
        public string EmailAddress {get; set;}
        public string Password { get; set; }
        public string Department {get; set;}
        public int? DepartmentId {get; set;}

        public string Role {get; set;}
        public string Status {get; set;}
        
    }


    public class DesktopCreateModel
    {

        [Required]
        public string ClientName { get; set; }
        [Required]
        public string ClientMacAddress { get; set; }
        [Required]
        public string ClientType { get; set; }
    }

    public class DataDownload
    {
        [Required]
        public string MacAddress {get; set;}
        public DateTime lastUpdate {get; set;}
    }


}