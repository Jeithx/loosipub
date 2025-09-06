namespace Core.Models.Write
{
    public class CountryWD
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsActive { get; set; }
        public int LanguageId { get; set; }
    }
}
