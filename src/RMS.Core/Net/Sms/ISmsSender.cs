using System.Threading.Tasks;

namespace RMS.Net.Sms
{
    public interface ISmsSender
    {
        Task SendAsync(string number, string message);
    }
}