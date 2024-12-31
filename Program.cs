// See https://aka.ms/new-console-template for more information

using hamonitor;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection().AddHttpClient();
var serviceProvider = services.BuildServiceProvider();

Uptime uptime = new Uptime(serviceProvider.GetService<IHttpClientFactory>());
RebootNotification reboot = new RebootNotification();
int waitTime = 60000;
int rebootCounter = 0;
string HOME_ASSISTANT = Environment.GetEnvironmentVariable("HOME_ASSISTANT");
string SWITCH_URL = Environment.GetEnvironmentVariable("SWITCH_URL");
string EMAIL = Environment.GetEnvironmentVariable("EMAIL");
string SEND_EMAIL = Environment.GetEnvironmentVariable("SEND_EMAIL");
bool KeepRunning = true;
while (KeepRunning)
{
    bool wasRebooted = uptime.UptimeMonitor(HOME_ASSISTANT, SWITCH_URL);
    if (wasRebooted)
    {
        rebootCounter++;
        waitTime += 60000;
        if (SEND_EMAIL == "true")
        {
            reboot.SendEmailNotification(rebootCounter, EMAIL);
        }
       
    }
    else
    {
        rebootCounter = 0;
    }
    Task.Delay(waitTime).Wait();
    if (waitTime >= 300000)
    {
        KeepRunning = false;
    }
}