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
        public Guid OidcIssuerId { get; set; }
        public string OidcIssuer { get; set; }
        public string Did { get; set; }
    }
}
