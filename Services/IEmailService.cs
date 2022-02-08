using E_Commerce.Models;
using System.Threading.Tasks;

namespace E_Commerce.Services
{
    public interface IEmailService
    {
        string GetEmailBody(string templateName);
        Task sendTestEmail(UserEmailOptions options);
    }
}