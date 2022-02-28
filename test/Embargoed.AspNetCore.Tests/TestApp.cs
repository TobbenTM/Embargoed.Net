using Embargoed.AspNetCore;
using Microsoft.AspNetCore.Builder;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.Use(async (context, next) =>
{
    context.Connection.RemoteIpAddress = TestApp.RemoteIpAddress;
    await next.Invoke();
});

app.UseRussiaEmbargo();

app.MapGet("/", () => "This page is not embargoed");

app.Run();

internal partial class TestApp
{
    public static IPAddress? RemoteIpAddress { get; set; }
}
