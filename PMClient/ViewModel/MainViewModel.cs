using GalaSoft.MvvmLight;
using PMClient.Model;
using System.Collections.ObjectModel;

namespace PMClient.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;

        /// <summary>
        /// The <see cref="WelcomeTitle" /> property's name.
        /// </summary>
        public const string WelcomeTitlePropertyName = "WelcomeTitle";

        private string _welcomeTitle = string.Empty;

        /// <summary>
        /// Gets the WelcomeTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string WelcomeTitle
        {
            get
            {
                return _welcomeTitle;
            }
            set
            {
                Set("WelcomeTitle", ref _welcomeTitle, value);
            }
        }

        /// <summary>
        /// The <see cref="WorkItems" /> property's name.
        /// </summary>
        public const string WorkItemsPropertyName = "WorkItems";

        private ObservableCollection<WorkItemViewModel> _workItems = new ObservableCollection<WorkItemViewModel>();

        /// <summary>
        /// Gets the WelcomeTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<WorkItemViewModel> WorkItems
        {
            get
            {
                return _workItems;
            }
            set
            {
                Set("WorkItems", ref _workItems, value);
            }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IDataService dataService)
        {
            _dataService = dataService;
            _dataService.GetData(
                (item, error) =>
                {
                    if (error != null)
                    {
                        // Report error here
                        return;
                    }

                    WelcomeTitle = item.Title;
                });
            _workItems.Add(new WorkItemViewModel() { Name = "A work item", Percentage = 30 });
            _workItems.Add(new WorkItemViewModel() { Name = "B work item", Percentage = 40 });
        }

      

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}