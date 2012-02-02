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
using Tree.Injector;
using Tree.Grafeas;
using Tree.Lifecycle;
using Tree.Archeio.ObjectStore;

namespace AspNetDemo.Chat.Impl
{
    public class ChatImpl : IChat, IInitializable
    {
        [Inject()]
        private ILogger logger;

        [Inject()]
        private IObjectStore store;

        private List<ChatMessage> messages = new List<ChatMessage>();

        public ChatImpl()
        {
        }

        public void Send(string who, string message)
        {
            ChatMessage c = new ChatMessage();
            String text = string.Format("[{0}] <b>{1}</b> says<br>{2}<br>", DateTime.Now.ToLongTimeString(), who, message);
            c.Message = text;
            messages.Add(c);
            store.Store<ChatMessage>(c);
            logger.Log(text);
        }

        public List<ChatMessage> Messages()
        {
            return messages;       
        }

        public void Initialize()
        {
            messages = store.GetAll<ChatMessage>();
        }
    }
}
