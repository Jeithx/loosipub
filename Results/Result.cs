namespace Core.Utilities.Results
{
    public class Result : IResult
    {
        public Result(bool success, string message, int? recordTotals) : this(success)
        {
            Message = message;
            RecordTotals = recordTotals;
        }

        public Result(bool success)
        {
            Success = success;
        }

        public bool Success { get; }

        public string Message { get; set; }
        public int? RecordTotals { get; set; }
    }
}