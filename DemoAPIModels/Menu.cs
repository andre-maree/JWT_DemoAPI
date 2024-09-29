using Newtonsoft.Json;

namespace Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Text { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<MenuItem> Children { get; set; }
    }
}
