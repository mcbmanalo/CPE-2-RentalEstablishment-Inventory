using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Manalo_Project.Models;

namespace Manalo_Project.Modules
{
    public class RentalEstablishmentModule : ObservableObject
    {
        private string _selectedRentalEstablishmentType;
        private int _typedNumberOfFloors;
        private string _searchRentalEstablishment;
        private Rental_Establishments _selectedRentalEstablishment;
        private ListCollectionView _view;


        public ObservableCollection<Rental_Establishments> OwnedRentalEstablishments { get; } =
            new ObservableCollection<Rental_Establishments>();

        public string[] RentalEstablishmentOptions => Enum.GetNames(typeof(RentalEstablishmentTypes));
        public AddRentalEstablishmentWindow _addRentalEstablishmentWindow;
        private NewRentalEstablishmentVM _newRentalEstablishmentVm;

        public ICommand OpenAddRentalWindowCommand => new RelayCommand(OpenAddRentalWindowProc);
        public ICommand AddRentalWindowCommand => new RelayCommand(AddRentalWindowProc);

        public RentalEstablishmentModule()
        {
            NewRentalEstablishmentVm = new NewRentalEstablishmentVM();
            //NewRentalEstablishmentVm.NewRentalEstablishment = new Apartment();
            //NewRentalEstablishmentVm.NewRentalEstablishment.EstablishmentName = "Liz-An";
            //NewRentalEstablishmentVm.NewRentalEstablishment.EstablishmentAddress = "Jacinto St., Davao City";
            //NewRentalEstablishmentVm.InputFloor(5);
            //OwnedRentalEstablishments.Add(NewRentalEstablishmentVm.NewRentalEstablishment);
            LoadEstablishments();
            InitializeView();
        }

        public void LoadEstablishments()
        {
            var reader = new StreamReader(Path.Combine(Directory.GetCurrentDirectory(),"Rental Establishment.txt"));
            string value;
            while ((value = reader.ReadLine()) != null)
            {
                var split = value.Split(',');
                if (split[0].Trim() == "Apartment")
                {
                    var establishment = new Apartment();
                    establishment.EstablishmentName = split[1].Trim();
                    establishment.EstablishmentAddress = split[2].Trim();
                    for (int i = 1; i <= int.Parse(split[3].Trim()); i++)
                    {
                        establishment.Floors.Add(new Floor(i));
                    }
                    OwnedRentalEstablishments.Add(establishment);
                }

                if (split[0].Trim() == "BoardingHouse")
                {
                    var establishment = new BoardingHouse();
                    establishment.EstablishmentName = split[1].Trim();
                    establishment.EstablishmentAddress = split[2].Trim();
                    for (int i = 1; i <= int.Parse(split[3].Trim()); i++)
                    {
                        establishment.Floors.Add(new Floor(i));
                    }
                    OwnedRentalEstablishments.Add(establishment);
                }

                if (split[0].Trim() == "Dormitory")
                {
                    var establishment = new Dormitory();
                    establishment.EstablishmentName = split[1].Trim();
                    establishment.EstablishmentAddress = split[2].Trim();
                    for (int i = 1; i <= int.Parse(split[3].Trim()); i++)
                    {
                        establishment.Floors.Add(new Floor(i));
                    }
                    OwnedRentalEstablishments.Add(establishment);
                }
               
                //var thing = new Jewelry
                //{
                //    ItemID = split[0].Trim(),
                //    MaterialType = (JewelryMaterialTypes)Enum.Parse(typeof(JewelryMaterialTypes), split[1]),
                //    RarityType = (RarityTypes)Enum.Parse(typeof(RarityTypes), split[2]),
                //    Quantity = int.Parse(split[3])
                //};
                //Things.Add(thing);
            }
        }

        public NewRentalEstablishmentVM NewRentalEstablishmentVm
        {
            get { return _newRentalEstablishmentVm; }
            set
            {
                _newRentalEstablishmentVm = value;
                RaisePropertyChanged(nameof(NewRentalEstablishmentVm));
            }
        }

        public Rental_Establishments SelectedRentalEstablishment
        {
            get { return _selectedRentalEstablishment; }
            set
            {
                _selectedRentalEstablishment = value;
                RaisePropertyChanged(nameof(SelectedRentalEstablishment));
            }
        }

        public string SearchRentalEstablishment
        {
            get { return _searchRentalEstablishment; }
            set
            {
                _searchRentalEstablishment = value;
                RaisePropertyChanged(nameof(SearchRentalEstablishment));
                FilterRentalEstablishment(SearchRentalEstablishment);
            }
        }

        private void FilterRentalEstablishment(string value)
        {
            _view.Filter = FiltertText;
        }

        private bool FiltertText(object o)
        {
            var item = o as Rental_Establishments;
            if (item == null) return false;
            if (item.EstablishmentName.ToLowerInvariant().Contains(_searchRentalEstablishment.ToLowerInvariant()) ||
                item.EstablishmentAddress.ToLowerInvariant().Contains(_searchRentalEstablishment.ToLowerInvariant()) ||
                item.NumberOfFloors.ToString().Contains(_searchRentalEstablishment.ToLowerInvariant()))
                return true;
            else
                return false;


        }

        private void InitializeView()
        {
            _view = CollectionViewSource.GetDefaultView(OwnedRentalEstablishments) as ListCollectionView;
        }

        public int TypedNumberOfFloors
        {
            get { return _typedNumberOfFloors; }
            set
            {
                _typedNumberOfFloors = value;
                RaisePropertyChanged(nameof(TypedNumberOfFloors));
            }
        }

        public string SelectedRentalEstablishmentType
        {
            get { return _selectedRentalEstablishmentType; }
            set
            {
                _selectedRentalEstablishmentType = value;
                RaisePropertyChanged(nameof(SelectedRentalEstablishmentType));
                SelectedEstablishmentType();
            }
        }

        public void SelectedEstablishmentType()
        {
            if (SelectedRentalEstablishmentType == RentalEstablishmentTypes.Dormitory.ToString())
            {
                NewRentalEstablishmentVm.NewRentalEstablishment = new Dormitory();
            }

            if (SelectedRentalEstablishmentType == RentalEstablishmentTypes.Apartment.ToString())
            {
                NewRentalEstablishmentVm.NewRentalEstablishment = new Apartment();
            }

            if (SelectedRentalEstablishmentType == RentalEstablishmentTypes.BoardingHouse.ToString())
            {
                NewRentalEstablishmentVm.NewRentalEstablishment = new BoardingHouse();
            }
        }

        private void AddRentalWindowProc()
        {
            if (TypedNumberOfFloors < 1000)
            {
                if (TypedNumberOfFloors > 0 && NewRentalEstablishmentVm.NewRentalEstablishment.EstablishmentName != null &&
                    NewRentalEstablishmentVm.NewRentalEstablishment.EstablishmentAddress != null)
                {
                    var establishment = NewRentalEstablishmentVm.NewRentalEstablishment;
                    StreamWriter writer = null;

                    if (establishment.GetType() == typeof(Apartment))
                    {
                        var newestablishment = establishment as Apartment;
                        writer = File.AppendText(Path.Combine(Directory.GetCurrentDirectory(), "Rental Establishment.txt"));
                        writer.WriteLine($"Apartment, {newestablishment.EstablishmentName}, {newestablishment.EstablishmentAddress}, {TypedNumberOfFloors}");
                    }

                    if (establishment.GetType() == typeof(BoardingHouse))
                    {
                        var newestablishment = establishment as BoardingHouse;
                        writer = File.AppendText(Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "Rental Establishment.txt")));
                        writer.WriteLine($"BoardingHouse, {newestablishment.EstablishmentName}, {newestablishment.EstablishmentAddress}, {TypedNumberOfFloors}");
                    }

                    if (establishment.GetType() == typeof(Dormitory))
                    {
                        var newestablishment = establishment as Dormitory;
                        writer = File.AppendText(Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "Rental Establishment.txt")));
                        writer.WriteLine($"Dormitory, {newestablishment.EstablishmentName}, {newestablishment.EstablishmentAddress}, {TypedNumberOfFloors}");
                    }

                    writer?.Flush();
                    writer?.Close();
                    writer?.Dispose();

                    NewRentalEstablishmentVm.InputFloor(TypedNumberOfFloors);
                    OwnedRentalEstablishments.Add(NewRentalEstablishmentVm.NewRentalEstablishment);
                    SelectedRentalEstablishmentType = null;
                    TypedNumberOfFloors = 0;
                    _addRentalEstablishmentWindow.Close();
                    MessageBox.Show("The Establishment has been added to the list.", "New Rental Establishment");

                }
                else
                {
                    MessageBox.Show("Kindly fill in all the fields.", "Unanswered Fields");
                }
            }
            else
            {
                MessageBox.Show("The typed number of floor is impossible.","Floors");
            }
        }

        private void OpenAddRentalWindowProc()
        {
            _addRentalEstablishmentWindow = new AddRentalEstablishmentWindow();
            _addRentalEstablishmentWindow.Owner = Application.Current.MainWindow;
            _addRentalEstablishmentWindow.ShowDialog();
        }
    }

    public enum RentalEstablishmentTypes
    {
        Dormitory,
        Apartment,
        BoardingHouse
    }

}