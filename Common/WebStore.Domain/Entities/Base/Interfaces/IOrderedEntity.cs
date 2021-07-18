namespace WebStore.Domain.Entities.Base.Interfaces
{
    /// <summary> Сортированная сущность </summary>
    public interface IOrderedEntity : IEntity
    {
        /// <summary> Сортировка </summary>
        int Order { get; set; }
    }
}
