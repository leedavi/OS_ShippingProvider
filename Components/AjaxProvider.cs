using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Compilation;
using System.Xml;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Services.Localization;
using NBrightCore.common;
using NBrightDNN;
using Nevoweb.DNN.NBrightBuy.Components;
using Nevoweb.DNN.NBrightBuy.Components.Products;
using Nevoweb.DNN.NBrightBuy.Components.Interfaces;
using RazorEngine.Compilation.ImpromptuInterface.InvokeExt;

namespace OS_ShippingProvider
{
    public class AjaxProvider : AjaxInterface
    {
        public override string Ajaxkey { get; set; }

        public override string ProcessCommand(string paramCmd, HttpContext context, string editlang = "")
        {
            var ajaxInfo = NBrightBuyUtils.GetAjaxFields(context);
            var lang = NBrightBuyUtils.SetContextLangauge(ajaxInfo); // Ajax breaks context with DNN, so reset the context language to match the client.
            var objCtrl = new NBrightBuyController();

            var strOut = "OS_ShippingProvider Error";
            switch (paramCmd)
            {
                case "os_shippingprovider_getsettings":
                    var info1 = objCtrl.GetPluginSinglePageData("OS_ShippingProvider", "SHIPPING", lang);
                    strOut = NBrightBuyUtils.RazorTemplRender("settingsfields.cshtml", 0, "", info1, "/DesktopModules/NBright/OS_ShippingProvider", "config", lang, StoreSettings.Current.Settings());
                    break;
                case "os_shippingprovider_savesettings":
                    strOut = objCtrl.SavePluginSinglePageData(context);
                    break;
                case "os_shippingprovider_selectlang":
                    objCtrl.SavePluginSinglePageData(context);
                    var nextlang = ajaxInfo.GetXmlProperty("genxml/hidden/nextlang");
                    var info2 = objCtrl.GetPluginSinglePageData("OS_ShippingProvider", "SHIPPING", nextlang);
                    strOut = NBrightBuyUtils.RazorTemplRender("settingsfields.cshtml", 0, "", info2, "/DesktopModules/NBright/OS_ShippingProvider", "config", nextlang, StoreSettings.Current.Settings());
                    break;
                case "os_shippingprovider_getcarttotals":
                    var cartd = new CartData(PortalSettings.Current.PortalId);

                    cartd.PurchaseInfo.SetXmlProperty("genxml/OS_ShippingProvidermessage", "");
                    cartd.PurchaseInfo.SetXmlProperty("genxml/OS_ShippingProviderlistidx", ajaxInfo.GetXmlProperty("genxml/radiobuttonlist/list"));
                    cartd.PurchaseInfo.SetXmlProperty("genxml/OS_ShippingProviderlistcode", "");
                    cartd.PurchaseInfo.SetXmlProperty("genxml/OS_ShippingProvideraddress", "");

                    cartd.Save();
                    strOut = NBrightBuyUtils.RazorTemplRender("CheckoutTotals.cshtml", 0, "", cartd, "/DesktopModules/NBright/NBrightBuy", "Default", Utils.GetCurrentCulture(), StoreSettings.Current.Settings());
                    break;
            }

            return strOut;

        }

        public override void Validate()
        {
            throw new NotImplementedException();
        }

    }
}
