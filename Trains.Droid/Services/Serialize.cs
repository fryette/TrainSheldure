using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Trains.Infrastructure.Interfaces;
using System.Threading.Tasks;

namespace Trains.Droid.Services
{
	class Serialize : ISerializableService
	{
		public async Task<bool> CheckIsFile(string fileName)
		{
			return await Task.Run<bool>(() => { return false; });
		}

		public Task SerializeObjectToXml<T>(T obj, string fileName)
		{
			return new Task(() => { });
		}

		public Task DeleteFile(string fileName)
		{
			return new Task(() => { });
		}

		public async Task<T> ReadObjectFromXmlFileAsync<T>(string filename) where T : class
		{
			return await Task.Run<T>(() => { return default(T); });
		}
	}
}