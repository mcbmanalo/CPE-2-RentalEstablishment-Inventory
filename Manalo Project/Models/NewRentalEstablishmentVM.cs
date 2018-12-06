using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace Manalo_Project.Models
{
    public class NewRentalEstablishmentVM: ObservableObject
    {
        private Rental_Establishments _newRentalEstablishment;

        public Rental_Establishments NewRentalEstablishment
        {
            get { return _newRentalEstablishment; }
            set
            {
                _newRentalEstablishment = value; 
                RaisePropertyChanged(nameof(NewRentalEstablishment));
            }
        }

        public void InputFloor(int numberoffloors)
        {
            for (int i = 1; i <= numberoffloors; i++)
            {
                NewRentalEstablishment.Floors.Add(new Floor(i));
            }
        }
    }
}
