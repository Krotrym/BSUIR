using Microsoft.Maui.Controls.PlatformConfiguration.TizenSpecific;
using System.Diagnostics;

namespace Laboratornay4
{
    public partial class MainPage : ContentPage
    {
        SemaphoreSlim semaphore = new SemaphoreSlim(1);
        CancellationTokenSource cts;
        private IProgress<(double ,string)> progress;
        public MainPage()
        {
            InitializeComponent();
            progress = new Progress<(double percent, string message)>(data =>
            {
                progressBar.Progress = data.percent / 100.0;
                result.Text = data.message;
                display.Text = $"{data.percent}%";
            });
        }

        public async Task CalculateIntegral(CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
            semaphore.WaitAsync();
                double h = 0.00000001;
                double integral = 0.0;
                long n = (long)(1 / h);
                double progressStep = n / 100;

                for (long i = 0; i <= n; i++)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    integral += h * Math.Sin(h * (i + 0.5));
                    
                    for (int j = 0; j < 100; j++)
                    {
                        _ = 2 + 2;
                    }

                    if (i % progressStep == 0)
                    {
                        int percent = (int)((double)i / n * 100);
                        progress.Report((percent , "Вычислеине"));
                    }
                }
                progress.Report((100, integral.ToString()));
                semaphore.Release();
            });

        }


        private async void ButtonStartClick(object sender, EventArgs e)
        {
            if (!await semaphore.WaitAsync(0))
            { return; }
                try
                {
                    cts = new CancellationTokenSource();

                    await CalculateIntegral(cts.Token);
                }
                catch
                {
                    result.Text = "Задание отменено";
                    semaphore.Release();

                }
            finally
                {
                    semaphore.Release();
                }
        }
        private async void ButtonStopClick(object sender, EventArgs e)
        {
            cts.Cancel();      
        }
    }   
}
