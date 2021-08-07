using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace DataCollection.Middleware
{
    public class LocalizationPipeline
    {
        public void Configure(IApplicationBuilder applicationBuilder)
        {
            CultureInfo Tr = new CultureInfo("tr-TR");
            Tr.NumberFormat.NumberDecimalSeparator = ",";

            var supportedCultures = new[]
            {
                Tr
            };

            var options = new RequestLocalizationOptions
            {

                DefaultRequestCulture = new RequestCulture(culture: "tr-TR", uiCulture: "tr-TR"),
                SupportedCultures = supportedCultures,                
                SupportedUICultures = supportedCultures,                                
            };
            
            options.RequestCultureProviders = new[]
                { new RouteDataRequestCultureProvider() { Options = options } };

            applicationBuilder.UseRequestLocalization(options);
        }
    }

}
