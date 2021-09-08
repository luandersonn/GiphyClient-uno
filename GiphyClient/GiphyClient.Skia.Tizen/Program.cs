using Tizen.Applications;
using Uno.UI.Runtime.Skia;

namespace GiphyClient.Skia.Tizen
{
	class Program
	{
		static void Main(string[] args)
		{
			var host = new TizenHost(() => new GiphyClient.App(), args);
			host.Run();
		}
	}
}
