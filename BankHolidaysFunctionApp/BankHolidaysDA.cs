using Dapper;
using Models;
using System;
using System.Data.SqlClient;
using System.Data;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;

namespace BankHolidaysFunctionApp
{
    public class BankHolidaysDA
    {
        public static async Task<bool> SaveBankHolidays()
        {
            HttpClient client = new()
            {
                BaseAddress = new Uri(Environment.GetEnvironmentVariable("HolidaysServiceURL"))
            };

            BankHolidays? root = await client.GetFromJsonAsync<BankHolidays?>("");

            DataTable dtHolidays = new("Holidays");
            dtHolidays.Columns.Add("Id", typeof(int));
            dtHolidays.Columns.Add("Name", typeof(string));
            dtHolidays.Columns.Add("Date", typeof(DateTime));

            DataTable dtRegionHolidays = new("RegionHolidays");
            dtRegionHolidays.Columns.Add("RegionId", typeof(int));
            dtRegionHolidays.Columns.Add("HolidayId", typeof(int));

            int hid = 1;

            foreach (Event item in root.englandandwales.events)
            {
                hid = ProcessData(dtHolidays, dtRegionHolidays, hid, item, (int)BankHolidayRegions.englandandwales);
            }

            foreach (Event item in root.scotland.events)
            {
                hid = ProcessData(dtHolidays, dtRegionHolidays, hid, item, (int)BankHolidayRegions.scotland);
            }

            foreach (Event item in root.northernireland.events)
            {
                hid = ProcessData(dtHolidays, dtRegionHolidays, hid, item, (int)BankHolidayRegions.northernireland);
            }

            DataTable dtRegions = new("RegionType");
            dtRegions.Columns.Add("Id", typeof(int));
            dtRegions.Columns.Add("Region", typeof(string));
            DataRow row = dtRegions.NewRow();
            row["Id"] = BankHolidayRegions.englandandwales; 
            row["Region"] = Enum.GetName(typeof(BankHolidayRegions), BankHolidayRegions.englandandwales);

            dtRegions.Rows.Add(row);
            row = dtRegions.NewRow();
            row["Id"] = BankHolidayRegions.scotland;
            row["Region"] = Enum.GetName(typeof(BankHolidayRegions), BankHolidayRegions.scotland);

            dtRegions.Rows.Add(row);
            row = dtRegions.NewRow();
            row["Id"] = BankHolidayRegions.northernireland;
            row["Region"] = Enum.GetName(typeof(BankHolidayRegions), BankHolidayRegions.northernireland);

            dtRegions.Rows.Add(row);

            using (SqlConnection connection = new(Environment.GetEnvironmentVariable("dbCon")))
            {
                // Define parameters including your output parameters
                DynamicParameters parameters = new();
                parameters.Add("@RegionType", dtRegions, DbType.Object);
                parameters.Add("@HolidayType", dtHolidays, DbType.Object);
                parameters.Add("@RegionHolidayType", dtRegionHolidays, DbType.Object);

                await connection.OpenAsync();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    // Execute the stored procedure
                    int result = await connection.ExecuteAsync("[dbo].[SP_Insert_RegionHolidays]", parameters, transaction, null, CommandType.StoredProcedure);

                    transaction.Commit();
                }
            }

            return true;
        }

        private static int ProcessData(DataTable dtHolidays, DataTable dtRegionHolidays, int hid, Event item, int regionId)
        {
            DataRow[] rows = dtHolidays.Select($"Name = '{item.title}' and Date = '{item.date}'");

            DataRow rhrow = dtRegionHolidays.NewRow();

            if (rows.Length > 0)
            {
                rhrow["HolidayId"] = Convert.ToInt32(rows[0][0]);
            }
            else
            {
                DataRow hrow = dtHolidays.NewRow();
                hrow["Id"] = hid;
                hrow["Name"] = item.title;
                hrow["Date"] = item.date;
                dtHolidays.Rows.Add(hrow);

                rhrow["HolidayId"] = hid;
                hid++;
            }

            rhrow["RegionId"] = regionId;

            dtRegionHolidays.Rows.Add(rhrow);

            return hid;
        }
    }
}
