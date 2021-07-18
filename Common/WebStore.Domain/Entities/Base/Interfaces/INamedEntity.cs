namespace WebStore.Domain.Entities.Base.Interfaces
{
    /// <summary> Именованная сущность </summary>
    public interface INamedEntity : IEntity
    {
        /// <summary> Название </summary>
        string Name { get; set; }
    }
}
