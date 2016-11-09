using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace PayNote
{
    public class Localization
    {
        public static string Get(string key)
        {
            return ResourceLoader.GetForCurrentView().GetString(key);
        }
    }
}
