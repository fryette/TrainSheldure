﻿using System.Threading.Tasks;

namespace Trains.Core.Interfaces
{
    public interface ISerializableService
    {
        Task<bool> CheckIsFile(string fileName);
        Task SerializeObjectToXml<T>(T obj, string fileName);
        Task DeleteFile(string fileName);
        Task<T> ReadObjectFromXmlFileAsync<T>(string filename) where T : class;
    }
}
