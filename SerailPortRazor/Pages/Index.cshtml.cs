using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.IO.Ports;
using System.IO;
using Microsoft.AspNetCore.SignalR;
using SerailPortRazor.Hubs;

namespace SerailPortRazor.Pages
{
    public class IndexModel : PageModel
    {

        private readonly IHubContext<MyHub> HubContext;

        public SerialPort potty;
        public string SerialData;
        public static Action<String> UpdateAction { get; set; }
        public IndexModel(IHubContext<MyHub> hubContext)
        {
            HubContext = hubContext;
            potty = new SerialPort
            {
                BaudRate = 9600,
                PortName = "COM11"
            };

            potty.Open();
            UpdateAction = ReceiveMessage;
            getMyData();
        }
        public static void Update(string msg)
        {
            UpdateAction.Invoke(msg);
        }
        public async void ReceiveMessage(string message1)
        {
            await HubContext.Clients.All.SendAsync("ReceiveMessage", message1);
            
        }

        public void OnGet()
        {
            
        }   

        public void OnPostConect()
        {

        }
        
        public void getMyData()
        {
            byte[] buffer = new byte[4096];
            Action kickoffRead = null;
            kickoffRead = delegate
            {
                potty.BaseStream.BeginRead(buffer, 0, buffer.Length, delegate (IAsyncResult ar)
                {
                    try
                    {
                        int actualLength = potty.BaseStream.EndRead(ar);
                        byte[] received = new byte[actualLength];
                        Buffer.BlockCopy(buffer, 0, received, 0, actualLength);
                        SerialData = System.Text.Encoding.Default.GetString(received);
                        Update(SerialData);
                        //raiseAppSerialDataEvent(received);
                    }
                    catch (IOException exc)
                    {
                        SerialData = "Fucked";
                    }
                    kickoffRead();
                }, null);
            };
            kickoffRead();
        }
    }
}
