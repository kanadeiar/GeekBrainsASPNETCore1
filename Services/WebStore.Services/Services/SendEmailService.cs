using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using WebStore.Interfaces.Services;

namespace WebStore.Services.Services
{
    public class SendEmailService : ISendEmailService
    {
        private readonly ILogger<SendEmailService> _logger;
        private MailAddress _from;
        private SmtpClient _smtp;
        public SendEmailService(string nameFrom, string mailFrom, string address, int port, NetworkCredential credentials, ILogger<SendEmailService> logger)
        {
            _logger = logger;
            _from = new MailAddress(mailFrom, nameFrom);
            _smtp = new SmtpClient(address, port);
            _smtp.Credentials = credentials;
            _smtp.EnableSsl = true;
        }

        public async Task SendEmailAsync(string nameTo, string mailTo, string subject, string body)
        {
            var to = new MailAddress(mailTo, nameTo);
            var message = new MailMessage(_from, to);
            message.Subject = subject;
            message.Body = body;
            try
            {
                await _smtp.SendMailAsync(message);
                _logger.LogInformation("Письмо успешно отправлено на адрес {0}", mailTo);
            }
            catch (Exception e)
            {
                _logger.LogError("Не удалось отправить письмо на адрес {0}, ошибка: {1}", mailTo, e.Message);
                throw;
            }
            
        }
    }
}
