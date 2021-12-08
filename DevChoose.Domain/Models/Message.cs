using System;

namespace DevChoose.Domain.Models
{
    public class Message
    {
        public int Id { get; set; }

        public int SenderId { get; set; }

        public string Text { get; set; }

        public DateTime Sent { get; set; }

        public int DialogId { get; set; }
    }
}
