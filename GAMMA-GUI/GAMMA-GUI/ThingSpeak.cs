using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace GAMMA_GUI
{
    class ThingSpeak
    {
        // Reusing HttpClient to avoid performance issues of creating multiple instances
        private static readonly HttpClient client = new HttpClient();

        // Optionally set default timeouts or headers if needed
        static ThingSpeak()
        {
            client.Timeout = TimeSpan.FromSeconds(30); // Example timeout of 30 seconds
            // client.DefaultRequestHeaders.Add("User-Agent", "GAMMA_GUI ThingSpeak Client"); // Optional
        }

        // Send GET request asynchronously
        public static async Task<string> SendGetRequest(string url)
        {
            try
            {
                // Send the GET request and get the response
                HttpResponseMessage response = await client.GetAsync(url);

                // Ensure the response is successful
                response.EnsureSuccessStatusCode();

                // Return the content as a string
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException e)
            {
                // Log and rethrow the error for further handling
                Console.WriteLine($"Request error: {e.Message}");
                throw;
            }
            catch (TaskCanceledException e)
            {
                // Handle timeout or cancellation errors
                Console.WriteLine($"Request timeout or cancellation error: {e.Message}");
                throw;
            }
        }
    }
}
