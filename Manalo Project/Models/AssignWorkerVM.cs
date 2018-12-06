using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace Manalo_Project.Models
{
    public class AssignWorkerVM:ObservableObject
    {
        #region Fields

        private Worker _selectedWorker;
        private Floor _selectedFloor;
        private string _selectedJob;
        private Worker _newWorker;

        #endregion

        #region Properties

        public string SelectedJob
        {
            get { return _selectedJob; }
            set
            {
                _selectedJob = value;
                RaisePropertyChanged(nameof(SelectedJob));
            }
        }

        public Floor SelectedFloor
        {
            get { return _selectedFloor; }
            set
            {
                _selectedFloor = value;
                RaisePropertyChanged(nameof(SelectedFloor));
            }
        }

        public Worker SelectedWorker
        {
            get { return _selectedWorker; }
            set
            {
                _selectedWorker = value;
                RaisePropertyChanged(nameof(SelectedWorker));
            }
        }

        public ObservableCollection<Worker> WorkersToBeAssigned { get; } = new ObservableCollection<Worker>();

        #endregion

        #region Methods

        public string[] JobOptions => Enum.GetNames(typeof(JobTypes));

        public void AddToWorkersToBeAssigned(ObservableCollection<Worker> vacantworker)
        {
            NewWorker.WorkerID = SelectedWorker.WorkerID;
            NewWorker.FirstName = SelectedWorker.FirstName;
            NewWorker.MiddleName = SelectedWorker.MiddleName;
            NewWorker.LastName = SelectedWorker.LastName;
            NewWorker.Job = SelectedJob;

            WorkersToBeAssigned.Add(NewWorker);
            vacantworker.Remove(SelectedWorker);
            NewWorker = new Worker();
        }

        #endregion

        public AssignWorkerVM()
        {
            NewWorker = new Worker();
        }

        public Worker NewWorker
        {
            get { return _newWorker; }
            set
            {
                _newWorker = value;
                RaisePropertyChanged(nameof(NewWorker));
            }
        }
    }

    public enum JobTypes
    {
        Cook,
        Janitor,
        Laundry,
    }
}
