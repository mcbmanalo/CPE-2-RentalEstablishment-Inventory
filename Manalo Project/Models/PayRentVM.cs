using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace Manalo_Project.Models
{
    public class PayRentVM : ObservableObject
    {
        #region Fields

        private Tenant _dummyTenant;
        private TenantPayment _newTenantPayment;
        private TenantPayment _dummyTenantPayment;

        #endregion

        #region Properties

        public Tenant DummyTenant
        {
            get { return _dummyTenant; }
            set
            {
                _dummyTenant = value;
                RaisePropertyChanged(nameof(DummyTenant));
            }
        }

        public TenantPayment DummyTenantPayment
        {
            get { return _dummyTenantPayment; }
            set
            {
                _dummyTenantPayment = value;
                RaisePropertyChanged(nameof(DummyTenantPayment));
            }
        }

        public TenantPayment NewTenantPayment
        {
            get { return _newTenantPayment; }
            set
            {
                _newTenantPayment = value;
                RaisePropertyChanged(nameof(NewTenantPayment));
            }
        }

        public ObservableCollection<Tenant> DummyTenantList { get; } = new ObservableCollection<Tenant>();
        public ObservableCollection<TenantPayment> DummyTenantPaymentsList { get; set; } = new ObservableCollection<TenantPayment>();

        #endregion

        #region Constructor

        public PayRentVM(ObservableCollection<Tenant> tenantcollection)
        {
            foreach (var tenant in tenantcollection)
            {
                DummyTenant = new Tenant();
                DummyTenant.TenantID = tenant.TenantID;
                DummyTenant.FirstName = tenant.FirstName;
                DummyTenant.MiddleName = tenant.MiddleName;
                DummyTenant.LastName = tenant.LastName;
                DummyTenant.Balance = tenant.Balance;
                DummyTenantList.Add(DummyTenant);
            }
        }

        #endregion

        #region Methods

        public string[] PaymentOptions => Enum.GetNames(typeof(PaymentTypes));

        public void TakeTenantForPay()
        {
            DummyTenantPaymentsList = new ObservableCollection<TenantPayment>();

            foreach (var dummy in DummyTenantList)
            {
                if (dummy.SelectedToPay == true)
                {
                    DummyTenantPayment = new TenantPayment();
                    DummyTenantPayment.TenantToPay = dummy;
                    DummyTenantPayment.PaymentType = NewTenantPayment.PaymentType;
                    if (NewTenantPayment.PaymentType == "Full")
                    {
                        DummyTenantPayment.AmountToPay = dummy.Balance;
                        DummyTenantPaymentsList.Add(DummyTenantPayment);

                    }
                    if (NewTenantPayment.PaymentType == "Partial")
                    {
                        DummyTenantPayment.AmountToPay = NewTenantPayment.AmountToPay;
                        DummyTenantPaymentsList.Add(DummyTenantPayment);

                        //if (NewTenantPayment.AmountToPay != 0)
                        //{
                        //}
                    }
                }

            }
        }

        #endregion

        #region Commented Command
        //public ObservableCollection<TenantPayment> TenantPaymentsList { get; } = new ObservableCollection<TenantPayment>();


        //private void FilterTenants(Tenant value)
        //{
        //    _tenantlistforpaymentview.Filter = FiltertText;
        //}

        //private void CheckTenantPayments()
        //{
        //    foreach (var o in _tenantlistforpaymentview)
        //    {
        //        foreach (var tenantpayment in TenantPaymentsList)
        //        {
        //            var dummytenant = o as TenantPayment;
        //            if (tenantpayment.TenantToPay.FirstName.ToLowerInvariant() != tenantpayment.TenantToPay.FirstName.ToLowerInvariant() ||
        //                   tenantpayment.TenantToPay.MiddleName.ToLowerInvariant() != tenantpayment.TenantToPay.MiddleName.ToLowerInvariant() ||
        //                   tenantpayment.TenantToPay.LastName.ToLowerInvariant() != tenantpayment.TenantToPay.LastName.ToLowerInvariant() ||
        //                   tenantpayment.TenantToPay.TenantID.ToString() != tenantpayment.TenantToPay.TenantID.ToString().ToLowerInvariant())
        //            {
        //                return;
        //            }
        //            else
        //            {
        //                continue;
        //            }

        //        }
        //    }
        //}

        //private bool FiltertText(object o)
        //{

        //    var tenant = o as Tenant;
        //    if (tenant == null) return false;
        //    return tenant.FirstName.ToLowerInvariant() != NewTenantPayment.TenantToPay.FirstName.ToLowerInvariant() ||
        //           tenant.MiddleName.ToLowerInvariant() != NewTenantPayment.TenantToPay.MiddleName.ToLowerInvariant() ||
        //           tenant.LastName.ToLowerInvariant() != NewTenantPayment.TenantToPay.LastName.ToLowerInvariant() ||
        //           tenant.TenantID.ToString() != NewTenantPayment.TenantToPay.TenantID.ToString().ToLowerInvariant();
        //}

        //public ICommand AddToTenantPaymentListCommand => new RelayCommand(AddToTenantPaymentListProc);

        //private void AddToTenantPaymentListProc()
        //{
        //    if (NewTenantPayment.TenantToPay != null && NewTenantPayment.PaymentType != null)
        //    {
        //        if (NewTenantPayment.PaymentType == "Full")
        //        {
        //            DummyTenantPayment = new TenantPayment();
        //            DummyTenantPayment.TenantToPay = new Tenant();
        //            DummyTenantPayment.TenantToPay.TenantID = NewTenantPayment.TenantToPay.TenantID;
        //            DummyTenantPayment.TenantToPay.FirstName = NewTenantPayment.TenantToPay.FirstName;
        //            DummyTenantPayment.TenantToPay.MiddleName = NewTenantPayment.TenantToPay.MiddleName;
        //            DummyTenantPayment.TenantToPay.LastName = NewTenantPayment.TenantToPay.LastName;
        //            DummyTenantPayment.PaymentType = NewTenantPayment.PaymentType;
        //            DummyTenantPayment.AmountToPay = NewTenantPayment.TenantToPay.Balance;

        //            NewTenantPayment.AmountToPay = NewTenantPayment.TenantToPay.Balance;
        //            TenantPaymentsList.Add(DummyTenantPayment);

        //            FilterTenants(NewTenantPayment.TenantToPay);
        //            MessageBox.Show("The tenat has been added to list for payment.", "Add Tenant");
        //            NewTenantPayment = new TenantPayment();
        //        }

        //        else
        //        {
        //            if (NewTenantPayment.AmountToPay == 0)
        //            {
        //                MessageBox.Show("Please input the partial payment.", "Unanswered Field");
        //            }
        //            else
        //            {
        //                TenantPaymentsList.Add(NewTenantPayment);
        //                MessageBox.Show("The tenat has been added to list for payment.", "Add Tenant");
        //                NewTenantPayment = new TenantPayment();
        //            }
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Kindly fill in all the fields.", "Unanswered Fields");
        //    }   
        //}

        #endregion
    }

    public class TenantPayment : ObservableObject
    {
        #region Fields

        private Tenant _tenantToPay;
        private float _amountToPay;
        private string _paymentType;

        #endregion

        #region Properties

        public Tenant TenantToPay
        {
            get { return _tenantToPay; }
            set
            {
                _tenantToPay = value;
                RaisePropertyChanged(nameof(TenantToPay));
            }
        }

        public string PaymentType
        {
            get { return _paymentType; }
            set
            {
                _paymentType = value;
                RaisePropertyChanged(nameof(PaymentType));
            }
        }

        public float AmountToPay
        {
            get { return _amountToPay; }
            set
            {
                _amountToPay = value;
                RaisePropertyChanged(nameof(AmountToPay));
            }
        }

        #endregion
    }

    public enum PaymentTypes
    {
        Full,
        Partial
    }
}

