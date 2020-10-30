using Newtonsoft.Json;
using System.Collections.Generic;

namespace Task.DTOModels
{
    public class JsonError
    {
        public JsonError(List<ErrorItem> errors)
        {
            Errors = errors;
        }

        [JsonProperty("errors")]
        public List<ErrorItem> Errors { get; set; }

        public class ErrorItem
        {
            public ErrorItem(int? id, string message)
            {
                Id = id;
                Message = message;
            }

            [JsonProperty("id")]
            public int? Id { get; set; }

            [JsonProperty("message")]
            public string Message { get; set; }
        }
    }
}