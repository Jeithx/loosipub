namespace Core.Utilities.Results
{
    public class DataResult<T> : Result, IDataResult<T>
    {
        public DataResult(T data, bool success, string message, int? recordTotals = 1) : base(success, message, recordTotals)
        {
            Data = data;
            RecordTotals = recordTotals;
        }

        public DataResult(T data, bool success) : base(success)
        {
            Data = data;
        }

        public T Data { get; }
    }
}
