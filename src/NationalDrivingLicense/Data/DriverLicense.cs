using System;
using System.ComponentModel.DataAnnotations;

namespace NationalDrivingLicense.Data
{
    public class DriverLicense
    {   
        public string UserName { get; set; }
        public string IssuedAt { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string DateOfBirth { get; set; }
        public string LicenseType { get; set; }
    }
}
