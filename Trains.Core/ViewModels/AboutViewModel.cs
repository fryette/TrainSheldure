using Cirrious.MvvmCross.ViewModels;
using Trains.Core.Resources;

namespace Trains.Core.ViewModels
{
    public class AboutViewModel : MvxViewModel
    {
        #region properties

        public string ApplicationName { get; set; }
        public string AboutUs { get; set; }
        public string Changes { get; set; }
        public string UsingLibrary { get; set; }
        public string SpecialThank { get; set; }
        public string SecondDesigner { get; set; }
        public string FirstDesigner { get; set; }
        
        #endregion

        #region actions

        public void Init(string param)
        {
            ApplicationName = ResourceLoader.Instance.Resource["ApplicationName"];
            AboutUs = ResourceLoader.Instance.Resource["AboutUs"];
            Changes = ResourceLoader.Instance.Resource["Changes"];
            UsingLibrary = ResourceLoader.Instance.Resource["UsingLibrary"];
            SpecialThank = ResourceLoader.Instance.Resource["SpecialThank"];
            FirstDesigner = ResourceLoader.Instance.Resource["FirstDesigner"];
            SecondDesigner = ResourceLoader.Instance.Resource["SecondDesigner"];

        }

        #endregion
    }
}