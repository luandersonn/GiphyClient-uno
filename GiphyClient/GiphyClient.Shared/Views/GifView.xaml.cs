using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace GiphyClient.Views
{
	public sealed partial class GifView : Page
	{
		public GifView() => InitializeComponent();

		protected override void OnNavigatedTo(NavigationEventArgs e) => DataContext = e.Parameter;

		private void GoBack()
		{
			if (Frame.CanGoBack)
				Frame.GoBack();
		}
	}
}
