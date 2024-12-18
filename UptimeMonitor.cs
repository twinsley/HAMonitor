using System.Net.NetworkInformation;

namespace hamonitor;

public class Uptime
{
    public bool UptimeMonitor(string HOME_ASSISTANT, string SWITCH_URL, HttpClient client)
    {
        bool isSystemUp = PingSystem(HOME_ASSISTANT);
        Console.WriteLine(isSystemUp);
        if (!isSystemUp)
        {
            Task.Delay(20000).Wait();
            isSystemUp = PingSystem(HOME_ASSISTANT);
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
    private bool PingSystem(string HOME_ASSISTANT)
    {
        Ping pingSender = new Ping();
        Console.WriteLine($"Ping to {HOME_ASSISTANT}...");
        PingReply reply = pingSender.Send(HOME_ASSISTANT);
        Console.WriteLine(reply.Status);

        if (reply.Status == IPStatus.Success)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}