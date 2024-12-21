// See https://aka.ms/new-console-template for more information

using hamonitor;


HttpClient client = new HttpClient();
Uptime uptime = new Uptime();
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
    bool wasRebooted = uptime.UptimeMonitor(HOME_ASSISTANT, SWITCH_URL, client);
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
