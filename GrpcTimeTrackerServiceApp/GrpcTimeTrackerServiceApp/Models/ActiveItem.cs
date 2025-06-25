namespace GrpcTimeTrackerServiceApp.Models
{
    public class ActiveItem
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public TimeSpan TimeSpent { get; set; }
    }
}