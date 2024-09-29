using Dapper;
using Microsoft.Extensions.Configuration;
using Models;

namespace DemoAPIDataAccess
{
    public class BankHolidaysDA : BaseDA, IBankHolidaysDA
    {
        public string cacheKey = "holidayscache";
        private IConfiguration _config;

        public BankHolidaysDA(IConfiguration config) : base(config) 
        {
            _config = config;
        }

        public async Task<IEnumerable<BankHolidaysFromDb>> GetBankHolidays(int regionId)
        {
            // Define parameters including your output parameters
            DynamicParameters parameters = new();
            parameters.Add("@RegionId", regionId);

            IEnumerable<BankHolidaysFromDb> result = await ExecuteStoredProcedureQueryAsync<BankHolidaysFromDb>("[dbo].[SP_Get_RegionHolidaysByRegionId]", parameters);

            return result;
        }

        public async Task<IEnumerable<BankHolidaysFromDbAll>> GetAllBankHolidays()
        {
            IEnumerable<BankHolidaysFromDbWithRegionId> res = await ExecuteStoredProcedureQueryAsync<BankHolidaysFromDbWithRegionId>("[dbo].[SP_Get_RegionHolidays]");

            List<BankHolidaysFromDbAll> result = new();
            int regionId = -1;
            BankHolidaysFromDbAll bankHolidaysFromDbAll = null;
            string region1 = "england-and-wales";
            string region2 = "scotland";
            string region3 = "northern-ireland";

            foreach (BankHolidaysFromDbWithRegionId item in res)
            {
                if (regionId != item.RegionId)
                {
                    bankHolidaysFromDbAll = new BankHolidaysFromDbAll(); 
                    result.Add(bankHolidaysFromDbAll);

                    if (item.RegionId == (int)BankHolidayRegions.englandandwales)
                    {
                        bankHolidaysFromDbAll.Region = region1;
                    }
                    else if (item.RegionId == (int)BankHolidayRegions.scotland)
                    {
                        bankHolidaysFromDbAll.Region = region2;
                    }
                    else if (item.RegionId == (int)BankHolidayRegions.northernireland)
                    {
                        bankHolidaysFromDbAll.Region = region3;
                    }
                    else
                    {
                        throw new Exception("Region not found");
                    }

                    regionId = item.RegionId;
                }

                bankHolidaysFromDbAll.BankHolidays.Add(item);
            }

            return result;
        }
    }
}
