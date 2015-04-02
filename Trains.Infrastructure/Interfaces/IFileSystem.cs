using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trains.Infrastructure.Interfaces
{
    public interface IFileSystem
    {
       Task<string> GetFileContents(string path);
    }
}
