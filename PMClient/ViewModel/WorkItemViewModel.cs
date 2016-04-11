using GalaSoft.MvvmLight;
using System;

namespace PMClient.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class WorkItemViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the WorkItemsViewModel class.
        /// </summary>
        public WorkItemViewModel()
        {

        }

        /// <summary>
        /// The <see cref="Name" /> property's name.
        /// </summary>
        public const string NamePropertyName = "Name";

        private string _name = string.Empty;

        /// <summary>
        /// Gets the WelcomeTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                Set("Name", ref _name, value);
            }
        }

        /// <summary>
        /// The <see cref="Deadline" /> property's name.
        /// </summary>
        public const string DeadlinePropertyName = "Deadline";

        private DateTime _deadline = DateTime.Now;

        /// <summary>
        /// Gets the WelcomeTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public DateTime Deadline
        {
            get
            {
                return _deadline;
            }
            set
            {
                Set("Deadline", ref _deadline, value);
            }
        }

        /// <summary>
        /// The <see cref="Name" /> property's name.
        /// </summary>
        public const string PercentagePropertyName = "Percentage";

        private int _percentage = 0;

        /// <summary>
        /// Gets the WelcomeTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int Percentage
        {
            get
            {
                return _percentage;
            }
            set
            {
                Set("Percentage", ref _percentage, value);
            }
        }

        /// <summary>
        /// The <see cref="Description" /> property's name.
        /// </summary>
        public const string DescriptionPropertyName = "Description";

        private string _description = string.Empty;

        /// <summary>
        /// Gets the WelcomeTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                Set("Description", ref _description, value);
            }
        }

        public int Priority { get; set; }
    }
}