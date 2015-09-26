using System.Threading.Tasks;

namespace WebApiAkkaDemo.AppService
{
    public interface IAskableService
    {
        string Ask(string question);
        Task<string> AskAsync(string question);
    }
}
