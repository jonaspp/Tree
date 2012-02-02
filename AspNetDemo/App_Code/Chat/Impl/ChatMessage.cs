using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Tree.Archeio.ObjectStore;

namespace AspNetDemo.Chat.Impl
{
    [Serializable()]
    public class ChatMessage : PersistentObject
    {
        public string Message { get; set; }
        public long Id { get; set; }
    }
    
}