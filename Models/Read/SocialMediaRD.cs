using Core.Models;
namespace Core.Models.Read
{
    public class SocialMediaRD
    {
        public long Id { get; set; }
        public string Logo { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsActive { get; set; }
        public int LanguageId { get; set; }
    }
}
