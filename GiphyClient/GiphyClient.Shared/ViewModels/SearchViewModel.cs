using GiphyClient.Commands;
using GiphyClient.WebApi;
using System;
using System.Diagnostics;

namespace GiphyClient.ViewModels
{
	public class SearchViewModel : GiphyViewModelBase
	{
		#region fields
		private string _searchText;
		#endregion

		#region properties
		public string SearchText
		{
			get => _searchText;
			set
			{
				if (Set(ref _searchText, value))
				{
					NotifyCanExecuteChanged();
				}
			}
		}
		#endregion

		#region constructors
		public SearchViewModel()
		{
			RefreshItems = new RelayCommand(OnRefreshItems, CanRefreshItems);
			LoadMoreItems = new RelayCommand(OnLoadMoreItems, CanRefreshItems);
		}
		#endregion

		#region methods
		private bool CanRefreshItems(object arg) => !string.IsNullOrEmpty(SearchText) && !_isLoading;

		private void OnRefreshItems(object obj)
		{
			_offset = 0;
			_gifs.Clear();
			LoadMoreItems.Execute(obj);
		}

		private async void OnLoadMoreItems(object obj)
		{
			try
			{
				IsLoading = true;
				NotifyCanExecuteChanged();
				_client = new GiphyApiClient(ApiKey);
				var gifs = await _client.SearchGifsAsync(SearchText, _limit, _offset);
				int count = 0;
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

		private void NotifyCanExecuteChanged()
		{
			(LoadMoreItems as RelayCommand).NotifyCanExecuteChanged();
			(RefreshItems as RelayCommand).NotifyCanExecuteChanged();
		}
		#endregion
	}
}
