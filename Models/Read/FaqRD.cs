namespace Core.Models.Read
{
    public class FaqRD
    {
        public long Id { get; set; }
        public int LanguageId { get; set; }
        public string? Language { get; set; }
        public int TypeId { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsActive { get; set; }
    }
}
