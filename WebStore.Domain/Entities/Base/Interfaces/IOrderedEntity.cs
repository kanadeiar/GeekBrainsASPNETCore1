namespace WebStore.Domain.Entities.Base.Interfaces
{
    public interface IOrderedEntity : IEntity
    {
        public int Order { get; set; }
    }
}
