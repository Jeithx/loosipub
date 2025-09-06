namespace Core.Utilities.Results
{
    public class SuccessDataResult<T> : DataResult<T>
    {
        public SuccessDataResult(T data) : base(data, true)
        {
        }

        public SuccessDataResult(T data, string message, int? recordTotals = 0) : base(data, true, message, recordTotals)
        {
        }

        public SuccessDataResult(string message) : base(default, true, message, 0)
        {
        }

        public SuccessDataResult() : base(default, true)
        {
        }
    }
}
