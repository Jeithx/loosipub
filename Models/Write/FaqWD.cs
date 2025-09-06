namespace Core.Models.Write
{
    public class FaqWD
    {
        public long Id { get; set; }
        public int LanguageId { get; set; }
        public int TypeId { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsActive { get; set; }
    }
}
