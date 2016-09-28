using System.Collections.Generic;
using Cirrious.MvvmCross.Plugins.File;
using Trains.Infrastructure.Interfaces;

namespace Trains.Services
{
	public class StorageProvider : ISorageProvider
	{
		private readonly IMvxFileStore _fileStore;
		private readonly IJsonConverter _jsonConverter;

		public StorageProvider(IMvxFileStore fileStore, IJsonConverter jsonConverter)
		{
			_fileStore = fileStore;
			_jsonConverter = jsonConverter;
		}

		public bool Exists(string fileName)
		{
			return _fileStore.Exists(fileName);
		}

		public void Save<T>(T obj, string fileName)
		{
			_fileStore.WriteFile(fileName, _jsonConverter.Serialize(obj));
		}

		public void TryToRemove(string fileName)
		{
			if (Exists(fileName))
			{
				_fileStore.DeleteFile(fileName);
			}
		}

		public T ReadAndMap<T>(string fileName) where T : class
		{
			var fileContent = TryReadTextFile(fileName);
			return string.IsNullOrEmpty(fileContent) ? default(T) : _jsonConverter.Deserialize<T>(fileContent);
		}

		public void ClearAll()
		{
			foreach (var fileName in new List<string>(_fileStore.GetFilesIn(string.Empty)))
			{
				TryToRemoveFile(fileName);
			}
		}

		private string TryReadTextFile(string fileName)
		{
			string content;

			_fileStore.TryReadTextFile(fileName, out content);

			return content;
		}

		private void TryToRemoveFile(string fileName)
		{
			TryToRemove(fileName);
		}
	}
}