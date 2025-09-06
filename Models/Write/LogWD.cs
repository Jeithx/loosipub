using Core.Enums;

namespace Core.Models.Write
{
    public class LogWD
    {
        public long Id { get; set; }
        public ELogType LogType { get; set; }
        public string RequestUrl { get; set; }
        public string HttpMethod { get; set; }
        public string RequestHeaders { get; set; }
        public string RequestBody { get; set; }
        public int StatusCode { get; set; }
        public string ExceptionMessage { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; } = DateTime.Now;
    }
}
