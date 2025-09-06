namespace Core.Models.Read
{
    public class CityRD
    {
        public long Id { get; set; }
        public long CountryId { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsActive { get; set; }
    }
}
