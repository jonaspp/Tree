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
using System.Collections.Generic;

namespace AspNetDemo.Chat.Impl
{
    public class ChatImpl : IChat
    {
        private List<string> messages = new List<string>();

        public ChatImpl()
        {
        }

        public void Send(string who, string message)
        {
            messages.Add(string.Format("[{0}] <b>{1}</b> says<br>{2}<br>", DateTime.Now.ToLongTimeString(), who, message)); 
        }

        public List<string> Messages()
        {
            return messages;       
        }
    }
}
