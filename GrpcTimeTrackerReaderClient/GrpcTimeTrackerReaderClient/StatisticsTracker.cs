using Grpc.Net.Client;
using GrpcTimeTrackerReaderClient.Models;
using GrpcTimeTrackerServiceApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrpcTimeTrackerReaderClient
{
    public class StatisticsTracker
    {
        TimeTracker.TimeTrackerClient client;

        public StatisticsTracker()
        {
            var channel = GrpcChannel.ForAddress("https://localhost:7246");
            client = new TimeTracker.TimeTrackerClient(channel);
        }

        public async Task<IEnumerable<StatItem>> GetStatistics()
        {
            var items = await client.GetAllAsync(new GetAllRequest());
            List<StatItem> statItems = new List<StatItem>();
            foreach (var item in items.Items)
                statItems.Add(new StatItem
                {
                    AppName = item.Title,
                    Percent = (int)item.Timespent.ToTimeSpan().TotalSeconds,
                    TimeSpent = item.Timespent.ToTimeSpan()
                });
            double sum = statItems.Sum(x => x.Percent);
            statItems.ForEach(x => x.Percent = x.Percent / sum * 100);

            return statItems;
        }
    }
}