using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PayNote
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //collapse the back button on this main page.
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Collapsed;

            base.OnNavigatedTo(e);
        }

        private void calcPay_Click(object sender, RoutedEventArgs e)
        {
            //navigation on button click
            Frame.Navigate(typeof(Payslip));
        }

        private void SavedPayslips_Click(object sender, RoutedEventArgs e)
        {
            //navigation on button click
            Frame.Navigate(typeof(Savedps));
        }

        private void Notes_Click(object sender, RoutedEventArgs e)
        {
            //navigation on button click
            Frame.Navigate(typeof(Notes));
        }
    }
}
