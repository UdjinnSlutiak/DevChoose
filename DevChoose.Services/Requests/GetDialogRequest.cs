using System.Collections.Generic;
using DevChoose.Domain.Models;

namespace DevChoose.Services.Requests
{
    public class GetDialogRequest
    {
        public Dialog Dialog { get; set; }

        public User Companion { get; set; }

        public IEnumerable<Message> Messages { get; set; }
    }
}
