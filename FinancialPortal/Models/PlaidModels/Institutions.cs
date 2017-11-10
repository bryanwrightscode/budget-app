using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialPortal.Models.PlaidModels
{
    public partial class Institutions
    {
        [JsonProperty("institutions")]
        public List<Institution> PurpleInstitutions { get; set; }

        [JsonProperty("request_id")]
        public string RequestId { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }
    }

    public partial class Institution
    {
        [JsonProperty("credentials")]
        public List<Credential> Credentials { get; set; }

        [JsonProperty("has_mfa")]
        public bool HasMfa { get; set; }

        [JsonProperty("institution_id")]
        public string InstitutionId { get; set; }

        [JsonProperty("mfa")]
        public List<string> Mfa { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("products")]
        public List<string> Products { get; set; }
    }

    public partial class Credential
    {
        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public partial class Institutions
    {
        public static Institutions FromJson(string json) => JsonConvert.DeserializeObject<Institutions>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Institutions self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    public class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
        };
    }
}