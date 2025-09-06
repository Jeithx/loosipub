namespace Core.Utilities.Results
{
    public interface IResult
    {
        bool Success { get; }
        string Message { get; set; }
        int? RecordTotals { get; set; }
    }
}
