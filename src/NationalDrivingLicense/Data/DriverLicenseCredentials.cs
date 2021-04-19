using NationalDrivingLicense.MattrOpenApiClient;
using System;
using System.ComponentModel.DataAnnotations;

namespace NationalDrivingLicense.Data
{
    public class DriverLicenseCredentials
    {   
        [Key]
        public int Id { get; set; }
        public V1_CreateOidcIssuerResponse V1_CreateOidcIssuerResponse { get; set; }
        public string Did { get; set; }
    }
}
