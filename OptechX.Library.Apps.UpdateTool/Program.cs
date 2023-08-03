using System.Text;
using System.Text.Json;
using OptechX.Library.Apps.UpdateTool.Constants;
using OptechX.Library.Apps.UpdateTool.Models;

namespace OptechX.Library.Apps.UpdateTool
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length != 1 && args.Length != 17)
            {
                Console.WriteLine("Usage: dotnet oxlaut.dll <app_uid>");
                Console.WriteLine("or");
                Console.WriteLine("Usage: dotnet oxlaut.dll <arg1> <arg2> ... <arg15>");
                return;
            }

            // declare variables
            string nUid;
            ApplicationCategory nApplicationCategory = ApplicationCategory.Default;
            string nPublisher;
            string nName;
            string nVersion;
            string nCopyright;
            bool nLicenseAcceptRequired;
            List<Lcid> nLcid;
            List<CpuArch> nCpuArch;
            string nHomepage;
            string nIcon;
            string nDocs;
            string nLicense;
            List<string> nTags;
            string nSummary;
            bool nEnabled;
            string nBannerIcon;

            nUid = args[0];

            // Make the API request
            using (HttpClient httpClient = new HttpClient())
            {
                string apiUrl = $"https://definitely-firm-chamois.ngrok-free.app/api/Application/byuid/{nUid}";
                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode();

                    // Read the response content as a string
                    string responseContent = await response.Content.ReadAsStringAsync();

                    // Now you can deserialize the JSON response to your Application class
                    List<Application> apps = JsonSerializer.Deserialize<List<Application>>(responseContent)!;

                    // Use the app object as needed...
                    if (apps.Count > 1)
                    {
                        return;
                    }

                    if (args.Length == 17)
                    {
                        nUid = args[0];
                        nApplicationCategory = Enum.Parse<ApplicationCategory>(args[1], true);
                        nPublisher = args[2];
                        nName = args[3];
                        nVersion = args[4];
                        nCopyright = args[5];
                        nLicenseAcceptRequired = bool.Parse(args[6]);
                        nLcid = args[7].Split(',').Select(lcidStr => Enum.Parse<Lcid>(lcidStr, true)).ToList();
                        nCpuArch = args[8].Split(',').Select(archStr => Enum.Parse<CpuArch>(archStr, true)).ToList();
                        nHomepage = args[9];
                        nIcon = args[10];
                        nDocs = args[11];
                        nLicense = args[12];
                        nTags = args[13].Split(',').ToList();
                        nSummary = args[14];
                        nEnabled = bool.Parse(args[15]);
                        nBannerIcon = args[16];

                        Application uApp = apps[0];
                        uApp.ApplicationCategory = nApplicationCategory;
                        uApp.Publisher = nPublisher;
                        uApp.Name = nName;
                        uApp.Version = nVersion;
                        uApp.Copyright = nCopyright;
                        uApp.LicenseAcceptRequired = nLicenseAcceptRequired;
                        uApp.Lcid = uApp.Lcid?.Union(nLcid.Except(uApp.Lcid ?? Enumerable.Empty<Lcid>())).ToList();
                        uApp.CpuArch = uApp.CpuArch?.Union(nCpuArch.Except(uApp.CpuArch ?? Enumerable.Empty<CpuArch>())).ToList();
                        uApp.Homepage = nHomepage;
                        uApp.Icon = nIcon;
                        uApp.Docs = nDocs;
                        uApp.License = nLicense;
                        uApp.Tags = uApp.Tags?.Union(nTags.Except(uApp.Tags ?? Enumerable.Empty<string>())).ToList();
                        uApp.Summary = nSummary;
                        uApp.Enabled = nEnabled;
                        uApp.BannerIcon = nBannerIcon;

                        string jsonString = JsonSerializer.Serialize(uApp);

                        Console.WriteLine(uApp.Id);
                        string apiUpdateUrl = $"https://definitely-firm-chamois.ngrok-free.app/api/Application/{uApp.Id}";
                        try
                        {
                            HttpResponseMessage updateResponse = await httpClient.PutAsync(apiUpdateUrl, new StringContent(jsonString, Encoding.UTF8, "application/json"));
                            updateResponse.EnsureSuccessStatusCode();

                            return;
                        }
                        catch (HttpRequestException ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                    }
                    else
                    {
                        return;
                    }
                    
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }
    }
}

