# National Driving License 

creates drving license credentials

## Database migrations create

### Console

dotnet ef migrations add init_ndl 

### Powershell

Add-Migration "init_ndl" 

## Database migrations update

### Console

dotnet ef database update

### Powershell

Update-Database


###

https://dev-damienbod.eu.auth0.com/.well-known/openid-configuration

https://learn.mattr.global/tutorials/issue/oidc-bridge/setup-fed-provider


```json
"license_issued_at": "2021-03-02",
"license_type": "B1",
"name": "Bob",
"first_name": "Lammy",
"date_of_birth": "1953-07-21"

```



```javascript
function (user, context, callback) {
    const namespace = 'https://ndl.com/';
    context.idToken[namespace + 'license_issued_at'] = user.user_metadata.license_issued_at;
    context.idToken[namespace + 'license_type'] = user.user_metadata.license_type;
    context.idToken[namespace + 'name'] = user.user_metadata.name;
    context.idToken[namespace + 'first_name'] = user.user_metadata.first_name;
    context.idToken[namespace + 'date_of_birth'] = user.user_metadata.date_of_birth;
    callback(null, user, context);
}

```