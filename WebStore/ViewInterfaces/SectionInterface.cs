using System.Collections.Generic;

namespace WebStore.ViewInterfaces
{
    public class SectionInterface
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public SectionInterface Parent { get; set; }
        public List<SectionInterface> Children { get; set; } = new();
        public int CountProduct { get; set; }
    }
}
