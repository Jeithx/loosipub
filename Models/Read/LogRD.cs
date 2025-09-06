namespace Core.Models.Read
{
    public class LogRD
    {
        public long Id { get; set; }
        public string Action { get; set; }
        public string Service { get; set; }
        public DateTime LogDate { get; set; }
    }
}
