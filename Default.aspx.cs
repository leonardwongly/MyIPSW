using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net;
using System.Data;
using Newtonsoft.Json;
using System.Web.Script;
using System.Web.Script.Serialization;
using System.IO;
using System.Collections;

namespace MyIPSW
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            WebClient webClient = new WebClient();
            string myJSON = webClient.DownloadString("https://api.ipsw.me/v4/devices");

            dynamic jsonObj = JsonConvert.DeserializeObject(myJSON);
            for (int i = 0; i < jsonObj.Count; i++)
            {

                string name = jsonObj[i]["name"].ToString();
                string identifier = jsonObj[i]["identifier"].ToString();
                ddliPhone.Items.Add(new ListItem(name,identifier));
            }
            
        }
    }
}