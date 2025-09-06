using Core.Models;
namespace Core.Models.Read
{
    public class WalletLogRD
    {
        public long Id { get; set; }
        public long WalletId { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsActive { get; set; }
    }
}
