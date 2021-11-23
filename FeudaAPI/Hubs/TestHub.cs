using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;


namespace FeudaAPI.Hubs
{
    public class TestHub : Hub
    {
        public void LogMessage(string message)
        {
            Debug.WriteLine(message);
            Debug.WriteLine(Clients);
        }

    }
}
