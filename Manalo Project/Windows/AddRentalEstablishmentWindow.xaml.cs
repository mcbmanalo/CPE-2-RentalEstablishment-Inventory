using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Manalo_Project
{
    /// <summary>
    /// Interaction logic for AddRentalEstablishmentWindow.xaml
    /// </summary>
    public partial class AddRentalEstablishmentWindow : Window
    {
        public AddRentalEstablishmentWindow()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
