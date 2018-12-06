using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace Manalo_Project
{
    public class Person:ObservableObject
    {
        #region Fields

        private string _firstName;
        private string _middleName;
        private string _lastName;
        private int _age;
        private GenderType _gender;
        private long _contactNumber;
        private string _occupation;
        private bool _selectedToPay;

        #endregion

        #region Properties

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                RaisePropertyChanged(nameof(FirstName));
            }
        }

        public string MiddleName
        {
            get { return _middleName; }
            set
            {
                _middleName = value;
                RaisePropertyChanged(nameof(MiddleName));
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                RaisePropertyChanged(nameof(LastName));
            }
        }

        public int Age
        {
            get { return _age; }
            set
            {
                _age = value;
                RaisePropertyChanged(nameof(Age));
            }
        }

        public GenderType Gender
        {
            get { return _gender; }
            set
            {
                _gender = value;
                RaisePropertyChanged(nameof(Gender));
            }
        }

        public long ContactNumber
        {
            get { return _contactNumber; }
            set
            {
                _contactNumber = value;
                RaisePropertyChanged(nameof(ContactNumber));
            }
        }

        public string Occupation
        {
            get { return _occupation; }
            set
            {
                _occupation = value;
                RaisePropertyChanged(nameof(Occupation));
            }
        }

        public bool SelectedToPay
        {
            get { return _selectedToPay; }
            set
            {
                _selectedToPay = value;
                RaisePropertyChanged(nameof(SelectedToPay));
            }
        }

        #endregion
    }

    public enum GenderType
    {
        Male,
        Female
    }

    public class Worker : Person
    {
        #region Fields

        private string _job;
        private long _workerId;
        private Floor _assignedFloor;

        #endregion

        #region Properties

        public long WorkerID
        {
            get { return _workerId; }
            set
            {
                _workerId = value;
                RaisePropertyChanged(nameof(WorkerID));
            }
        }

        public string Job
        {
            get { return _job; }
            set
            {
                _job = value;
                RaisePropertyChanged(nameof(Job));
            }
        }

        public Floor AssignedFloor
        {
            get { return _assignedFloor; }
            set
            {
                _assignedFloor = value;
                RaisePropertyChanged(nameof(AssignedFloor));
            }
        }

        #endregion
    }

    public class Tenant : Person
    {
        #region Fields

        private Person _fatherOfTenant;
        private Person _motherOfTenant;
        private Room _occupiedRoom;
        private float _balance;
        private long _tenantId;

        #endregion

        #region Properties

        public long TenantID
        {
            get { return _tenantId; }
            set
            {
                _tenantId = value;
                RaisePropertyChanged(nameof(TenantID));
            }
        }

        public float Balance
        {
            get { return _balance; }
            set
            {
                _balance = value;
                RaisePropertyChanged(nameof(Balance));
            }
        }

        public Room OccupiedRoom
        {
            get { return _occupiedRoom; }
            set
            {
                _occupiedRoom = value;
                RaisePropertyChanged(nameof(OccupiedRoom));
            }
        }

        public Person FatherOfTenant
        {
            get { return _fatherOfTenant; }
            set
            {
                _fatherOfTenant = value;
                RaisePropertyChanged(nameof(FatherOfTenant));
            }
        }

        public Person MotherOfTenant
        {
            get { return _motherOfTenant; }
            set
            {
                _motherOfTenant = value;
                RaisePropertyChanged(nameof(MotherOfTenant));
            }
        }

        #endregion
    }
}
