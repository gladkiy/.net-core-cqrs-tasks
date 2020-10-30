using Newtonsoft.Json;

namespace Task.DTOModels
{
    public class Json
    {
        public Json(object data = null, object meta = null)
        {
            Data = data;
            Meta = meta;
        }

        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public object Data { get; set; }

        [JsonProperty("meta", NullValueHandling = NullValueHandling.Ignore)]
        public object Meta { get; set; }
    }
}