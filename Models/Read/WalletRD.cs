using Core.Models;
namespace Core.Models.Read
{
    public class WalletRD
    {
        public decimal Balance { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public long Id { get; set; }
        public long UserId { get; set; }
    }
}
