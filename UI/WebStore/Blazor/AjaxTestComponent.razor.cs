using System;
using System.Threading.Tasks;
using WebStore.Domain.WebModels.Ajax;

namespace WebStore.Blazor
{
    public partial class AjaxTestComponent
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime DateTime { get; set; }
        public string StyleSpinner { get; set; } = "display: none";
        public string StyleView { get; set; } = "display: none";

        public async Task OnLoadTestAjaxData()
        {
            StyleSpinner = "";
            var model = await GetDataFromServer(12, "HelloWorldFromBlazor", 1000);
            Id = model.Id;
            Message = model.Message;
            DateTime = model.ServerTime;
            StyleSpinner = "display: none";
            StyleView = "";
        }

        private async Task<AjaxDataWebModel> GetDataFromServer(int? id, string msg, int Delay = 1000)
        {
            await Task.Delay(Delay);
            return new AjaxDataWebModel
            {
                Id = id ?? 1,
                Message = msg,
                ServerTime = DateTime.Now,
            };
        }
    }
}
