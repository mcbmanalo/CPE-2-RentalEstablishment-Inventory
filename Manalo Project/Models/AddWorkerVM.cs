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
    public class AddWorkerVM:ObservableObject
    {
        private Worker _workerVm;
        private Worker _selectedWorker;
        public string[] GenderOptions => Enum.GetNames(typeof(GenderType));

        public ObservableCollection<Worker> WorkersToBeAdded { get; } = new ObservableCollection<Worker>();
        public ICommand AddWorkerToListCommand => new RelayCommand(AddWorkerToListProc);
        public ICommand RemoveWorkerFromListCommand => new RelayCommand(RemoveWorkerFromListProc,RemoveWorkerFromListCondition);

        private bool RemoveWorkerFromListCondition()
        {
            return SelectedWorker != null;
        }

        private void RemoveWorkerFromListProc()
        {
            var selector = MessageBox.Show("Are you sure you want to remove this worker?","Remove Worker",MessageBoxButton.YesNo);
            if (selector == MessageBoxResult.Yes)
            {
                WorkersToBeAdded.Remove(SelectedWorker);
                MessageBox.Show("The worker has been removed from the list.", "Remove Worker");
            }
            else
            {
                MessageBox.Show("The operation has been cancelled.", "Cancel");
            }
        }

        public bool WorkerCheck()
        {
            return WorkerVM.FirstName != null && WorkerVM.MiddleName != null && WorkerVM.LastName != null &&
                   WorkerVM.Age > 0 && WorkerVM.ContactNumber > 0;
        }

        private void AddWorkerToListProc()
        {
            //if (WorkerVM.FirstName != null && WorkerVM.MiddleName != null && WorkerVM.LastName != null &&
            //    WorkerVM.Age > 0 && (WorkerVM.Gender == Manalo_Project.GenderType.Female || WorkerVM.Gender == Manalo_Project.GenderType.Male)
            //    && WorkerVM.ContactNumber > 0)
            if (WorkerCheck() == true)
            {
                WorkersToBeAdded.Add(WorkerVM);
                MessageBox.Show("Worker has been added to the list.", "Add Worker");
                WorkerVM = new Worker();
            }
            else
            {
                MessageBox.Show("Kindly answer all the fields.", "Unanswered Fields");
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

        public Worker WorkerVM
        {
            get { return _workerVm; }
            set
            {
                _workerVm = value;
                RaisePropertyChanged(nameof(WorkerVM));
            }
        }
    }

    public enum GenderType
    {
        Male,
        Female
    }
}
