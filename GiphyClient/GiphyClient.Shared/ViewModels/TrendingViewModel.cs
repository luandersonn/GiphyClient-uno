using GiphyClient.Commands;
using GiphyClient.WebApi;
using System;
using System.Diagnostics;

namespace GiphyClient.ViewModels
{
	public class TrendingViewModel : GiphyViewModelBase
	{
		public TrendingViewModel()
		{
			LoadMoreItems = new RelayCommand(LoadMoreItemsAsync, CanLoadMoreItems);
			RefreshItems = new RelayCommand(OnRefreshItems, CanLoadMoreItems);
			_limit = 50;
		}

		private bool CanLoadMoreItems(object arg) => !IsLoading;

		private async void LoadMoreItemsAsync(object arg)
		{
			try
			{
				IsLoading = true;
				NotifyCanExecuteChanged();
				_client = new GiphyApiClient(ApiKey);
				var gifs = await _client.GetTrendingGifsAsync(_limit, _offset);
				var count = 0;
				foreach (var gif in gifs)
				{
					_gifs.Add(gif);
					count++;
				}
				_offset += count;
			}
			catch (Exception e)
			{
				Debug.WriteLine(e);
			}
			finally
			{
				IsLoading = false;
				NotifyCanExecuteChanged();
			}
		}

		private void OnRefreshItems(object arg)
		{
			_offset = 0;
			_gifs.Clear();
			LoadMoreItemsAsync(arg);
		}

		private void NotifyCanExecuteChanged()
		{
			(LoadMoreItems as RelayCommand).NotifyCanExecuteChanged();
			(RefreshItems as RelayCommand).NotifyCanExecuteChanged();
		}

	}
}
