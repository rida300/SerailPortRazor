using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SerailPortRazor.Hubs
{
    public class MyHub: Hub
    {
        public async Task SendMessage(string user)
        {
            await Clients.All.SendAsync("ReceiveMessage: ", user);
        }
    }
}
