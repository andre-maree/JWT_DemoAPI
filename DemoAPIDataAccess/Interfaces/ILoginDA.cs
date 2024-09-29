
namespace DemoAPIDataAccess
{
    public interface ILoginDA
    {
        Task<bool> CreateLogin(string username, string password);
        Task<string> GetLogin(string username, string password);
    }
}