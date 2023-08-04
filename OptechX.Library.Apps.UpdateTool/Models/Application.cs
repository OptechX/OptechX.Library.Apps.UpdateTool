using System.Text.Json.Serialization;

namespace OptechX.Library.Apps.UpdateTool.Models
{
    public class Application
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("uid")]
        public string? UID { get; set; }

        [JsonPropertyName("lastUpdate")]
        public DateTime? LastUpdate { get; set; }

        [JsonPropertyName("applicationCategory")]
        public string? ApplicationCategory { get; set; }

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

        [JsonPropertyName("lcid")]
        public List<string>? Lcid { get; set; }

        [JsonPropertyName("cpuarch")]
        public List<string>? CpuArch { get; set; }

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
            LastUpdate = DateTime.UtcNow;
        }

        public void AddNewCpuArchValues(Application nApp)
        {
            if (CpuArch == null)
            {
                CpuArch = nApp.CpuArch?.ToList();
            }
            else if (nApp.CpuArch != null)
            {
                CpuArch.AddRange(nApp.CpuArch.Except(CpuArch));
            }
        }

        public void AddNewLcidValues(Application newApp)
        {
            if (Lcid == null)
            {
                Lcid = newApp.Lcid?.ToList();
            }
            else if (newApp.Lcid != null)
            {
                Lcid.AddRange(newApp.Lcid.Except(Lcid));
            }
        }

        public void AddNewTagsValues(Application newApp)
        {
            if (Tags == null)
            {
                Tags = newApp.Tags?.ToList();
            }
            else if (newApp.Tags != null)
            {
                Tags.AddRange(newApp.Tags.Except(Tags));
            }
        }
    }
}
