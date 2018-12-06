using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace Manalo_Project.Models
{
    public class RoomHistory: ObservableObject
    {
        private DateTime _dateTimeLeft;
        private DateTime _dateTimeEntered;
        private Tenant _exTenant;

        public Tenant ExTenant
        {
            get { return _exTenant; }
            set
            {
                _exTenant = value;
                RaisePropertyChanged(nameof(ExTenant));
            }
        }

        public DateTime DateTimeEntered
        {
            get { return _dateTimeEntered; }
            set
            {
                _dateTimeEntered = value; 
                RaisePropertyChanged(nameof(DateTimeEntered));
            }
        }

        public DateTime DateTimeLeft
        {
            get { return _dateTimeLeft; }
            set
            {
                _dateTimeLeft = value; 
                RaisePropertyChanged(nameof(DateTimeLeft));
            }
        }
    }
}
