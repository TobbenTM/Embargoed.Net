using MaxMind.GeoIP2;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Embargoed.AspNetCore
{
    internal class Middleware
    {
        private readonly RequestDelegate _next;
        private readonly string[] _embargoedCountries;

        public Middleware(RequestDelegate next)
        {
            _next = next;
            _embargoedCountries = new[] { "RU", "BLR" };
        }

        public async Task Invoke(HttpContext context)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var geoIpResourcePath = assembly
                .GetManifestResourceNames()
                .Single(res => res.EndsWith("Geoacumen-Country.mmdb"));

            using var stream = assembly.GetManifestResourceStream(geoIpResourcePath);
            using var reader = new DatabaseReader(stream);

            var originCountry = reader.Country(context.Connection.RemoteIpAddress)?.Country;

            if (_embargoedCountries.Contains(originCountry?.IsoCode))
            {
                context.Response.StatusCode = 200;
                context.Response.ContentType = "text/html";

                var embargoPageResourcePath = assembly
                    .GetManifestResourceNames()
                    .Single(res => res.EndsWith("embargo.html"));
                using var pageStream = assembly.GetManifestResourceStream(embargoPageResourcePath);
                context.Response.ContentLength = pageStream.Length;
                await pageStream.CopyToAsync(context.Response.Body);

                return;
            }

            await _next(context);
        }
    }
}
