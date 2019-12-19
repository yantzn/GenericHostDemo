using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AzureBatchDemoApp
{
	internal class BatchService : IHostedService
	{
		private readonly ILogger _logger;
		private readonly IHostApplicationLifetime _appLifetime;
		private readonly AppConfigItem _config;

		public BatchService(ILogger<BatchService> logger, IHostApplicationLifetime appLifetime, IOptions<AppConfigItem> config)
		{
			this._logger = logger;
			this._appLifetime = appLifetime;
			this._config = config.Value;
		}

		/// <summary>
		/// ホストの開始
		/// キャンセル トークンまたはシャットダウンがトリガーされると完了する 
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public Task StartAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("StartAsync has been called.");

			// ホストが完全に起動されたとき
			_appLifetime.ApplicationStarted.Register(OnStarted);
			
			// ホストが正常なシャットダウンを完了しているとき
			_appLifetime.ApplicationStopping.Register(OnStopping);
			
			// ホストが正常なシャットダウンを行っているとき
			_appLifetime.ApplicationStopped.Register(OnStopped);

			return Task.CompletedTask;
		}

		/// <summary>
		/// 指定されたタイムアウト内でホストの停止を試みる
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}

		private void OnStarted()
		{
			_logger.LogInformation("OnStarted has been called.");

			_logger.LogInformation("実行環境は {this._config.Environment} です", this._config.Environment);

			// アプリの終了
			this._appLifetime.StopApplication();
		}

		private void OnStopping()
		{
			_logger.LogInformation("OnStopping has been called.");

		}

		private void OnStopped()
		{
			_logger.LogInformation("OnStopped has been called.");

		}
	}
}
