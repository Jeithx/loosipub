namespace Core.Entities.Concrete
{
    public partial class OperationClaim : IEntity
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
