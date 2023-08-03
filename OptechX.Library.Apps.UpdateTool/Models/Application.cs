using System.Text.Json;
using System.Text.Json.Serialization;
using OptechX.Library.Apps.UpdateTool.Constants;

namespace OptechX.Library.Apps.UpdateTool.Models
{
    public class Application
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("uid")]
        public string? UID { get; set; }

        [JsonPropertyName("lastUpdate")]
        public DateTime LastUpdate { get; set; }

        [JsonPropertyName("applicationCategory")]
        public ApplicationCategory ApplicationCategory { get; set; }

        [JsonPropertyName("publisher")]
        public string? Publisher { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("version")]
        public string? Version { get; set; }

        [JsonPropertyName("copyright")]
        public string? Copyright { get; set; }

        [JsonPropertyName("licenseAcceptRequired")]
        public bool LicenseAcceptRequired { get; set; } = false;

        [JsonConverter(typeof(LcidConverter))]
        [JsonPropertyName("lcid")]
        public List<Lcid>? Lcid { get; set; }

        [JsonConverter(typeof(CpuArchConverter))]
        [JsonPropertyName("cpuarch")]
        public List<CpuArch>? CpuArch { get; set; }

        [JsonPropertyName("homepage")]
        public string? Homepage { get; set; }

        [JsonPropertyName("icon")]
        public string? Icon { get; set; }

        [JsonPropertyName("docs")]
        public string? Docs { get; set; }

        [JsonPropertyName("license")]
        public string? License { get; set; }

        [JsonPropertyName("tags")]
        public List<string>? Tags { get; set; }

        [JsonPropertyName("summary")]
        public string? Summary { get; set; }

        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; } = true;

        [JsonPropertyName("bannerIcon")]
        public string? BannerIcon { get; set; }

        public Application()
        {
            LastUpdate = DateTime.Today.Date;
        }

        public class CpuArchConverter : JsonConverter<List<CpuArch>>
        {
            public override List<CpuArch>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    var archList = new List<CpuArch>();
                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.EndArray)
                        {
                            return archList;
                        }

                        if (reader.TokenType == JsonTokenType.String && Enum.TryParse<CpuArch>(reader.GetString(), true, out var cpuArch))
                        {
                            archList.Add(cpuArch);
                        }
                    }
                }

                return null;
            }

            public override void Write(Utf8JsonWriter writer, List<CpuArch> value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();

                foreach (var arch in value)
                {
                    writer.WriteStringValue(arch.ToString());
                }

                writer.WriteEndArray();
            }
        }


        public class LcidConverter : JsonConverter<List<Lcid>>
        {
            public override List<Lcid>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    var lcidList = new List<Lcid>();
                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.EndArray)
                        {
                            return lcidList;
                        }

                        if (reader.TokenType == JsonTokenType.String && Enum.TryParse<Lcid>(reader.GetString(), true, out var lcid))
                        {
                            lcidList.Add(lcid);
                        }
                    }
                }

                return null;
            }

            public override void Write(Utf8JsonWriter writer, List<Lcid> value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();

                foreach (var lcid in value)
                {
                    writer.WriteStringValue(lcid.ToString());
                }

                writer.WriteEndArray();
            }
        }
    }
}
