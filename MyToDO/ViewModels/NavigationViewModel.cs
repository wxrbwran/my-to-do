using MyToDO.Extensions;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;

namespace MyToDO.ViewModels
{
	public class NavigationViewModel : BindableBase, INavigationAware
	{
		private readonly IContainerProvider provider;
		private readonly IEventAggregator aggregator;

		public NavigationViewModel(IContainerProvider provider)
		{
			this.provider = provider;
			aggregator = provider.Resolve<IEventAggregator>();
		}
		public virtual bool IsNavigationTarget(NavigationContext navigationContext)
		{
			return true;
		}

		public virtual void OnNavigatedFrom(NavigationContext navigationContext)
		{

		}

		public virtual void OnNavigatedTo(NavigationContext navigationContext)
		{

		}

		public void UpdateLoading(bool IsOpen)
		{
			aggregator.UpdateLoading(new Events.UpdateModal()
			{
				IsOpen = IsOpen
			});
		}
	}
}
