using DemoAPIDataAccess;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Models;

namespace MemLib
{

    public class BankHolidaysService : IBankHolidaysService
    {
        private readonly IMemoryCache _memoryCache;
        private IBankHolidaysDA _BankHolidaysDA;
        private readonly HttpClient _httpClient;

        public BankHolidaysService(IMemoryCache memoryCache, IConfiguration config, IBankHolidaysDA bankHolidaysDA, HttpClient httpClient)
        {
            _memoryCache = memoryCache;
            _BankHolidaysDA = bankHolidaysDA;
            _httpClient = httpClient;   
        }

        public async Task<(bool, string)> StartBankHolidaysSaveProcess()
        {
            HttpResponseMessage resp = await _httpClient.GetAsync("");

            if (resp.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                return (true, "The process started successfully.");
            }

            if (resp.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                string text = await resp.Content.ReadAsStringAsync();

                return (false, text);
            }

            return (false, "Internal server error.");
        }

        public async Task<IEnumerable<BankHolidaysFromDb>> GetBankHolidays(int regionId)
        {
            IEnumerable<BankHolidaysFromDb> bankHolidaysFromDb;
            string cachekey = "holidaysByRegionId" + regionId;

            if (!_memoryCache.TryGetValue(cachekey, out bankHolidaysFromDb))
            {
                IEnumerable<BankHolidaysFromDb> res = await _BankHolidaysDA.GetBankHolidays(regionId);

                _memoryCache.Set(cachekey, res,
                new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromHours(1)));

                return res;
            }

            return bankHolidaysFromDb;
        }

        public async Task<IEnumerable<BankHolidaysFromDbAll>> GetBankHolidays()
        {
            IEnumerable<BankHolidaysFromDbAll> bankHolidaysFromDb;
            string cachekey = "holidays";

            if (!_memoryCache.TryGetValue(cachekey, out bankHolidaysFromDb))
            {
                IEnumerable<BankHolidaysFromDbAll> res = await _BankHolidaysDA.GetAllBankHolidays();

                _memoryCache.Set(cachekey, res,
                    new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromHours(1)));

                return res;
            }

            return bankHolidaysFromDb;
        }
    }
}
