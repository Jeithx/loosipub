using Core.Models;
namespace Core.Models.Write
{
    public class TagWD
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsActive { get; set; }
    }
}
