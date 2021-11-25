using System.Collections.Generic;

namespace DevChoose.Domain.Models
{
    public class Dialog
    {
        public int Id { get; set; }
        public User Developer { get; set; }
        public User Customer { get; set; }
        public IEnumerable<Message> Messages {get; set; }
    }
}
