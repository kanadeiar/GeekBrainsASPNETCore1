using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.Domain.Entities.Base.Interfaces
{
    public interface IOrderedEntity : IEntity
    {
        public int Order { get; set; }
    }
}
