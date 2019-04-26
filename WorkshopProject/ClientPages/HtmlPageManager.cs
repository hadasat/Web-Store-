using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkshopProject.ClientPages
{
    class HtmlPageManager
    {

        public enum PagesNames {
            Main,
            Error
        };

        public static readonly Dictionary<string, PagesNames> enumStringMapping = new Dictionary<string, PagesNames>()
        {
            {"main",PagesNames.Main },
            {"/wot/main",PagesNames.Main },
            {"/wot/",PagesNames.Main },
            {"/wot",PagesNames.Main },
        };

        public static readonly Dictionary<PagesNames, string> htmlPages = new Dictionary<PagesNames, string>()
        {
            {PagesNames.Main , Properties.Resources.WSSclientExample },
            {PagesNames.Error,"<html><body>error path not found>" }
        };

        //todo check this
        /// <summary>
        /// first string is name second string is the file or path to file?? 
        /// </summary>
        public static readonly Dictionary<PagesNames, string> htmlPagesSecure = new Dictionary<PagesNames, string>()
        {
            {PagesNames.Main , Properties.Resources.WSSclientExample },
            {PagesNames.Error,"<html><body>error path not found>" }
        };

        public static PagesNames getEnumByName (string name)
        {
            foreach (var curr in enumStringMapping)
            {
                if (curr.Key == name.ToLower())
                {
                    return enumStringMapping[curr.Key];
                }
            }

            return PagesNames.Error;
        }


        public static string findPageByName(bool isSecureConnection, string requestedPage)
        {
            PagesNames pageName = HtmlPageManager.getEnumByName(requestedPage);
            string pageCode = isSecureConnection ?
                HtmlPageManager.htmlPagesSecure[pageName] :
                HtmlPageManager.htmlPages[pageName];

            return pageCode;
        }
    }
}
