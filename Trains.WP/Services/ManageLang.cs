using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trains.Core.Interfaces;

namespace Trains.WP.Services
{
    public class ManageLang : IManageLangService
    {
        public void ChangeAppLanguage(string cultureName)
        {
            var culture = new CultureInfo(cultureName);
            Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = culture.Name;
            var resourceContext = Windows.ApplicationModel.Resources.Core.ResourceContext.GetForCurrentView();
            resourceContext.Reset();
        }
    }
}
