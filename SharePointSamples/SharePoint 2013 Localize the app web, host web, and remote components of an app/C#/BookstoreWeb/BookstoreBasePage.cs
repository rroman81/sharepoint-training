using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Threading;
using System.Globalization;

namespace BookstoreWeb
{
    public class BookstoreBasePage : System.Web.UI.Page
    {
        protected override void InitializeCulture()
        {
            if (Request.QueryString["SPLanguage"] != null)
            {
                string selectedLanguage = Request.QueryString["SPLanguage"];
                UICulture = selectedLanguage;
                Culture = selectedLanguage;

                Thread.CurrentThread.CurrentCulture =
                    CultureInfo.CreateSpecificCulture(selectedLanguage);
                Thread.CurrentThread.CurrentUICulture = new
                    CultureInfo(selectedLanguage);
            }
            base.InitializeCulture();
        }
    }
}