using System.ComponentModel.DataAnnotations;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.Entities.Base
{
    /// <summary> Сортированная сущность </summary>
    public abstract class NamedEntity : Entity, INamedEntity
    {
        /// <summary> Название </summary>
        [Required, MaxLength(100)]
        public string Name { get; set; }
    }
}
