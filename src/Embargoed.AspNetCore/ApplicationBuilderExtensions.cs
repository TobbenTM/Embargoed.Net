using Microsoft.AspNetCore.Builder;

namespace Embargoed.AspNetCore
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseRussiaEmbargo(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseMiddleware<Middleware>();
        }
    }
}
