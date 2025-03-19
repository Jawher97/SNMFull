using Newtonsoft.Json;
using Newtonsoft.Json;
using System;

namespace SNM.Instagram.Application.DTO
{
    public class InsightDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("period")]
        public string Period { get; set; }

        [JsonProperty("values")]
        public IList<ValueItem> Values { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        // Ajout des propriétés manquantes
        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("metric")]
        public string Metric { get; set; }

        [JsonProperty("value")]
        public int Value { get; set; }
    }

    public class ValueItem
    {
        [JsonProperty("value")]
        public int Value { get; set; }

        [JsonProperty("end_time")]
        public DateTime EndTime { get; set; }
    }
}

