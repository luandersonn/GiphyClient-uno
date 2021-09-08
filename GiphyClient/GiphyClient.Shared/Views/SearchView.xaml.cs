using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace GiphyClient.Views
{
	public sealed partial class SearchView : Page
	{
		public double ItemSize { get => (double)GetValue(ItemSizeProperty); set => SetValue(ItemSizeProperty, value); }
		public static readonly DependencyProperty ItemSizeProperty = DependencyProperty.Register("ItemSize",
			typeof(double), typeof(SearchView), new PropertyMetadata(0.0));

		public double DesiredThumbnailSize { get => (double)GetValue(DesiredThumbnailSizeProperty); set => SetValue(DesiredThumbnailSizeProperty, value); }
		public static readonly DependencyProperty DesiredThumbnailSizeProperty = DependencyProperty.Register("DesiredThumbnailSize",
			typeof(double), typeof(SearchView), new PropertyMetadata(0.0));

		private readonly double MinThumbnailWidth = 50;
		private readonly double MaxThumbnailWidth = 350;
		private readonly double ThumbnailSliderStep = 100;

		public SearchView()
		{
			InitializeComponent();
			DesiredThumbnailSize = MinThumbnailWidth + ThumbnailSliderStep;
		}

		private void IncrementThumbnailWidth() => DesiredThumbnailSize = Math.Min(DesiredThumbnailSize + ThumbnailSliderStep, MaxThumbnailWidth);
		private void DecrementThumbnailWidth() => DesiredThumbnailSize = Math.Max(DesiredThumbnailSize - ThumbnailSliderStep, MinThumbnailWidth);
		private void DetermineItemSize()
		{
			if (GifsGridView != null)
			{
				// The 'margins' value represents the total of the margins around the
				// image in the grid item. 8 from the ItemTemplate root grid + 8 from
				// the ItemContainerStyle * (Right + Left). If those values change,
				// this value needs to be updated to match.
				var margins = (int)Resources["ItemMarginValue"] * 4;
				var gridWidth = GifsGridView.ActualWidth - (int)Resources["DefaultWindowSidePaddingValue"];
				var ItemWidth = sizeSlider.Value + margins;
				// We need at least 1 column.
				var columns = (int)Math.Max(gridWidth / ItemWidth, 1);

				// Adjust the available grid width to account for margins around each item.
				var adjustedGridWidth = gridWidth - (columns * margins);

				ItemSize = adjustedGridWidth / columns;
			}
			else
			{
				ItemSize = DesiredThumbnailSize;
			}
		}

		private void SearchViewQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
		{
			ViewModel.SearchText = args.QueryText;
			if (ViewModel.RefreshItems.CanExecute(null))
				ViewModel.RefreshItems.Execute(null);
		}

		private void OnGifTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
		{
			var element = (FrameworkElement)sender;
			Frame.Navigate(typeof(GifView), element.DataContext);
		}

		private void GoBack()
		{
			if (Frame.CanGoBack)
				Frame.GoBack();
		}
	}
}
