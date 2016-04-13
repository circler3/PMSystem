using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PMClient.Model;
using System;
using System.Collections.ObjectModel;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace PMClient.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase, IDisposable
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

        private RelayCommand _addCommand;

        /// <summary>
        /// Gets the MyCommand.
        /// </summary>
        public RelayCommand AddCommand
        {
            get
            {
                return _addCommand
                    ?? (_addCommand = new RelayCommand(
                    () =>
                    {
                        WorkItems.Add(new WorkItemViewModel() { Name = "<输入标题>", Description = "<输入描述>"});
                    }));
            }
        }

        private RelayCommand<object> _updateCommand;

        /// <summary>
        /// Gets the MyCommand.
        /// </summary>
        public RelayCommand<object> UpdateCommand
        {
            get
            {
                return _updateCommand
                    ?? (_updateCommand = new RelayCommand<object>(UpdateWorkItem));
            }
        }

        private void UpdateWorkItem(object o)
        {
            WorkItemViewModel w = (WorkItemViewModel)o;
            var mes = string.Format("{0} {1} {2} {3} {4} {5} {6}\r\n", "Update", w.Guid, w.Name, w.Description, w.Percentage, w.Deadline.ToShortDateString(), w.Priority);
            Send(mes);
        }

        private RelayCommand<object> _deleteCommand;

        /// <summary>
        /// Gets the MyCommand.
        /// </summary>
        public RelayCommand<object> DeleteCommand
        {
            get
            {
                return _deleteCommand
                    ?? (_deleteCommand = new RelayCommand<object>(DeleteWorkItem));
            }
        }

        private void DeleteWorkItem(object parameter)
        {
            WorkItemViewModel w = (WorkItemViewModel)parameter;
            var mes = string.Format("{0} {1} {2} {3} {4} {5} {6}\r\n", "Remove", w.Guid, w.Name, w.Description, w.Percentage, w.Deadline.ToShortDateString(), w.Priority);
            Send(mes);
            WorkItems.Remove(w);
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
            _workItems.Add(new WorkItemViewModel() { Name = "工作标题", Percentage = 30, Description= "完成一定精度的任务完成一定精度的任务完成一定精度的任务" });
            _workItems.Add(new WorkItemViewModel() { Name = "B work item", Percentage = 40 });

            tcpClient = new TcpClient();
            tcpClient.Connect(Properties.Settings.Default.ServerIP, Properties.Settings.Default.ServerPort);
            stream = tcpClient.GetStream();
            Thread th = new Thread(new ThreadStart(ReadData));
            th.IsBackground = true;
            th.Start();
        }

        private TcpClient tcpClient;
        private NetworkStream stream;


        public void Send(string message)
        {
            byte[] data = System.Text.Encoding.GetEncoding("gb2312").GetBytes(message);
            if(stream.CanWrite)
            {
                stream.Write(data, 0, data.Length);
            }
        }

        private void ReadData()
        {
            while (true)
            {
                if (stream.CanRead)
                {
                    byte[] readBuffer = new byte[1024];
                    //stream.BeginRead(readBuffer, 0, readBuffer.Length, new AsyncCallback((x) => { }), stream);
                    int count = stream.Read(readBuffer, 0, readBuffer.Length);
                    ProcessMessage(readBuffer, count);
                }
            }

        }

        private void ProcessMessage(byte[] buffer, int count)
        {
            string message = System.Text.Encoding.GetEncoding("gb2312").GetString(buffer, 0, count);
            if (message.StartsWith("Assign"))
            {

            }
        }

        public override void Cleanup()
        {
            // Clean up if needed
            if (stream != null)
            {
                stream.Close();
            }
            if(tcpClient!=null)
            {
                tcpClient.Close();
            }
            base.Cleanup();
        }

        public void Dispose()
        {
            Cleanup();
        }
    }
}