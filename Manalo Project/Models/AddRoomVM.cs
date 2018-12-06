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
    public class AddRoomVM:ObservableObject
    {
        #region Fields

        private Room _newRoom;
        private Room _selectedRoom;

        #endregion

        #region Properties

        public Room NewRoom
        {
            get { return _newRoom; }
            set
            {
                _newRoom = value;
                RaisePropertyChanged(nameof(NewRoom));
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

        public ObservableCollection<Room> RoomsToBeAdded { get; } = new ObservableCollection<Room>();

        #endregion

        #region ICommands

        public ICommand RemoveSelectedRoomCommand => new RelayCommand(RemoveSelectedRoomProc, RemoveSelectedRoomCondition);

        private void RemoveSelectedRoomProc()
        {
            var selector = MessageBox.Show("Are you sure you want to remove this room?", "Remove",
                MessageBoxButton.YesNo);

            if (selector == MessageBoxResult.Yes)
            {
                RoomsToBeAdded.Remove(SelectedRoom);
                MessageBox.Show("The selected room has been removed.", "Remove");
            }
            else
            {
                MessageBox.Show("Operation has been cancelled.", "Cancel");
            }
        }

        private bool RemoveSelectedRoomCondition()
        {
            return SelectedRoom != null;
        }

        #endregion

        #region Methods and Booleans

        public string[] RoomTypeOptions => Enum.GetNames(typeof(RoomTypes));

        public void GetCapacity()
        {
            if (NewRoom.RoomType == RoomTypes.Single.ToString())
            {
                NewRoom.RoomCapacity = 1;
            }

            if (NewRoom.RoomType == RoomTypes.Double.ToString())
            {
                NewRoom.RoomCapacity = 2;
            }

            if (NewRoom.RoomType == RoomTypes.Triple.ToString())
            {
                NewRoom.RoomCapacity = 3;
            }

            if (NewRoom.RoomType == RoomTypes.Quad.ToString())
            {
                NewRoom.RoomCapacity = 4;
            }
        }

        #endregion
    }

    public enum RoomTypes
    {
        Single = 1,
        Double = 2,
        Triple = 3,
        Quad = 4
    }
}
