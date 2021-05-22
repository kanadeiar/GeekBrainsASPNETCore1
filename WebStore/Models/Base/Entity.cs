using System.ComponentModel.DataAnnotations;

namespace WebStore.Models.Base
{
    public abstract class Entity
    {
        public int Id { get; set; }
        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}
