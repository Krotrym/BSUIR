using Laboratornay6.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Laboratornay6;

namespace MauiApp1
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            
            builder.Services.AddHttpClient<IRateService, RateService>(opt =>
            {
                opt.BaseAddress = new Uri("https://api.nbrb.by/exrates/rates");
            });


            builder.Services.AddHttpClient<RateService>();
            builder.Services.AddSingleton<RateService>();

            builder.Services.AddTransient<ConvertPage>();

            

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
