namespace WebStore.Domain.Entities.Base.Interfaces
{
    /// <summary> Базовая сущность для данных </summary>
    public interface IEntity
    {
        /// <summary> Идентификатор </summary>
        int Id { get; set; }
    }
}
