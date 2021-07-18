using System.ComponentModel.DataAnnotations;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.Entities.Base
{
    /// <summary> Базовая сущность для данных </summary>
    public abstract class Entity : IEntity
    {
        /// <summary> Идентификатор </summary>
        [Key]
        public int Id { get; set; }
        /// <summary> Датовременной штамп </summary>
        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}
