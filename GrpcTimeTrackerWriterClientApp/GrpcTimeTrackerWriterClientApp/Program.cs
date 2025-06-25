using Grpc.Core;
using Grpc.Net.Client;
using GrpcTimeTrackerServiceApp;
using GrpcTimeTrackerWriterClientApp;

bool isRunning = true;
ActiveWindowTracker tracker = new ActiveWindowTracker();

Console.WriteLine("\n-------------------------------------------------------\n" +
        "Type 'run' to run writer service\n" +
        "Type 'stop' to stop writer service\n" +
        "Type 'exit' to exit the app");

while (isRunning)
{
    Console.WriteLine("Input command:");
    var read = Console.ReadLine();
    switch (read)
    {
        case "run":
            if (!tracker.IsRunning)
            {
                tracker.Run();
                Console.WriteLine("Writer service is RUNNING.\n");
            }
            break;
        case "stop":
            tracker.Stop();
            Console.WriteLine("Writer service is STOPPED.\n\n");
            break;
        case "exit":
            tracker.Stop();
            isRunning = false;
            break;
    }
}

Console.WriteLine("\nWrite to DB is stoped. Press any key to exit...\n\n");
Console.ReadLine();