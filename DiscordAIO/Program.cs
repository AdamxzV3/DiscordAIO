using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

class Program
{
    static async Task Main(string[] args)
    {
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine(@"
 _________            .___    __________      .__                     _____  .___________   
\_   ___ \  ____   __| _/____\______   \__ __|  |   ______ ____     /  _  \ |   \_____  \  
/    \  \/ /  _ \ / __ |/ __ \|     ___/  |  \  |  /  ___// __ \   /  /_\  \|   |/   |   \ 
\     \___(  <_> ) /_/ \  ___/|    |   |  |  /  |__\___ \\  ___/  /    |    \   /    |    \
 \______  /\____/\____ |\___  >____|   |____/|____/____  >\___  > \____|__  /___\_______  /
        \/            \/    \/                         \/     \/          \/            \/ 

Welcome to CodePulse AIO!
This tool is made by CodePulse

Select an option:
[1] Remove webhook
[2] Spam webhook
[3] Exit");
        Console.ResetColor();

        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Enter the webhook URL to remove:");
                string webhookUrl = Console.ReadLine();
                await RemoveWebhookAsync(webhookUrl);
                Console.WriteLine("Webhook removed successfully.");
                break;

            case "2":
                Console.WriteLine("Enter the webhook URL to spam:");
                string webhookUrl2 = Console.ReadLine();
                Console.WriteLine("Enter the number of times to spam the webhook:");
                int count = int.Parse(Console.ReadLine());
                Console.WriteLine("Enter the message to send:");
                string message = Console.ReadLine();
                await SpamWebhookAsync(webhookUrl2, count, message);
                Console.WriteLine($"Webhook spammed {count} times with message: {message}");
                break;

            case "3":
                Environment.Exit(0);
                break;

            default:
                Console.WriteLine("Invalid option selected.");
                break;
        }
    }

    static async Task RemoveWebhookAsync(string webhookUrl)
    {
        using var httpClient = new HttpClient();
        var response = await httpClient.DeleteAsync(webhookUrl);
        response.EnsureSuccessStatusCode();
    }

    static async Task SpamWebhookAsync(string webhookUrl, int count, string message)
    {
        using var httpClient = new HttpClient();

        for (int i = 0; i < count; i++)
        {
            var data = new
            {
                content = message
            };
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(webhookUrl, content);
            if (!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error: {response.StatusCode} - {responseContent}");
            }
            response.EnsureSuccessStatusCode();
        }
    }
}
