using NationalDrivingLicense.MattrOpenApiClient;
using System;
using System.ComponentModel.DataAnnotations;

namespace NationalDrivingLicense.Data
{
    public class DriverLicenseCredentials
    {   
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public V1_CreateOidcIssuerResponse OidcIssuer { get; set; }
        public V1_CreateDidResponse Did { get; set; }
    }
}
