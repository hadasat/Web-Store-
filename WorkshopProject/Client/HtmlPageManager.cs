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
            signIn,
            communication,
            navbar,
            signin_style,
            store,
            product,
            newStore,
            shoppingBasket,
            search,
            adminActions,
            payment,
            policies
        };

        private static readonly Dictionary<string, PagesNames> enumStringMapping = new Dictionary<string, PagesNames>()
        {
            {"/wot/main",PagesNames.Main },
            {"/wot/",PagesNames.Main },
            {"/wot",PagesNames.Main },
            {"/wot/index",PagesNames.Main },
            {"/wot/signin",PagesNames.signIn },
            {"/wot/signIn",PagesNames.signIn },
            {"/wot/communication.js",PagesNames.communication },
            {"/wot/navbar.js",PagesNames.navbar },
            {"/wot/signin.css",PagesNames.signin_style },
            {"/wot/store/[0-9]+",PagesNames.store},
            {"/wot/store/[0-9]+/[0-9]+",PagesNames.product},
            {"/wot/newstore",PagesNames.newStore},
            {"/wot/shoppingbasket",PagesNames.shoppingBasket},
            {"/wot/search",PagesNames.search},
            {"/wot/adminactions",PagesNames.adminActions},
            {"/wot/payment",PagesNames.payment},
            {"/wot/policies/[0-9]+",PagesNames.policies}
        };

        public static readonly Dictionary<PagesNames, string> htmlPages = new Dictionary<PagesNames, string>()
        {
            {PagesNames.Main , Properties.Resources.index },
            {PagesNames.signIn,Properties.Resources.SignIn },
            {PagesNames.communication,Properties.Resources.communication },
            {PagesNames.navbar,Properties.Resources.navbar },
            {PagesNames.signin_style,Properties.Resources.SignIn1 },
            {PagesNames.store,Properties.Resources.store },
            {PagesNames.product,Properties.Resources.product },
            {PagesNames.newStore,Properties.Resources.newStore },
            {PagesNames.shoppingBasket,Properties.Resources.shoppingBasket },
            {PagesNames.search,Properties.Resources.Search },
            {PagesNames.adminActions,Properties.Resources.adminActions },
            {PagesNames.payment,Properties.Resources.payment},
            {PagesNames.policies,Properties.Resources.policies},
            {PagesNames.Error,"<html><body>error path not found</body></html>" }
        };

        public static PagesNames getEnumByName(string name)
        {
            foreach (var curr in enumStringMapping)
            {
                var regexp = new System.Text.RegularExpressions.Regex("^"+curr.Key+"$");
                if (regexp.IsMatch(name))      
                    return enumStringMapping[curr.Key];
            }

            return PagesNames.Error;
        }


        public static string findPageByName(string requestedPage)
        {
            PagesNames pageName = HtmlPageManager.getEnumByName(requestedPage);
            return HtmlPageManager.htmlPages[pageName];
        }
    }
}
