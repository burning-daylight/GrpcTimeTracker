using Grpc.Core;
using Grpc.Net.Client;
using GrpcTimeTrackerServiceApp;
using GrpcTimeTrackerWriterClientApp;

bool isRunning = true;
ActiveWindowTracker tracker = new ActiveWindowTracker();

while (isRunning)
{
    Console.WriteLine("Type 'run' to run writer service\n" +
        "Type 'stop' to stop writer service\n" +
        "Type 'exit' to exit the app");
    var read = Console.ReadLine();
    switch (read)
    {
        case "run":
            if (!tracker.IsRunning)
            {
                tracker.Run();
                Console.WriteLine("Writer service is running\n");
            }
            break;
        case "stop":
            tracker.Stop();
            Console.WriteLine("Writer service is stopped\n\n");
            break;
        case "exit":
            tracker.Stop();
            isRunning = false;
            break;
    }
}

Console.WriteLine("\nWrite to DB is stoped. Press any key to exit...");
Console.ReadLine();