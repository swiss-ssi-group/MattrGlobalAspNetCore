namespace NationalDrivingLicense.Services
{
    public class MattrCreateDid
    {
        //{
        //  "method": "key"
        //  "options": {
        //    "keyType": "ed25519"
        //  }
        //}

        
        public string Method { get; set; } = "key";
        public class Options
        {
            /// <summary>
            /// The supported key types for the DIDs are ed25519 and bls12381g2. 
            /// If the keyType is omitted, the default key type that will be used is ed25519.
            /// 
            /// If the keyType in options is set to bls12381g2 a DID will be created with 
            /// a BLS key type which supports BBS+ signatures for issuing ZKP-enabled credentials.
            /// </summary>
            public string KeyType { get; set; } = "ed25519";
        }
    }
}
