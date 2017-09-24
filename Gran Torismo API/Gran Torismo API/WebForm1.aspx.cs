using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gran_Torismo_API
{
    public partial class WebForm1 : System.Web.UI.Page
    {

        MongoConnection miConexion = new MongoConnection();

        protected void Page_Load(object sender, EventArgs e)
        {
            Button1.Text = miConexion.getPersonas()[3].Nombre.ToString();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

        }
    }
}