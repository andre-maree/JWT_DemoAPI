using System.Text.Json.Serialization;

namespace Models
{
    public class BankHolidaysFromDbAll
    {
        public string Region { get; set; }
        public List<BankHolidaysFromDb> BankHolidays { get; set; } = new();
    }

    public class BankHolidaysFromDb
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
    }

    public class BankHolidaysFromDbWithRegionId : BankHolidaysFromDb
    {
        public int RegionId { get; set; }
    }

    public class EnglandAndWales
    {
        public string division { get; set; }
        public List<Event> events { get; set; }
    }

    public class Event
    {
        public string title { get; set; }
        public string date { get; set; }
        public string notes { get; set; }
        public bool bunting { get; set; }
    }

    public class NorthernIreland
    {
        public string division { get; set; }
        public List<Event> events { get; set; }
    }

    public class BankHolidays
    {
        [JsonPropertyName("england-and-wales")]
        public EnglandAndWales englandandwales { get; set; }
        public Scotland scotland { get; set; }

        [JsonPropertyName("northern-ireland")]
        public NorthernIreland northernireland { get; set; }
    }

    public class Scotland
    {
        public string division { get; set; }
        public List<Event> events { get; set; }
    }

    public enum BankHolidayRegions
    {
        englandandwales = 1,
        scotland,
        northernireland
    }
}
