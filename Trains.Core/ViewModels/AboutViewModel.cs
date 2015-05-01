using Cirrious.MvvmCross.ViewModels;

namespace Trains.Core.ViewModels
{
    public class AboutViewModel : MvxViewModel
    {
        #region properties

        public string ApplicationName { get; set; }
        public string AboutUs { get; set; }
        public string Changes { get; set; }
        public string UsingLibrary { get; set; }

        #endregion

        #region actions

        public void Init(string param)
        {
            ApplicationName = ResourceLoader.Instance.Resource["ApplicationName"];
            AboutUs = ResourceLoader.Instance.Resource["AboutUs"];
            Changes = ResourceLoader.Instance.Resource["Changes"];
            UsingLibrary = ResourceLoader.Instance.Resource["UsingLibrary"];
        }

        #endregion
    }
}