using System;

namespace DevChoose.Domain.Models
{
    public class Message
    {
        public int Id { get; set; }
        public User Sender { get; set; }
        public User Receiver { get; set; }
        public string Text { get; set; }
        public DateTime Sent { get; set; }
        public int DialogId { get; set; }
    }
}
