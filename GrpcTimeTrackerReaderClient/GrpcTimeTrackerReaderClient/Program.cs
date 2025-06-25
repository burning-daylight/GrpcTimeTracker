using Grpc.Core;
using Grpc.Net.Client;
using GrpcTimeTrackerServiceApp;
using GrpcTimeTrackerReaderClient;

bool isRunning = true;
StatisticsTracker tracker = new StatisticsTracker();

while (isRunning)
{
    Console.WriteLine("\n\nPress any key to get statistics");
    Console.ReadLine();
    var items = tracker.GetStatistics().Result.OrderByDescending(x => x.Percent);
    foreach( var item in items)
        Console.WriteLine($"{Math.Round(item.Percent, 1)}%\t{item.TimeSpent}\t{item.AppName}");
}