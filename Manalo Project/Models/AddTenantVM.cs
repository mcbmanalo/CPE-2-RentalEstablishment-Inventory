using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace Manalo_Project.Models
{
    public class AddTenantVM: ObservableObject
    {
        private Tenant _tenantVm;
        public string[] GenderOptions => Enum.GetNames(typeof(GenderType));
        public Tenant TenantVM
        {
            get { return _tenantVm; }
            set
            {
                _tenantVm = value; 
                RaisePropertyChanged(nameof(TenantVM));
            }
        }

        
    }
}
