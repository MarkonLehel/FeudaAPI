using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeudaAPI.Models
{
    public class Message
    {
        public DateTime SendTime { get; }
        public string SentBy { get; }
        public string Text { get; set; }

        public Message(string sentBy, string text)
        {
            SentBy = sentBy;
            Text = text;
            SendTime = DateTime.UtcNow;
        }
    }
}
