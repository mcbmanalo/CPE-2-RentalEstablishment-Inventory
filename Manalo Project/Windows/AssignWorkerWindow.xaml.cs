﻿using System;
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

namespace Manalo_Project.Windows
{
    /// <summary>
    /// Interaction logic for AssignWorkerWindow.xaml
    /// </summary>
    public partial class AssignWorkerWindow : Window
    {
        public AssignWorkerWindow()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var selector = MessageBox.Show("Are you sure you want to cancel operation?", "Cancel",
                MessageBoxButton.YesNo);
            if (selector == MessageBoxResult.Yes)
            {
                MessageBox.Show("Operation has been cancelled.", "Cancel");
                Close();
            }
        }
    }
}
