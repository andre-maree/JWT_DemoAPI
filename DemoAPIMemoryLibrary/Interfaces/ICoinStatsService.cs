using Models;

namespace MemLib
{
    public interface ICoinStatsService
    {
        Task<CoinsRoot?> GetCoinStats();
    }
}