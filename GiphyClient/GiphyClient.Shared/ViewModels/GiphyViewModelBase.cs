using GiphyClient.Models;
using GiphyClient.WebApi;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace GiphyClient.ViewModels
{
	public abstract class GiphyViewModelBase : BaseViewModel
	{
		#region fields		
		protected bool _isLoading;
		protected ObservableCollection<Gif> _gifs;
		protected GiphyApiClient _client;
		protected int _offset;
		protected int _limit;
		#endregion

		#region properties		
		public bool IsLoading { get => _isLoading; protected set => Set(ref _isLoading, value); }
		public ReadOnlyObservableCollection<Gif> Gifs { get; protected set; }
		public string ApiKey
		{
			get => Windows.Storage.ApplicationData.Current.LocalSettings.Values["ApiKey"] as string;
			set => Windows.Storage.ApplicationData.Current.LocalSettings.Values["ApiKey"] = value;
		}
		#endregion

		#region commands		
		public ICommand LoadMoreItems { get; protected set; }
		public ICommand RefreshItems { get; protected set; }
		#endregion

		#region constructor		
		public GiphyViewModelBase()
		{
			_gifs = new ObservableCollection<Gif>();
			Gifs = new ReadOnlyObservableCollection<Gif>(_gifs);

			_offset = 0;
			_limit = 50;
		}
		#endregion
	}
}
