using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using System.Xml;
using DotNetNuke.Entities.Portals;
using NBrightCore.common;
using NBrightDNN;
using Nevoweb.DNN.NBrightBuy.Components;
using Nevoweb.DNN.NBrightBuy.Components.Interfaces;
using System.Globalization;

namespace OS_ShippingProvider
{
    public class Provider : ShippingInterface
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cartInfo"></param>
        /// <returns></returns>
        public override NBrightInfo CalculateShipping(NBrightInfo cartInfo)
        {
            // return zero if we have invalid data
            cartInfo.SetXmlPropertyDouble("genxml/shippingcost", "0");
            cartInfo.SetXmlPropertyDouble("genxml/shippingcostTVA", "0");
            cartInfo.SetXmlPropertyDouble("genxml/shippingdealercost", "0");

            var modCtrl = new NBrightBuyController();
            var info = modCtrl.GetByGuidKey(PortalSettings.Current.PortalId, -1, "SHIPPING", Shippingkey);
            if (info == null) return cartInfo;


            double shippingcost = 10;
            if (cartInfo.GetXmlPropertyInt("genxml/OS_ShippingProviderlistidx") == 1)
            {
                shippingcost = 99.99;
            }

            var shippingdealercost = shippingcost;
            cartInfo.SetXmlPropertyDouble("genxml/shippingcostTVA", "0");
            cartInfo.SetXmlPropertyDouble("genxml/shippingcost", shippingcost);
            cartInfo.SetXmlPropertyDouble("genxml/shippingdealercost", shippingdealercost);

            return cartInfo;

        }

        public override string Shippingkey { get; set; }

        public override string Name()
        {
            var objCtrl = new NBrightBuyController();
            var info = objCtrl.GetPluginSinglePageData("OS_ShippingProvider", "SHIPPING", Utils.GetCurrentCulture());
            var rtn = info.GetXmlProperty("genxml/lang/genxml/textbox/name");
            if (rtn == "") rtn = info.GetXmlProperty("genxml/textbox/name");
            if (rtn == "") rtn = "OS_ShippingProvider";
            return rtn;
        }

        public override string GetTemplate(NBrightInfo cartInfo)
        {
            return GetTemplateData("carttemplate.cshtml", cartInfo); ;
        }

        public override string GetDeliveryLabelUrl(NBrightInfo cartInfo)
        {
            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templatename"></param>
        /// <param name="cartInfo"></param>
        /// <returns></returns>
        private String GetTemplateData(String templatename, NBrightInfo cartInfo)
        {

            var modCtrl = new NBrightBuyController();

            var info = modCtrl.GetPluginSinglePageData(Shippingkey, "SHIPPING", Utils.GetCurrentCulture());
            if (info == null) return "";

            var controlMapPath = HttpContext.Current.Server.MapPath("/DesktopModules/NBright/OS_ShippingProvider");
            var templCtrl = new NBrightCore.TemplateEngine.TemplateGetter(PortalSettings.Current.HomeDirectoryMapPath, controlMapPath, "Themes\\config", "");
            var templ = templCtrl.GetTemplateData(templatename, Utils.GetCurrentCulture());

            return templ;
        }


        public override bool IsValid(NBrightInfo cartInfo)
        {
            if (cartInfo.GetXmlPropertyDouble("genxml/totalweight") <= 99)
            {
                return true;
            }
            return false;
        }

    }

}
