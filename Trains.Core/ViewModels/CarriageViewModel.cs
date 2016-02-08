using Cirrious.MvvmCross.ViewModels;
using Newtonsoft.Json;
using Trains.Model.Entities;

namespace Trains.Core.ViewModels
{
	public class CarriageViewModel : MvxViewModel
	{
		public CarriageModel CarriageModel { get; set; }

		public void Init(string param)
		{
			CarriageModel = JsonConvert.DeserializeObject<CarriageModel>(param);
		}
	}
}
