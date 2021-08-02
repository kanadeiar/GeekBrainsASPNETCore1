using System;

namespace WebStore.Domain.WebModels.Ajax
{
    public class AjaxDataWebModel
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime ServerTime { get; set; }
    }
}
