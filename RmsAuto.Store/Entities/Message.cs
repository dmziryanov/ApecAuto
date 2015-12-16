using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RmsAuto.Store.Entities
{
    public class ClientMessage
    {
        public int? Id { get; set; }
        public string ClientName { get; set; }
        public string SendTo { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime Time { get; set; }
        public int? Viewed { get; set; }
        public byte? UserRole { get; set; }
    }
}
