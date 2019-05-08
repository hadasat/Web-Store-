using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkshopProject.Client;

namespace WorkshopProject.Client
{
    public class HtmlPageManager
    {
        public enum PagesNames
        {
            Main,
            Error,
            signIn
        };

        private static readonly Dictionary<string, PagesNames> enumStringMapping = new Dictionary<string, PagesNames>()
        {
            {"/wot/main",PagesNames.Main },
            {"/wot/",PagesNames.Main },
            {"/wot",PagesNames.Main },
            {"/wot/index",PagesNames.Main },
            {"/wot/signin",PagesNames.signIn },
            {"/wot/signIn",PagesNames.signIn }
        };

        public static readonly Dictionary<PagesNames, string> htmlPages = new Dictionary<PagesNames, string>()
        {
            {PagesNames.Main , Properties.Resources.index },
            {PagesNames.signIn,Properties.Resources.SignIn },
            {PagesNames.Error,"<html><body>error path not found>" },
        };

        public static PagesNames getEnumByName(string name)
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


        public static string findPageByName( string requestedPage)
        {
            PagesNames pageName = HtmlPageManager.getEnumByName(requestedPage);
            return HtmlPageManager.htmlPages[pageName];
        }
    }
}
