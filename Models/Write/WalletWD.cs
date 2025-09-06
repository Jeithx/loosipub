using Core.Models;
namespace Core.Models.Write
{
    public class WalletWD
    {
        public decimal Balance { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public long Id { get; set; }
        public long UserId { get; set; }
    }
}
