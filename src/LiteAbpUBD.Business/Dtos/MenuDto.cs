

using Newtonsoft.Json;

namespace LiteAbpUBD.Business.Dtos
{
    public class MenuDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("pid")]
        public int Pid { get; set; }
        [JsonProperty("icon")]
        public string Icon { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("route")]
        public string Route { get; set; }
        [JsonProperty("order")]
        public int Order { get; set; }
    }
}
