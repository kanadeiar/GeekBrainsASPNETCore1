using System.Threading.Tasks;

namespace WebStore.Interfaces.Services
{
    public interface ISendEmailService
    {
        Task SendEmailAsync(string nameTo, string mailTo, string subject, string body);
    }
}