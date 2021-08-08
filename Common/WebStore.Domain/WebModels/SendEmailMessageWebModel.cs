using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace WebStore.Domain.WebModels
{
    /// <summary> Веб модель отправки сообщения по электронной почте </summary>
    public class SendEmailMessageWebModel
    {
        /// <summary> Имя отправителя </summary>
        [Display(Name = "Имя отправителя")]
        [Required(ErrorMessage = "Не указано имя отправителя")]
        public string NameFrom { get; set; }
        /// <summary> Адрес отправителя </summary>
        [Display(Name = "Адрес отправителя")]
        [Required(ErrorMessage = "Не указан адрес отправителя")]
        public string MailFrom { get; set; }
        /// <summary> Адрес почты отправителя </summary>
        [Display(Name = "Адрес почтового сервера")]
        [Required(ErrorMessage = "Не указан адрес сервера")]
        public string Address { get; set; }
        /// <summary> Порт почтового сервера </summary>
        [Display(Name = "Порт почтового сервера")]
        [Required(ErrorMessage = "Не указан порт сервера")]
        public int Port { get; set; }
        /// <summary> Логин пользователя </summary>
        [Display(Name = "Логин пользователя почтового сервера")]
        [Required(ErrorMessage = "Не указан логин пользователя для входа на почтовый сервер")]
        public string Login { get; set; }
        /// <summary> Пароль пользователя </summary>
        [Display(Name = "Пароль пользователя почтового сервера")]
        [Required(ErrorMessage = "Не указан пароль пользователя для входа на почтовый сервер")]
        public string Password { get; set; }
        /// <summary> Имя получателя </summary>
        [Display(Name = "Имя получателя")]
        [Required(ErrorMessage = "Не указано имя получателя сообщения")]
        public string NameTo { get; set; }
        /// <summary> Адрес получателя </summary>
        [Display(Name = "Адрес получателя")]
        [Required(ErrorMessage = "Не указан адрес получателя сообщения")]
        public string MailTo { get; set; }
        /// <summary> Заголовок сообщения </summary>
        [Display(Name = "Заголовок сообщения")]
        [Required(ErrorMessage = "Не указан заголовок сообщения")]
        public string Subject { get; set; }
        /// <summary> Тело сообщения </summary>
        [Display(Name = "Тело сообщения")]
        [Required(ErrorMessage = "Не указано тело сообщения")]
        public string Body { get; set; }
    }
}
