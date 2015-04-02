using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trains.Services.Interfaces
{
	public interface ITestService
	{
		Task<string> GetHtml(Uri url);
	}
}
