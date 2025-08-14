using IO.Ably;
using IO.Ably.Realtime;


using System;
using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Core.ApplicationCore.Interfaces.HelperFiles;

public class AblyPublisher:IAblyService
{
    public readonly IRealtimeClient  _Client;

    public AblyPublisher()
    {
       // change to this Environment.GetEnvironmentVariable("MY_API_KEY")
        string apiKey = "T6f7Iw.WEI6WQ:NE-bz1_TczRbZdfMxyXGHGKJ46ogedn6BRzx78rEVyE";
        _Client = new AblyRealtime(apiKey);
        
    }

    public async Task BroadcastAnnounccementsAsync(AnnouncementEntry announcements)
    {
        try
        {
            var channel = _Client.Channels.Get($"announcements:{announcements.SchoolId}");


            await channel.PublishAsync("new-annoucnement", new
            {
                title = announcements.Title,
                message = announcements.Message,
                sender = announcements.SenderId,
                tags = announcements.Tags,
                Date = announcements.Date,
                priority = announcements.Priority,
            });
            
            _Client.Close();
        }
        catch (AblyException e)
        {
            Console.WriteLine($"Ably Broadcast Failed: {e.Message}");
        }
}
}