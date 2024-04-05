using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatOnline.Object
{
    class ChatData
    {
        public int IDChatData { get; set; }
        public DateTime SendTime { get; set; }
        public string Datatype { get; set; }
        public string Content { get; set; }
        public string sendUser { get; set; }
        public int Flag { get; set; }
    }
}
