using System;
using System.ComponentModel.DataAnnotations;

namespace NationalDrivingLicense.Data
{
    public class DriverLicenseCredentials
    {   
        [Key]
        public string UserName { get; set; }
        public string OfferUrl { get; set; }
        public string Did { get; set; }
    }
}
