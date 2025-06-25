using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrpcTimeTrackerReaderClient.Models
{
    public class StatItem
    {
        public string AppName { get; set; }
        public TimeSpan TimeSpent { get; set; }
        public double Percent {  get; set; }
    }
}