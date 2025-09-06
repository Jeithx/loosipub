using Core.Entities.Concrete;
using Core.Models;
namespace Core.Models.Write
{
    public class UserCallPriceWD
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public decimal RatePerMinuute { get; set; }

        public int Currency { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }

    }
}
