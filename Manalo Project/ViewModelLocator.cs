using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Manalo_Project.Modules;

namespace Manalo_Project
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            RentalEstablishmentModule = new RentalEstablishmentModule();
        }

        public RentalEstablishmentModule RentalEstablishmentModule { get; }
    }
}