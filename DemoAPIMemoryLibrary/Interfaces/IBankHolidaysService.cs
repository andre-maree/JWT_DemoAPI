using Models;

namespace MemLib
{
    public interface IBankHolidaysService
    {
        Task<IEnumerable<BankHolidaysFromDbAll>> GetBankHolidays();
        Task<IEnumerable<BankHolidaysFromDb>> GetBankHolidays(int regionId);
        Task<(bool, string)> StartBankHolidaysSaveProcess();
    }
}