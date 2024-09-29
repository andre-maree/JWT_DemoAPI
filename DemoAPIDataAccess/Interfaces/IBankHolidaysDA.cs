using Models;

namespace DemoAPIDataAccess
{
    public interface IBankHolidaysDA
    {
        Task<IEnumerable<BankHolidaysFromDbAll>> GetAllBankHolidays();
        Task<IEnumerable<BankHolidaysFromDb>> GetBankHolidays(int regionId);
    }
}