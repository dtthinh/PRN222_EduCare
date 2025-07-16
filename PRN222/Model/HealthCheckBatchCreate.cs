namespace PRN222.Model
{
    public class HealthCheckBatchCreate
    {
        public int NurseId { get; set; }
        public DateTime Date { get; set; }
        public string HealthCheckDescription { get; set; }
        public List<int> ClassIds { get; set; } = new();
    }
}
