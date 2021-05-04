# MATTR Global ASP.NET Core

## Blogs

- [Getting started with Self Sovereign Identity SSI](https://damienbod.com/2021/03/29/getting-started-with-self-sovereign-identity-ssi/)
- [Create an OIDC credential Issuer with MATTR and ASP.NET Core](https://damienbod.com/2021/05/03/create-an-oidc-credential-issuer-with-mattr-and-asp-net-core/)

## Test run the applications

## NationalDrivingLicense (OIDC Credential Issuer)

 - Get an account from Mattr (see MATTR docs)
 - Add the secrets to your configuration
 - Initialize your database
 - Install a MATTR Wallet on your phone
 - start application 

## BoInsurance (Verification using credentials from Wallet)

 - Install ngrok for the verifier application (npm)
 - Add the secrets to your configuration
 - Initialize your database
 - Start application using for example http://localhost:5000
 - Start ngrok using **ngrok http localhost:5000** (like above)
 - Copy the DID for the OIDC Issuer Credentials from the NationalDrivingLicense UI
 - Create a presentation template in the BoInsurance (Use copied DID)
 - Verify using the wallet and the application

## secrets

```
{
  // use user secrets
  "ConnectionStrings": {
    "DefaultConnection": "--your-connection-string--"
  },
  "MattrConfiguration": {
    "Audience": "https://vii.mattr.global",
    "ClientId": "--your-client-id--",
    "ClientSecret": "--your-client-secret--",
    "TenantId": "--your-tenant--",
    "TenantSubdomain": "--your-tenant-sub-domain--",
    "Url": "http://mattr-prod.au.auth0.com/oauth/token"
  },
  "Auth0": {
    "Domain": "--your-auth0-domain",
    "ClientId": "--your--auth0-client-id--",
    "ClientSecret": "--your-auth0-client-secret--",
  }
  "Auth0Wallet": {
    "Domain": "--your-auth0-wallet-domain",
    "ClientId": "--your--auth0-wallet-client-id--",
    "ClientSecret": "--your-auth0-wallet-client-secret--",
  }
}
```

## Creating Migrations

### Console

```
dotnet ef migrations add ndl_init
```

### Powershell

```
Add-Migration "ndl_init"
```

## Running Migrations

### Console

```
dotnet restore

dotnet ef database update --context NationalDrivingLicenseMattrContext
```

### Powershell

```
Update-Database 
```


## Links

https://mattr.global/

https://mattr.global/get-started/

https://learn.mattr.global/

https://keybase.io/

https://www.youtube.com/watch?v=2_TDN-81ytM

https://learn.mattr.global/tutorials/dids/did-key

https://gunnarpeipman.com/httpclient-remove-charset/

https://www.lfph.io/wp-content/uploads/2021/02/Verifiable-Credentials-Flavors-Explained.pdf

https://www.xtseminars.co.uk/post/introduction-to-the-future-of-identity-dids-vcs

# Mattr.Global instructions 

In order to obtain a Credential on the mobile wallet you will need to use the OIDC Bridge, so try following this tutorial.

https://learn.mattr.global/tutorials/issue/oidc-bridge/issue-oidc

At the end of the tutorial you will have a client-bound Credential stored on the mobile wallet.
You can then move to Verify a Credential tutorials, first setup a Presentation Template:

https://learn.mattr.global/tutorials/verify/presentation-request-template

Then you can setup your tenant to run the Verify flow, a quick way of doing that is to use a Sample App to orchestrate a number of steps: 

https://learn.mattr.global/tutorials/verify/using-callback/callback-intro

Note: because you just have the 1 sandbox tenant, you will be issuing credentials and verifying them through the same instance, but Issuer and Verifier could easily be separate tenants on our platform or indeed any other interoperable platform.


## Verifing a credential

https://learn.mattr.global/tutorials/verify/using-callback/callback-local

```
ngrok http http://localhost:5000
```


https://learn.mattr.global/tutorials/verify/using-callback/callback-e-to-e
