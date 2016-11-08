using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace PayNote
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Savedps : Page
    {
        public Savedps()
        {
            this.InitializeComponent();
            checkFileContent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame.CanGoBack)
            {
                // Show UI in title bar if opted-in and in-app backstack is not empty.
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Visible;
                SystemNavigationManager.GetForCurrentView().BackRequested += Savedps_BackRequested;
            }
            else
            {
                // Remove the UI from the title bar if in-app back stack is empty.
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Collapsed;
            }
        }

        private void Savedps_BackRequested(object sender, BackRequestedEventArgs e)
        {
            //handle the back request and if there is a page to go back to allow it
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
                return;

            if (rootFrame.CanGoBack && e.Handled == false)
            {
                e.Handled = true;
                rootFrame.GoBack();
            }
        }

        private async void clearpayslips_Click(object sender, RoutedEventArgs e)
        {
            //get a link to the Current folder and file
            StorageFolder payslipFolder = ApplicationData.Current.LocalFolder;

            StorageFile payslips;
            try
            {
                //get the file
                payslips = await payslipFolder.GetFileAsync("payslips.txt");
            }
            catch (Exception E)
            {
                string message = E.Message;
                return;
            }
            //now put an empty string into the file to overwrite its contents to be empty
            string empty = "";
            await Windows.Storage.FileIO.WriteTextAsync(payslips, empty);
            //get the textblock text from resouces file and collapse clear button
            savedps.Text = "Cleared payslips";
            clearpayslips.Visibility = Visibility.Collapsed;
        }

        private async void checkFileContent()
        {
            //check if the textblock has any text if not clear button should not be shown
            if (savedps.Text.ToString() == "")
            {
                clearpayslips.Visibility = Visibility.Collapsed;
            }
            //get a link to current folder and read from locations.txt file
            StorageFolder payslipFolder = ApplicationData.Current.LocalFolder;

            StorageFile payslips;
            try
            {
                //get the file
                payslips = await payslipFolder.GetFileAsync("payslips.txt");
            }
            catch (Exception E)
            {
                string message = E.Message;
                return;
            }
            //read from the file
            string fileText = await Windows.Storage.FileIO.ReadTextAsync(payslips);
            //output file contents to textblock
            savedps.Text = savedps.Text + fileText;
            //if the textblock has text now show the clear button
            if (savedps.Text.ToString() != "")
            {
                clearpayslips.Visibility = Visibility.Visible;
            }
        }
    }
}
