using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Manalo_Project.Models;
using Manalo_Project.Modules;
using Manalo_Project.Windows;

namespace Manalo_Project
{
    public class Rental_Establishments : ObservableObject
    {
        #region Fields

        private AddRoomWindow _addRoomWindow;
        private AddWorkersWindow _addWorkersWindow;
        private AssignWorkerWindow _assignWorkerWindow;
        private AddTenantWindow _addTenantWindow;
        private PayRentWindow _payRentWindow;

        private AddRoomVM _addRoomVm;
        private AddWorkerVM _addWorkerVm;
        private AssignWorkerVM _assignWorkerVm;
        private AddTenantVM _addTenantVm;
        private PayRentVM _payRentVm;
        private RoomHistory _roomHistory;

        private Floor _selectedFloor;
        private Room _selectedRoom;
        private Room _selectedRoomForDetails;
        private Tenant _selectedTenantForDetails;

        public ListCollectionView _roomdetailsview;
        public ListCollectionView _tenantdetailsview;
        public ListCollectionView _floordetailsview;

        private string _establishmentAddress;
        private string _establishmentName;
        private string _searchTenant;
        private bool _isAscending;
        private bool _isDescending;
        private string _selectedDisplayType;
        private Worker _selectedWorker;
        private bool _goodToAdd;

        #endregion

        #region Properties

        public float amount;

        public string EstablishmentName
        {
            get { return _establishmentName; }
            set
            {
                _establishmentName = value;
                RaisePropertyChanged(nameof(EstablishmentName));
            }
        }

        public string EstablishmentAddress
        {
            get { return _establishmentAddress; }
            set
            {
                _establishmentAddress = value;
                RaisePropertyChanged(nameof(EstablishmentAddress));
            }
        }

        public string SearchTenant
        {
            get { return _searchTenant; }
            set
            {
                _searchTenant = value;
                FilterTenants(SearchTenant);
                RaisePropertyChanged(nameof(SearchTenant));
            }
        }

        public AddRoomVM AddRoomVm
        {
            get { return _addRoomVm; }
            set
            {
                _addRoomVm = value;
                RaisePropertyChanged(nameof(AddRoomVm));
            }
        }

        public AddWorkerVM AddWorkerVm
        {
            get { return _addWorkerVm; }
            set
            {
                _addWorkerVm = value;
                RaisePropertyChanged(nameof(AddWorkerVm));
            }
        }

        public AssignWorkerVM AssignWorkerVm
        {
            get { return _assignWorkerVm; }
            set
            {
                _assignWorkerVm = value;
                RaisePropertyChanged(nameof(AssignWorkerVm));
            }
        }

        public AddTenantVM AddTenantVm
        {
            get { return _addTenantVm; }
            set
            {
                _addTenantVm = value;
                RaisePropertyChanged(nameof(AddTenantVm));
            }
        }

        public PayRentVM PayRentVm
        {
            get { return _payRentVm; }
            set
            {
                _payRentVm = value;
                RaisePropertyChanged(nameof(PayRentVm));
            }
        }

        public RoomHistory RoomHistory
        {
            get { return _roomHistory; }
            set
            {
                _roomHistory = value;
                RaisePropertyChanged(nameof(RoomHistory));
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

        public Room SelectedRoom
        {
            get { return _selectedRoom; }
            set
            {
                _selectedRoom = value;
                RaisePropertyChanged(nameof(SelectedRoom));
            }
        }

        public Room SelectedRoomForDetails
        {
            get { return _selectedRoomForDetails; }
            set
            {
                _selectedRoomForDetails = value;
                RaisePropertyChanged(nameof(SelectedRoomForDetails));
            }
        }

        public Tenant SelectedTenantForDetails
        {
            get { return _selectedTenantForDetails; }
            set
            {
                _selectedTenantForDetails = value;
                RaisePropertyChanged(nameof(SelectedTenantForDetails));
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

        public string SelectedDisplayType
        {
            get { return _selectedDisplayType; }
            set
            {
                _selectedDisplayType = value;
                DisplayRooms(SelectedDisplayType);
                RaisePropertyChanged(nameof(SelectedDisplayType));
            }
        }

        public bool GoodToAdd
        {
            get { return _goodToAdd; }
            set
            {
                _goodToAdd = value;
                RaisePropertyChanged(nameof(GoodToAdd));
            }
        }

        public bool IsAscending
        {
            get { return _isAscending; }
            set
            {
                _isAscending = value;
                if (_isAscending == true && IsDescending == true)
                {
                    IsDescending = false;
                }
                else { }
                RaisePropertyChanged(nameof(IsAscending));
            }
        }

        public bool IsDescending
        {
            get { return _isDescending; }
            set
            {
                _isDescending = value;
                if (_isDescending == true && IsAscending == true)
                {
                    IsAscending = false;
                }
                else
                { }
                RaisePropertyChanged(nameof(IsDescending));
            }
        }

        public int NumberOfFloors => Floors.Count;

        public ObservableCollection<Floor> Floors { get; } = new ObservableCollection<Floor>();
        public ObservableCollection<Room> AllRooms { get; } = new ObservableCollection<Room>();
        public ObservableCollection<Worker> Workers { get; } = new ObservableCollection<Worker>();
        public ObservableCollection<Worker> VacantWorkers { get; } = new ObservableCollection<Worker>();
        public ObservableCollection<Tenant> AllTenants { get; } = new ObservableCollection<Tenant>();
        public ObservableCollection<Tenant> TenantsListForPayment { get; } = new ObservableCollection<Tenant>();

        #endregion

        #region Constructors

        public Rental_Establishments()
        {
            AddRoomVm = new AddRoomVM();
            RoomHistory = new RoomHistory();
            RoomHistory.ExTenant = new Tenant();
            AddTenantVm = new AddTenantVM();
            AddWorkerVm = new AddWorkerVM();
            AssignWorkerVm = new AssignWorkerVM();
            InitializeView();
        }

        #endregion

        #region ICommands

        public ICommand OpenAddRoomWindowCommand => new RelayCommand(OpenAddRoomWindowProc);

        private void OpenAddRoomWindowProc()
        {
            AddRoomVm = new AddRoomVM();
            AddRoomVm.NewRoom = new Room();
            _addRoomWindow = new AddRoomWindow();
            _addRoomWindow.Owner = Application.Current.MainWindow;
            _addRoomWindow.ShowDialog();
        }

        public ICommand OpenAddTenantWindowCommand => new RelayCommand(OpenAddTenantWindowProc,OpenAddTenantWindowCondition);

        private void OpenAddTenantWindowProc()
        {
            AddTenantVm = new AddTenantVM();
            AddTenantVm.TenantVM = new Tenant();
            AddTenantVm.TenantVM.FatherOfTenant = new Person();
            AddTenantVm.TenantVM.MotherOfTenant = new Person();
            _addTenantWindow = new AddTenantWindow();
            _addTenantWindow.Owner = Application.Current.MainWindow;
            _addTenantWindow.ShowDialog();
        }

        private bool OpenAddTenantWindowCondition()
        {
            return AllRooms.Count != 0;
        }

        public ICommand OpenAddWorkersWindowCommand => new RelayCommand(OpenAddWorkersWindowProc);

        private void OpenAddWorkersWindowProc()
        {
            AddWorkerVm = new AddWorkerVM();
            AddWorkerVm.WorkerVM = new Worker();
            _addWorkersWindow = new AddWorkersWindow();
            _addWorkersWindow.Owner = Application.Current.MainWindow;
            _addWorkersWindow.ShowDialog();
        }

        public ICommand OpenAssignWorkersWindowCommand => new RelayCommand(OpenAssignWorkersWindowProc, OpenAssignWorkersWindowCondition);

        private void OpenAssignWorkersWindowProc()
        {
            AssignWorkerVm = new AssignWorkerVM();
            _assignWorkerWindow = new AssignWorkerWindow();
            _assignWorkerWindow.Owner = Application.Current.MainWindow;
            _assignWorkerWindow.ShowDialog();
        }

        private bool OpenAssignWorkersWindowCondition()
        {
            return VacantWorkers.Count != 0;
        }

        public ICommand OpenPayRentWindowCommand => new RelayCommand(OpenPayRentWindowProc, OpenPayRentWindowCondition);

        private void OpenPayRentWindowProc()
        {
            PayRentVm = new PayRentVM(TenantsListForPayment);
            PayRentVm.NewTenantPayment = new TenantPayment();
            PayRentVm.NewTenantPayment.TenantToPay = new Tenant();
            _payRentWindow = new PayRentWindow();
            _payRentWindow.Owner = Application.Current.MainWindow;
            _payRentWindow.ShowDialog();
        }

        private bool OpenPayRentWindowCondition()
        {
            return AllTenants.Count > 0;
        }

        public ICommand AddRoomToListCommand => new RelayCommand(AddRoomToListProc);

        private void AddRoomToListProc()
        {
            
            if ((AddRoomVm.NewRoom.RoomType != null) && (AddRoomVm.NewRoom.RoomNumber != 0) && (AddRoomVm.NewRoom.RoomPrice != 0))
            {

                AddRoomVm.GetCapacity();

                if (AddRoomVm.RoomsToBeAdded.Count == 0)
                {
                    AddRoomVm.RoomsToBeAdded.Add(AddRoomVm.NewRoom);
                    MessageBox.Show("Room has been added to the list of rooms.", "Add Room");
                    AddRoomVm.NewRoom = new Room();
                    AddRoomVm.NewRoom.RoomType = RoomTypes.Single.ToString();
                    GoodToAdd = false;
                }

                else
                {
                    GoodToAdd = true;

                    foreach (var room in AddRoomVm.RoomsToBeAdded)
                    {
                        if (room.RoomNumber == AddRoomVm.NewRoom.RoomNumber)
                        {
                            GoodToAdd = false;
                            break;
                        }
                    }

                    if (GoodToAdd == true)
                    {
                        AddRoomVm.RoomsToBeAdded.Add(AddRoomVm.NewRoom);
                        MessageBox.Show("Room has been added to the list of rooms.", "Add Room");
                        AddRoomVm.NewRoom = new Room();
                        AddRoomVm.NewRoom.RoomType = RoomTypes.Single.ToString();
                    }

                    else
                    {
                        MessageBox.Show("The room number is not available. Please enter another room number.", "Room Number");
                    }
                }
            }

            else
            {
                MessageBox.Show("Kindly fill in all the fields.", "Blank Fields");
            }
        }

        public ICommand AddRoomsToFloorCommand => new RelayCommand(AddRoomsToFloorProc, AddRoomsToFloorCondition);

        private void AddRoomsToFloorProc()
        {
            foreach (var room in AddRoomVm.RoomsToBeAdded)
            {
                room.FloorOfRoom = new Floor(SelectedFloor.FloorNumber);
                AllRooms.Add(room);
                SelectedFloor.RoomsInFloor.Add(room);
            }
            MessageBox.Show("The rooms have been added.", "Add Rooms");
            SelectedFloor = null;
            _addRoomWindow.Close();
        }

        private bool AddRoomsToFloorCondition()
        {
            return SelectedFloor != null && AddRoomVm.RoomsToBeAdded.Count != 0;
        }

        public ICommand AddWorkersToEstabishmentCommand => new RelayCommand(AddWorkersToEstabishmentProc, AddWorkersToEstabishmentCondition);

        private void AddWorkersToEstabishmentProc()
        {
            var selector = MessageBox.Show("Are you sure you want to add these workers?", "Add Workers",
                MessageBoxButton.YesNo);
            if (selector == MessageBoxResult.Yes)
            {
                foreach (var worker in AddWorkerVm.WorkersToBeAdded)
                {
                    worker.WorkerID = Workers.Count + DateTime.Now.ToFileTime();
                    Workers.Add(worker);
                    VacantWorkers.Add(worker);
                }
                MessageBox.Show("The workers has been successfully added.", "Add Workers");
                _addWorkersWindow.Close();
            }
            else
            {
                MessageBox.Show("The operation has been cancelled.", "Cancel");
            }
        }

        private bool AddWorkersToEstabishmentCondition()
        {
            return AddWorkerVm.WorkersToBeAdded.Count != 0;
        }

        public ICommand AssignWorkersCommand => new RelayCommand(AssignWorkersProc, AssignWorkersCondition);

        private void AssignWorkersProc()
        {
            if (AssignWorkerVm.SelectedFloor != null)
            {
                foreach (var assignedworker in AssignWorkerVm.WorkersToBeAssigned)
                {
                    foreach (var worker in Workers)
                    {
                        if (worker.WorkerID == assignedworker.WorkerID)
                        {
                            worker.Job = assignedworker.Job;
                            worker.AssignedFloor = AssignWorkerVm.SelectedFloor;
                            AssignWorkerVm.SelectedFloor.AssignedWorkers.Add(worker);
                        }
                    }
                }
                MessageBox.Show("All workers has been assigned.", "Assign Worker");
                _assignWorkerWindow.Close();
            }

            else
            {
                MessageBox.Show("Please select a floor.", "No Selected Floor");
            }
        }

        private bool AssignWorkersCondition()
        {
            return AssignWorkerVm.WorkersToBeAssigned.Count != 0;
        }

        public ICommand AddToAssignWorkerListCommand => new RelayCommand(AddToAssignWorkerListProc);

        private void AddToAssignWorkerListProc()
        {
            if (AssignWorkerVm.SelectedJob != null && AssignWorkerVm.SelectedWorker != null)
            {
                AssignWorkerVm.AddToWorkersToBeAssigned(VacantWorkers);
            }
            else
            {
                MessageBox.Show("Kindly answer all the fields.", "Unanswered Field");
            }
        }

        public ICommand CancelAssignWorkerCommand => new RelayCommand(CancelAssignWorkerProc);

        private void CancelAssignWorkerProc()
        {
            var selector = MessageBox.Show("Are you sure you want to cancel operation?", "Cancel",
                   MessageBoxButton.YesNo);
            if (selector == MessageBoxResult.Yes)
            {
                foreach (var worker in AssignWorkerVm.WorkersToBeAssigned)
                {
                    worker.Job = null;
                    VacantWorkers.Add(worker);
                }

                MessageBox.Show("Operation has been cancelled.", "Cancel");
                _assignWorkerWindow.Close();
            }
            
        }

        public ICommand AddTenantCommand => new RelayCommand(AddTenantProc, AddTenantCondition);

        private void AddTenantProc()
        {
            if (SelectedRoom.Occupants.Count < SelectedRoom.RoomCapacity)
            {
                AddTenant(SelectedRoom);
                RaisePropertyChanged(nameof(Floors));
                RaisePropertyChanged(nameof(SelectedRoom.NumberOfOccupants));
            }

            else
            {
                MessageBox.Show("The room is fully occupied. Please choose another room.", "Room Full");
            }
        }

        private bool AddTenantCondition()
        {
            return SelectedRoom != null;
        }

        public ICommand RemoveTenantCommand => new RelayCommand(RemoveTenantProc, RemoveTenantCondition);

        private void RemoveTenantProc()
        {
            var selector = MessageBox.Show("Are you sure you want to remove this tenant?", "Remove Tenant",
                MessageBoxButton.YesNo);

            if (selector == MessageBoxResult.Yes)
            {
                foreach (var roomHistory in SelectedTenantForDetails.OccupiedRoom.History)
                {
                    if (roomHistory.ExTenant != SelectedTenantForDetails) continue;
                    roomHistory.DateTimeLeft = DateTime.Now;
                    SelectedTenantForDetails.OccupiedRoom.Occupants.Remove(SelectedTenantForDetails);
                    SelectedTenantForDetails.OccupiedRoom.RaisePropertyChanged(nameof(SelectedTenantForDetails.OccupiedRoom.NumberOfOccupants));
                    AllTenants.Remove(SelectedTenantForDetails);
                    
                    MessageBox.Show("The tenant has been removed.", "Remove Tenant");
                }
            }

            else
            {
                MessageBox.Show("Operation has been cancelled.", "Cancel");
            }

        }

        private bool RemoveTenantCondition()
        {
            return SelectedTenantForDetails != null;
        }

        public ICommand PayRentCommand => new RelayCommand(PayRentProc);

        private void PayRentProc()
        {
            PayRentVm.TakeTenantForPay();
            if (PayRentVm.DummyTenantPaymentsList.Count > 0)
            {
                foreach (var tenantPayment in PayRentVm.DummyTenantPaymentsList)
                {
                    foreach (var tenant in AllTenants)
                    {
                        if (tenant.TenantID == tenantPayment.TenantToPay.TenantID)
                        {
                            amount = tenantPayment.AmountToPay;
                            tenant.Balance -= tenantPayment.AmountToPay;
                            tenant.RaisePropertyChanged(nameof(tenant.Balance));
                        }
                    }
                }

                if (amount != 0)
                {
                    MessageBox.Show("The balance of the tenants has been updated.", "Update");
                    PayRentVm = new PayRentVM(TenantsListForPayment);
                    PayRentVm.NewTenantPayment = new TenantPayment();
                    _payRentWindow.Close();
                }
                else
                {
                    MessageBox.Show("Please enter an amount for payment.", "Amount");
                }
            }

            else
            {
                MessageBox.Show("Please select a tenant.", "Select a tenant");
            }

            amount = 0;
        }

        public ICommand UpdateBalanceCommand => new RelayCommand(UpdateBalanceProc, UpdateBalanceCondition);

        private void UpdateBalanceProc()
        {
            foreach (var tenant in AllTenants)
            {
                tenant.Balance += tenant.OccupiedRoom.RoomPricePerPerson;
            }

            MessageBox.Show("The balance of the tenants have been updated.", "Balance");
        }

        private bool UpdateBalanceCondition()
        {
            return AllTenants.Count != 0;
        }

        #endregion

        #region Sort ICommands

        public ICommand SortRoomNumber => new RelayCommand(SortRoomNumberProc);

        private void SortRoomNumberProc()
        {
            _roomdetailsview?.SortDescriptions?.Clear();
            if (IsDescending == true)
            {
                _roomdetailsview?.SortDescriptions?.Add(new SortDescription("RoomNumber", ListSortDirection.Descending));
            }
            else if (IsAscending == true)
            {
                _roomdetailsview?.SortDescriptions?.Add(new SortDescription("RoomNumber", ListSortDirection.Ascending));
            }
        }

        public ICommand SortRoomType => new RelayCommand(SortRoomTypeProc);

        private void SortRoomTypeProc()
        {
            _roomdetailsview?.SortDescriptions?.Clear();
            if (IsDescending == true)
            {
                _roomdetailsview?.SortDescriptions?.Add(new SortDescription("RoomType", ListSortDirection.Descending));
            }
            else if (IsAscending == true)
            {
                _roomdetailsview?.SortDescriptions?.Add(new SortDescription("RoomType", ListSortDirection.Ascending));
            }
        }

        public ICommand SortRoomCapacity => new RelayCommand(SortRoomCapacityProc);

        private void SortRoomCapacityProc()
        {
            _roomdetailsview?.SortDescriptions?.Clear();
            if (IsDescending == true)
            {
                _roomdetailsview?.SortDescriptions?.Add(new SortDescription("RoomCapacity", ListSortDirection.Descending));
            }
            else if (IsAscending == true)
            {
                _roomdetailsview?.SortDescriptions?.Add(new SortDescription("RoomCapacity", ListSortDirection.Ascending));
            }
        }

        public ICommand SortNumberOfOccupants => new RelayCommand(SortNumberOfOccupantsProc);

        public void SortNumberOfOccupantsProc()
        {
            _roomdetailsview?.SortDescriptions?.Clear();
            if (IsDescending == true)
            {
                _roomdetailsview?.SortDescriptions?.Add(new SortDescription("NumberOfOccupants", ListSortDirection.Descending));
            }
            else if (IsAscending == true)
            {
                _roomdetailsview?.SortDescriptions?.Add(new SortDescription("NumberOfOccupants", ListSortDirection.Ascending));
            }

        }

        public ICommand SortFirstName => new RelayCommand(SortFirstNameProc);

        private void SortFirstNameProc()
        {
            _tenantdetailsview?.SortDescriptions?.Clear();
            if (IsDescending == true)
            {
                _tenantdetailsview?.SortDescriptions?.Add(new SortDescription("FirstName", ListSortDirection.Descending));
            }
            else if (IsAscending == true)
            {
                _tenantdetailsview?.SortDescriptions?.Add(new SortDescription("FirstName", ListSortDirection.Ascending));
            }
        }

        public ICommand SortMiddleName => new RelayCommand(SortMiddleNameProc);

        private void SortMiddleNameProc()
        {
            _tenantdetailsview?.SortDescriptions?.Clear();
            if (IsDescending == true)
            {
                _tenantdetailsview?.SortDescriptions?.Add(new SortDescription("MiddleName", ListSortDirection.Descending));
            }
            else if (IsAscending == true)
            {
                _tenantdetailsview?.SortDescriptions?.Add(new SortDescription("MiddleName", ListSortDirection.Ascending));
            }
        }

        public ICommand SortLastName => new RelayCommand(SortLastNameProc);

        private void SortLastNameProc()
        {
            _tenantdetailsview?.SortDescriptions?.Clear();
            if (IsDescending == true)
            {
                _tenantdetailsview?.SortDescriptions?.Add(new SortDescription("LastName", ListSortDirection.Descending));
            }
            else if (IsAscending == true)
            {
                _tenantdetailsview?.SortDescriptions?.Add(new SortDescription("LastName", ListSortDirection.Ascending));
            }
        }

        public ICommand SortAge => new RelayCommand(SortAgeProc);

        private void SortAgeProc()
        {
            _tenantdetailsview?.SortDescriptions?.Clear();
            if (IsDescending == true)
            {
                _tenantdetailsview?.SortDescriptions?.Add(new SortDescription("Age", ListSortDirection.Descending));
            }
            else if (IsAscending == true)
            {
                _tenantdetailsview?.SortDescriptions?.Add(new SortDescription("Age", ListSortDirection.Ascending));
            }
        }

        public ICommand SortGender => new RelayCommand(SortGenderProc);


        private void SortGenderProc()
        {
            _tenantdetailsview?.SortDescriptions?.Clear();
            if (IsDescending == true)
            {
                _tenantdetailsview?.SortDescriptions?.Add(new SortDescription("Gender", ListSortDirection.Descending));
            }
            else if (IsAscending == true)
            {
                _tenantdetailsview?.SortDescriptions?.Add(new SortDescription("Gender", ListSortDirection.Ascending));
            }
        }

        public ICommand SortOccupation => new RelayCommand(SortOccupationProc);

        private void SortOccupationProc()
        {
            _tenantdetailsview?.SortDescriptions?.Clear();
            if (IsDescending == true)
            {
                _tenantdetailsview?.SortDescriptions?.Add(new SortDescription("Occupation", ListSortDirection.Descending));
            }
            else if (IsAscending == true)
            {
                _tenantdetailsview?.SortDescriptions?.Add(new SortDescription("Occupation", ListSortDirection.Ascending));
            }
        }

        public ICommand SortContactNumber => new RelayCommand(SortContactNumberProc);

        private void SortContactNumberProc()
        {
            _tenantdetailsview?.SortDescriptions?.Clear();
            if (IsDescending == true)
            {
                _tenantdetailsview?.SortDescriptions?.Add(new SortDescription("ContactNumber", ListSortDirection.Descending));
            }
            else if (IsAscending == true)
            {
                _tenantdetailsview?.SortDescriptions?.Add(new SortDescription("ContactNumber", ListSortDirection.Ascending));
            }
        }

        #endregion

        #region Methods and Booleans

        public string[] DisplayOptions => Enum.GetNames(typeof(DisplayTypes));

        private void InitializeView()
        {
            _roomdetailsview = CollectionViewSource.GetDefaultView(AllRooms) as ListCollectionView;
            _tenantdetailsview = CollectionViewSource.GetDefaultView(AllTenants) as ListCollectionView;
            _floordetailsview = CollectionViewSource.GetDefaultView(Floors) as ListCollectionView;
            IsDescending = true;
        }

        private void AddTenant(Room selectedRoom)
        {
            if (ContentCheck(AddTenantVm) == true)
            {
                AddTenantVm.TenantVM.OccupiedRoom = new Room();
                RoomHistory = new RoomHistory();
                RoomHistory.ExTenant = new Tenant();

                AddTenantVm.TenantVM.Balance = selectedRoom.RoomPricePerPerson;
                AddTenantVm.TenantVM.TenantID = DateTime.Now.ToFileTime();

                AllTenants.Add(AddTenantVm.TenantVM);
                TenantsListForPayment.Add(AddTenantVm.TenantVM);
                selectedRoom.Occupants.Add(AddTenantVm.TenantVM);

                RoomHistory.ExTenant = AddTenantVm.TenantVM;
                RoomHistory.DateTimeEntered = DateTime.Now;
                selectedRoom.History.Add(RoomHistory);
                AddTenantVm.TenantVM.OccupiedRoom = selectedRoom;
                selectedRoom.RaisePropertyChanged(nameof(selectedRoom.NumberOfOccupants));
                selectedRoom = null;
                MessageBox.Show("The tenant has been added to the room.", "Add Tenant");
                _addTenantWindow.Close();
            }
            else
            {
                MessageBox.Show("Kindly answer all the fields.", "Blank Fields");
            }
        }

        private void FilterTenants(string value)
        {
            _tenantdetailsview.Filter = FiltertText;
        }

        private bool FiltertText(object o)
        {
            var tenant = o as Tenant;
            if (tenant == null) return false;
            if (tenant.FirstName.ToLowerInvariant().Contains(_searchTenant.ToLowerInvariant()) ||
                tenant.MiddleName.ToLowerInvariant().Contains(_searchTenant.ToLowerInvariant()) ||
                tenant.LastName.ToLowerInvariant().Contains(_searchTenant.ToLowerInvariant()) ||
                tenant.Gender.ToString().ToLowerInvariant().Contains(_searchTenant.ToLowerInvariant()) ||
                tenant.Occupation.ToLowerInvariant().Contains(_searchTenant.ToLowerInvariant()) ||
                tenant.ContactNumber.ToString().Contains(_searchTenant.ToLowerInvariant()) ||
                tenant.Age.ToString().Contains(_searchTenant.ToLowerInvariant()))
                return true;
            else
                return false;
        }

        private void DisplayRooms(string value)
        {
            if (value == "Occupied")
            {
                _roomdetailsview.Filter = Occupied;
            }

            if (value == "Vacant")
            {
                _roomdetailsview.Filter = Vacant;
            }

            if (value == "All")
            {
                _roomdetailsview.Filter = All;
            }
        }

        private bool Occupied(object o)
        {
            var room = o as Room;
            if (room == null) return false;
            if (room.NumberOfOccupants > 0)
                return true;
            else
                return false;
        }

        private bool Vacant(object o)
        {
            var room = o as Room;
            if (room == null) return false;
            if (room.NumberOfOccupants == 0)
                return true;
            else
                return false;
        }

        private bool All(object o)
        {
            var room = o as Room;
            return room != null;
        }

        private bool ContentCheck(AddTenantVM tenant)
        {
            return tenant.TenantVM.FirstName != null &&
                   tenant.TenantVM.MiddleName != null &&
                   tenant.TenantVM.LastName != null &&
                   tenant.TenantVM.Age != 0 &&
                   tenant.TenantVM.Occupation != null &&
                   tenant.TenantVM.ContactNumber != 0 &&
                   tenant.TenantVM.FatherOfTenant.FirstName != null &&
                   tenant.TenantVM.FatherOfTenant.MiddleName != null &&
                   tenant.TenantVM.FatherOfTenant.LastName != null &&
                   tenant.TenantVM.FatherOfTenant.ContactNumber != 0 &&
                   tenant.TenantVM.MotherOfTenant.FirstName != null &&
                   tenant.TenantVM.MotherOfTenant.MiddleName != null &&
                   tenant.TenantVM.MotherOfTenant.LastName != null &&
                   tenant.TenantVM.MotherOfTenant.ContactNumber != 0;
        }

        #endregion   
    }

    public class Dormitory : Rental_Establishments
    {
    }

    public class Apartment : Rental_Establishments
    {
    }

    public class BoardingHouse : Rental_Establishments
    {
    }

    public class Floor : ObservableObject
    {
        #region Fields

        private int _floorNumber;

        #endregion

        #region Properties

        public int FloorNumber
        {
            get { return _floorNumber; }
            set
            {
                _floorNumber = value;
                RaisePropertyChanged(nameof(FloorNumber));
            }
        }

        public ObservableCollection<Room> RoomsInFloor { get; } = new ObservableCollection<Room>();
        public ObservableCollection<Worker> AssignedWorkers { get; } = new ObservableCollection<Worker>();

        #endregion

        #region Constructors

        public Floor(int floornumber)
        {
            _floorNumber = floornumber;
        }

        #endregion
    }

    public class Room : ObservableObject
    {
        #region Fields

        private string _roomType;
        private int _roomNumber;
        private int _roomPrice;
        private int _roomCapacity;
        private Floor _floorOfRoom;

        #endregion

        #region Properties

        public string RoomType
        {
            get { return _roomType; }
            set
            {
                _roomType = value;
                RaisePropertyChanged(nameof(RoomType));
            }
        }    

        public int RoomNumber
        {
            get { return _roomNumber; }
            set
            {
                _roomNumber = value;
                RaisePropertyChanged(nameof(RoomNumber));
            }
        }

        public int RoomPrice
        {
            get { return _roomPrice; }
            set
            {
                _roomPrice = value;
                RaisePropertyChanged(nameof(RoomPrice));
            }
        }

        public int RoomCapacity
        {
            get { return _roomCapacity; }
            set
            {
                _roomCapacity = value;
                RaisePropertyChanged(nameof(RoomCapacity));
            }
        }

        public Floor FloorOfRoom
        {
            get { return _floorOfRoom; }
            set
            {
                _floorOfRoom = value;
                RaisePropertyChanged(nameof(FloorOfRoom));
            }
        }

        public int NumberOfOccupants => Occupants.Count;
        public float RoomPricePerPerson => RoomPrice / (float)RoomCapacity;

        public ObservableCollection<Tenant> Occupants { get; } = new ObservableCollection<Tenant>();
        public ObservableCollection<RoomHistory> History { get; } = new ObservableCollection<RoomHistory>();

        #endregion      
    }

    public enum RentalEstablishmentTypes
    {
        Dormitory,
        Apartment,
        BoardingHouse
    }

    public enum DisplayTypes
    {
        Occupied,
        Vacant,
        All
    }

}