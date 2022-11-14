using Microsoft.Extensions.Logging;

namespace Sample;

public static class MauiProgram
{
    public static IServiceProvider ServiceProvider { get; private set; }

    public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
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
        builder.Services.AddSingleton<MainPageViewModel>();
        builder.Services.AddSingleton<MainPage>();
        ServiceProvider = builder.Services.BuildServiceProvider();
        return builder.Build();
	}
}

