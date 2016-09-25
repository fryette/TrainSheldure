using System.Collections.Generic;
using Cirrious.MvvmCross.Plugins.File;
using Trains.Infrastructure.Interfaces;

namespace Trains.Services
{
	public class SerializableService : ISerializableService
	{
		private readonly IMvxFileStore _fileStore;
		private readonly IJsonConverter _jsonConverter;

		public SerializableService(IMvxFileStore fileStore, IJsonConverter jsonConverter)
		{
			_fileStore = fileStore;
			_jsonConverter = jsonConverter;
		}

		public bool Exists(string fileName)
		{
			return _fileStore.Exists(fileName);
		}

		public void Serialize<T>(T obj, string fileName)
		{
			_fileStore.WriteFile(fileName, _jsonConverter.Serialize(obj));
		}

		public void Delete(string fileName)
		{
			if (Exists(fileName))
				_fileStore.DeleteFile(fileName);

		}

		public T Desserialize<T>(string filename) where T : class
		{
			try
			{
				string textJson;
				_fileStore.TryReadTextFile(filename, out textJson);
				return textJson == null ? null : _jsonConverter.Deserialize<T>(textJson);
			}
			catch
			{
				return default(T);
			}
		}

		public void ClearAll()
		{
			foreach (var file in new List<string>(_fileStore.GetFilesIn("")))
				Delete(file);
		}
	}
}