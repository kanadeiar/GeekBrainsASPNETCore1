using System.Collections.Generic;

namespace WebStore.WebModels
{
    public class SectionWebModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public SectionWebModel Parent { get; set; }
        public List<SectionWebModel> Children { get; set; } = new();
        public int CountProduct { get; set; }
    }
}
