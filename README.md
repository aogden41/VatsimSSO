# VATSIM SSO
##### An OAuth 1.0 wrapper designed to easily implement the VATSIM SSO system in C# web applications.
Installation

    PM> Install-Package VatsimSSO
    
Dependencies will be pulled at the same time.
### Implementation

#### Initial login token
The code below will return the login token used to authenticate the user when redirecting to the SSO server (BaseUrl property defaults to https://cert.vatsim.net/sso/api).
```csharp
TokenResponse tokenResponse = VatsimSingleSignOn.SSO.GetRequestToken("CONSUMER_KEY", "CONSUMER_SECRET", "CALLBACK_URI");
```

The object returned is a custom `TokenResponse` object which contains the token and result information. This object inherits from the `IResponse` interface.
A redirect example in ASP.NET would look like this:
```csharp
return Redirect("https://cert.vatsim.net/sso/auth/pre_login/?oauth_token=" + tokenResponse.Token.OAuthToken);
```

You can return the raw JSON string by returning the `Raw` property of the object.

#### Returned user data
The below code will return a JSON string of the information for the user that has just logged in, using the access token, secret and verifier.
```csharp
UserResponse userResponse = VatsimSingleSignOn.SSO.GetUserData("CONSUMER_KEY", "CONSUMER_SECRET", "OAUTH_VERIFIER", tokenResponse.Token.OAuthToken, tokenResponse.Token.TokenSecret);
```

You can view the raw JSON response data from any object inheriting from `IResponse` via the `Raw` property.
Example:
```csharp
Console.WriteLine(userResponse.Raw)
```
Would result in:
```json
{"request":{"result":"success","message":""},"user":{"id":"1300007","name_first":"7th","name_last":"Test","rating":{"id":"7","short":"C3","long":"Senior Controller","GRP":"Senior Controller"},"pilot_rating":{"rating":"0"},"email":"noreply@vatsim.net","experience":"N","reg_date":"2014-05-14 17:17:26","country":{"code":"GB","name":"United Kingdom"},"region":{"code":"USA-S","name":"South America"},"division":{"code":"CAM","name":"Central America"},"subdivision":{"code":null,"name":null}}}
```

Please open up an issue if there are any problems or feel free to post on the forum thread.
