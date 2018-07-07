# VATSIM SSO
##### An OAuth 1.0 wrapper designed to easily implement the VATSIM SSO system in C# web applications.
Installation

    PM> Install-Package VatsimSSO
    
Dependencies will be pulled at the same time.
### Implementation

#### Initial login token
The code below will return the login token used to authenticate the user when redirecting to the SSO server (BaseUrl property defaults to https://cert.vatsim.net/sso/api).
```csharp
// Client object
VatsimSSO r = new VatsimSSO("CONSUMER_KEY", "CONSUMER_SECRET", "CALLBACK_URI");

// Get the request token
var RequestToken = r.GetRequestToken();
```
The object returned is a custom `SSOAuthRequest` object which returns the token and result information.
A redirect example in ASP.NET would look like this:
```csharp
return Redirect("https://cert.vatsim.net/sso/auth/pre_login/?oauth_token=" + RequestToken.Token.oauth_token);
```
You can return the raw JSON string by returning the `Raw` property of the object.
#### Returned user data
The below code will return a JSON string of the information for the user that has just logged in, using the access token, secret and verifier.
```csharp
// Client object
VatsimSSO r = new VatsimSSO("CONSUMER_KEY", "CONSUMER_SECRET", Token: oauth_token, Verifier: oauth_verifier, TokenSecret: oauth_token_secret);

// Json string
string Results = r.ReturnData().Raw;
```
An example of such JSON response would be:
```json
{"request":{"result":"success","message":""},"user":{"id":"1300007","name_first":"7th","name_last":"Test","rating":{"id":"7","short":"C3","long":"Senior Controller","GRP":"Senior Controller"},"pilot_rating":{"rating":"0"},"email":"noreply@vatsim.net","experience":"N","reg_date":"2014-05-14 17:17:26","country":{"code":"GB","name":"United Kingdom"},"region":{"code":"USA-S","name":"South America"},"division":{"code":"CAM","name":"Central America"},"subdivision":{"code":null,"name":null}}}
```

You can also return an `SSOReturnData` object. This object contains already parsed user and result information as a C# object.
Example:
```csharp
// Return data object
var Results = r.ReturnData();
```
Please open up an issue if there are any problems or feel free to post on the forum thread.
