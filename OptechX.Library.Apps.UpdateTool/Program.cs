using System.Text;
using System.Text.Json;
using OptechX.Library.Apps.UpdateTool.Models;

namespace OptechX.Library.Apps.UpdateTool
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Any(arg => arg.Contains("--version")))
            {
                Console.WriteLine("Version: 1.0.7");
                return;
            }

            if (args.Any(arg => arg.Contains("--help")))
            {
                Console.WriteLine("Usage: oxlaut --json <json_file>");
                return;
            }

            int index = Array.IndexOf(args, "--json");
            if (index == -1 || index == args.Length - 1)
            {
                // '--json' not found or found at the end of the arguments
                Console.WriteLine("Usage: oxlaut --json <json_file>");
                return;
            }

            string jsonFilePath = args[index + 1];

            if (!File.Exists(jsonFilePath))
            {
                Console.WriteLine($"The file {jsonFilePath} does not exist.");
                return;
            }

            // Read the JSON content from the file
            string jsonContent = File.ReadAllText(jsonFilePath);

            // Deserialize the JSON into the Application class
            Application jsonApp = JsonSerializer.Deserialize<Application>(jsonContent)!;

            // make the api request
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync($"https://definitely-firm-chamois.ngrok-free.app/api/Application/byuid/{jsonApp.UID}");
                    response.EnsureSuccessStatusCode();

                    // Read the response content as a string
                    string responseContent = await response.Content.ReadAsStringAsync();

                    // Now you can deserialize the JSON response to your Application class
                    List<Application> apps = JsonSerializer.Deserialize<List<Application>>(responseContent)!;

                    // Use the app object as needed...
                    if (apps.Count > 1)
                    {
                        Console.WriteLine("More than 1 app error, needs to be investigated");
                        return;
                    }

                    // replace existing object with new object and merge data
                    Application uApp = apps[0];
                    uApp.ApplicationCategory = jsonApp.ApplicationCategory;
                    uApp.Publisher = jsonApp.Publisher;
                    uApp.Name = jsonApp.Name;
                    uApp.Version = jsonApp.Version;
                    uApp.Copyright = jsonApp.Copyright;
                    uApp.LicenseAcceptRequired = jsonApp.LicenseAcceptRequired;
                    uApp.AddNewLcidValues(jsonApp);
                    uApp.AddNewCpuArchValues(jsonApp);
                    uApp.Homepage = jsonApp.Homepage;
                    uApp.Icon = jsonApp.Icon;
                    uApp.Docs = jsonApp.Docs;
                    uApp.License = jsonApp.License;
                    uApp.AddNewTagsValues(jsonApp);
                    uApp.Summary = jsonApp.Summary;
                    uApp.Enabled = jsonApp.Enabled;
                    uApp.BannerIcon = jsonApp.BannerIcon;

                    string apiUpdateApplicationUrl = $"https://definitely-firm-chamois.ngrok-free.app/api/Application/{uApp.Id}";

                    try
                    {
                        HttpResponseMessage updateResponse = await httpClient.PutAsync(apiUpdateApplicationUrl, new StringContent(JsonSerializer.Serialize(uApp), Encoding.UTF8, "application/json"));
                        updateResponse.EnsureSuccessStatusCode();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("Application updated: ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(uApp.UID);
                        Console.ResetColor();
                    }
                    catch (HttpRequestException ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        Console.WriteLine($"Unable to update application: {uApp.UID}");
                    }

                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");

                    string apiNewApplicationUrl = $"https://definitely-firm-chamois.ngrok-free.app/api/Application";

                    try
                    {
                        HttpResponseMessage updateResponse = await httpClient.PostAsync(apiNewApplicationUrl, new StringContent(JsonSerializer.Serialize(jsonApp), Encoding.UTF8, "application/json"));
                        updateResponse.EnsureSuccessStatusCode();
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("Posted new application: ");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(jsonApp.UID);
                        Console.ResetColor();
                    }
                    catch (HttpRequestException ex2)
                    {
                        Console.WriteLine($"Error: {ex2.Message}");
                    }

                    return;
                }
            }
        }
    }
}
