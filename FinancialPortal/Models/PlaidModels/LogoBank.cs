using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialPortal.Models.PlaidModels
{
    public partial class LogoBank
    {
        [JsonProperty("institution")]
        public Institution Institution { get; set; }

        [JsonProperty("request_id")]
        public string RequestId { get; set; }
    }

    public partial class Institution
    {
        [JsonProperty("colors")]
        public Colors Colors { get; set; }

        //[JsonProperty("credentials")]
        //public List<Credential> Credentials { get; set; }

        //[JsonProperty("has_mfa")]
        //public bool HasMfa { get; set; }

        //[JsonProperty("institution_id")]
        //public string InstitutionId { get; set; }

        [JsonProperty("logo")]
        public string Logo { get; set; }

        //[JsonProperty("mfa")]
        //public List<object> Mfa { get; set; }

        //[JsonProperty("name")]
        //public string Name { get; set; }

        [JsonProperty("name_break")]
        public object NameBreak { get; set; }

        //[JsonProperty("products")]
        //public List<string> Products { get; set; }

        [JsonProperty("url_account_locked")]
        public string UrlAccountLocked { get; set; }

        [JsonProperty("url_account_setup")]
        public string UrlAccountSetup { get; set; }

        [JsonProperty("url_forgotten_password")]
        public string UrlForgottenPassword { get; set; }
    }

    public partial class Credential
    {
        //[JsonProperty("label")]
        //public string Label { get; set; }

        //[JsonProperty("name")]
        //public string Name { get; set; }

        //[JsonProperty("type")]
        //public string Type { get; set; }
    }

    public partial class Colors
    {
        [JsonProperty("dark")]
        public string Dark { get; set; }

        [JsonProperty("darker")]
        public string Darker { get; set; }

        [JsonProperty("light")]
        public string Light { get; set; }

        [JsonProperty("primary")]
        public string Primary { get; set; }
    }

    public partial class LogoBank
    {
        public static LogoBank FromJson(string json) => JsonConvert.DeserializeObject<LogoBank>(json, ConverterLogo.Settings);
    }

    public static class SerializeLogo
    {
        public static string ToJson(this LogoBank self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    public class ConverterLogo
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
        };
    }
}