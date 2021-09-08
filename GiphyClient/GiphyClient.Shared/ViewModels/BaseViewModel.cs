using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GiphyClient.ViewModels
{
	public abstract class BaseViewModel : INotifyPropertyChanged
	{
		protected virtual bool Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
		{
			if (Equals(storage, value))
				return false;
			storage = value;
			OnPropertyChanged(propertyName);
			return true;
		}
		protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		public event PropertyChangedEventHandler PropertyChanged;
	}
}