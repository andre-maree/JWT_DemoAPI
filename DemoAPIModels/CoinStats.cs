namespace Models
{
    public class CoinsMeta
    {
        public int page { get; set; }
        public int limit { get; set; }
        public int itemCount { get; set; }
        public int pageCount { get; set; }
        public bool hasPreviousPage { get; set; }
        public bool hasNextPage { get; set; }
    }

    public class Coin
    {
        public string id { get; set; }
        public string icon { get; set; }
        public string name { get; set; }
        public string symbol { get; set; }
        public int rank { get; set; }
        public double price { get; set; }
        public double priceBtc { get; set; }
        public double volume { get; set; }
        public double marketCap { get; set; }
        public object availableSupply { get; set; }
        public object totalSupply { get; set; }
        public double fullyDilutedValuation { get; set; }
        public double priceChange1h { get; set; }
        public double priceChange1d { get; set; }
        public double priceChange1w { get; set; }
        public string redditUrl { get; set; }
        public string twitterUrl { get; set; }
        public List<string> explorers { get; set; }
        public string websiteUrl { get; set; }
        public string contractAddress { get; set; }
        public int? decimals { get; set; }
    }

    public class CoinsRoot
    {
        public List<Coin> result { get; set; }
        public CoinsMeta meta { get; set; }
    }
}
