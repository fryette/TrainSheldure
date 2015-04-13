using Cirrious.MvvmCross.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trains.Model.Entities;

namespace Trains.Core.ViewModels
{
    public class CarriageViewModel : MvxViewModel
    {
        public CarriageModel CarriageModel { get; set; }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// Set the default parameter of some properties.
        /// </summary>
        public void Init(string param)
        {
            CarriageModel = JsonConvert.DeserializeObject<CarriageModel>(param);
        }
    }
}
