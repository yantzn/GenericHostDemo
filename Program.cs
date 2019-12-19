using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AzureBatchDemoApp
{
	class Program
	{
		static async Task Main(string[] args)
		{
			var host = new HostBuilder()
				.ConfigureHostConfiguration(configHost =>
				{
					// 環境設定ファイルの追加
					configHost.SetBasePath(Directory.GetCurrentDirectory());
					configHost.AddJsonFile("hostsettings.json", optional: true);
					configHost.AddEnvironmentVariables();
					configHost.AddCommandLine(args);
				})
				.ConfigureAppConfiguration((hostContext, configApp) =>
				{
					// アプリケーション設定ファイルの追加
					configApp.SetBasePath(Directory.GetCurrentDirectory());
					configApp.AddJsonFile("appsettings.json", optional: true);
					configApp.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json",optional: true);
					configApp.AddEnvironmentVariables();
					configApp.AddCommandLine(args);
				})
				.ConfigureServices((hostContext, services) =>
				{
					// サービスの追加
					services.Configure<AppConfigItem>(hostContext.Configuration.GetSection("AppConfig"));
					services.AddHostedService<BatchService>();
				})
				.ConfigureLogging((hostContext, configLogging) =>
				{
					// Application Insight や ロガーの追加
					configLogging.AddConsole();
					configLogging.AddDebug();
				})
				.UseConsoleLifetime()
				.Build();

			await host.RunAsync();
		}
	}
}
