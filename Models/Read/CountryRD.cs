namespace Core.Models.Read
{
    public class CountryRD
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsActive { get; set; }
        public int LanguageId { get; set; }
    }
}
