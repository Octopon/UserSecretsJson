using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Reflection;
using UserSecretsJson.Helpers;

namespace UserSecretsJson;

public static class MauiProgram
{
    //1 option
    //Init secrets.json +
    //Customize Build +
    //Create UserSecretHelper

    //2 option
    private const string Namespace = "UserSecretsJson";
    private const string FileName = "secrets2.json";
    public static MauiApp CreateMauiApp()
	{



		using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{Namespace}.{FileName}");
		var config = new ConfigurationBuilder().AddJsonStream(stream).Build();

		var builder = MauiApp.CreateBuilder();

		builder.Configuration.AddConfiguration(config);

		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		//get value by key
		var secret = UserSecretHelper.Instance["Settings:Password"];

		var secret2 = config.GetSection("Settings2").Value;
#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
