using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Grpc.Net.Client;
using GrpcTimeTrackerServiceApp;
using Google.Protobuf.WellKnownTypes;

namespace GrpcTimeTrackerWriterClientApp
{
    public class ActiveWindowTracker
    {
        public bool IsRunning { private set; get; } = false;
        TimeTracker.TimeTrackerClient client;
        int checkDelayMs = 2000;

        public ActiveWindowTracker()
        {
            var channel = GrpcChannel.ForAddress("https://localhost:7246");
            client = new TimeTracker.TimeTrackerClient(channel);
        }

        public async Task Run()
        {
            IsRunning = true;
            await Task.Run(async () =>
            {
                while (IsRunning)
                {
                    await client.InsertAsync(new InsertRequest
                    {
                        Title = getCurrentAppName(),
                        Timespent = Duration.FromTimeSpan(TimeSpan.FromSeconds(checkDelayMs / 1000)),
                    });

                    await Task.Delay(checkDelayMs);
                }
            });
        }

        public void Stop()
        {
            IsRunning = false;
        }

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll")]
        static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out IntPtr lpdwProcessId);

        string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
        }

        string getCurrentAppName()
        {
            IntPtr activeAppHandle = GetForegroundWindow();

            IntPtr activeAppProcessId;
            GetWindowThreadProcessId(activeAppHandle, out activeAppProcessId);

            Process currentAppProcess = Process.GetProcessById((int)activeAppProcessId);
            string currentAppName = FileVersionInfo.GetVersionInfo(currentAppProcess.MainModule.FileName).FileDescription;

            return currentAppName;
        }
    }
}
