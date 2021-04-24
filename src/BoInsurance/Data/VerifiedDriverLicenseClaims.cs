﻿using System.Text.Json.Serialization;

namespace BoInsurance.Data
{
    public class VerifiedDriverLicenseClaims
    {
        public string Id { get; set; }

        [JsonPropertyName("http://schema.org/name")]
        public string Name { get; set; }

        [JsonPropertyName("http://schema.org/firstName")]
        public string FirstName { get; set; }

        [JsonPropertyName("http://schema.org/licenseType")]
        public string LicenseType { get; set; }

        [JsonPropertyName("http://schema.org/dateOfBirth")]
        public string DateOfBirth { get; set; }

        [JsonPropertyName("http://schema.org/licenseIssuedAt")]
        public string LicenseIssuedAt { get; set; }
    }
}