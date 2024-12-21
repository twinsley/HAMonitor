using System.Net;
using System.Net.NetworkInformation;

namespace hamonitor;

public class Uptime
{
    public bool UptimeMonitor(string HOME_ASSISTANT, string SWITCH_URL, HttpClient client)
    {
        bool isSystemUp = PingSystem(HOME_ASSISTANT, client);
        Console.WriteLine(isSystemUp);
        if (!isSystemUp)
        {
            Task.Delay(20000).Wait();
            isSystemUp = PingSystem(HOME_ASSISTANT, client);
            if (!isSystemUp)
            {
                CyclePowerSwitch(client, SWITCH_URL);
                return true;
            }
        }

        return false;


    }

    private void CyclePowerSwitch(HttpClient client, string SWITCH_URL)
    {
        try
        {
            string Switch_Url_On = SWITCH_URL + "/rpc/Switch.Set?id=0&on=true";
            string Switch_Url_Off = SWITCH_URL + "/rpc/Switch.Set?id=0&on=false";
            Console.WriteLine($"Switching off... URL {Switch_Url_Off}");
            Console.WriteLine(client.GetAsync(Switch_Url_Off).Result);
            Console.WriteLine("Waiting...");
            Task.Delay(4000).Wait();
            Console.WriteLine($"Switching On... URL {Switch_Url_On}");
            Console.WriteLine(client.GetAsync(Switch_Url_On).Result);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    private bool PingSystem(string HOME_ASSISTANT, HttpClient client)
    {
        try
        {
            Console.WriteLine($"Ping to {HOME_ASSISTANT}...");
            var result = client.GetAsync(HOME_ASSISTANT + ":8123").Result;
            Console.WriteLine(result.StatusCode);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return true;
        }
    }
}