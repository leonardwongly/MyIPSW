using System;
using System.Web;
using System.Web.UI;

namespace ipsw
{
    public partial class Link : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["result"] = "links";

            FirmwareSource parsedSource;
            if (SelectionStateValidator.TryParseSource(Request.QueryString["source"], out parsedSource)
                || SelectionStateValidator.TryParseSource(Request.QueryString["type"], out parsedSource))
            {
                query["source"] = new SelectionState { Source = parsedSource }.ToSourceQueryValue();
            }

            var device = SelectionStateValidator.NormalizeToken(Request.QueryString["device"], 64);
            if (!string.IsNullOrWhiteSpace(device))
            {
                query["device"] = device;
            }

            var version = SelectionStateValidator.NormalizeToken(Request.QueryString["version"], 32);
            if (!string.IsNullOrWhiteSpace(version))
            {
                query["version"] = version;
            }

            if (string.Equals(Request.QueryString["ui"], "next", StringComparison.OrdinalIgnoreCase))
            {
                query["ui"] = "next";
            }

            var destination = "~/Default.aspx?" + query;
            Response.Redirect(destination, false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}
