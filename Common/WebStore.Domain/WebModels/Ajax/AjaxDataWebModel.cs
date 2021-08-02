using System;

namespace WebStore.Domain.WebModels.Ajax
{
    /// <summary> Дата модель тестирования ajax </summary>
    public class AjaxDataWebModel
    {
        /// <summary> Идентификатор </summary>
        public int Id { get; set; }
        /// <summary> Сообщение </summary>
        public string Message { get; set; }
        /// <summary> Дата и время на сервере </summary>
        public DateTime ServerTime { get; set; }
    }
}
