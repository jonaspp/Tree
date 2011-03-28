using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Tree.Injector;
using AspNetDemo.Chat;
using Tree.Container;
using Tree.Factory;
using AspNetDemo.Chat.Impl;

public partial class _Default : Page 
{
    private IChat chat = ObjectContainer.Lookup<IChat>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Render();
        }
    }

    private void Render()
    {
        LabelMessages.Text = "";
        foreach (String str in chat.Messages())
        {
            LabelMessages.Text = str + "<br>" + LabelMessages.Text;
        }
    }

    protected void ButtonChat_Click(object sender, EventArgs e)
    {
        chat.Send(TextBoxWho.Text, TextBoxMessage.Text);
        Render();
        TextBoxMessage.Text = "";
        TextBoxMessage.Focus();
    }
}
