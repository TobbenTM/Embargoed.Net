## Embargoed.Net

Embargoed.Net is [ASP.NET](https://dotnet.microsoft.com/en-us/apps/aspnet) middleware with a that blocks all requests from Russia and displays a pro-Ukraine message instead.

This is a port of [Embargoed](https://github.com/rameerez/embargoed).
The author also maintains a list of ports on [Embargoed-list](https://github.com/rameerez/embargoed-list).

Here's the message that replaces all pages of your app:

![Embargoed message displayed to Russian visitors](https://github.com/rameerez/embargoed/blob/main/public/embargoed-message.jpg?raw=true)

##  Installation

Use Rider/Visual Studio or run the following command:

`dotnet add package Embargoed.AspNetCore`

More details found here: [https://www.nuget.org/packages/Embargoed.AspNetCore/](https://www.nuget.org/packages/Embargoed.AspNetCore/)

In your `Program.cs` file (using .Net 6.0 minimal API):

```csharp
...
app.UseRussiaEmbargo();
...

```

That's it.  Traffic from Russia is now blocked.
