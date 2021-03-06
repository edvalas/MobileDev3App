﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Popups;
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
    public sealed partial class Payslip : Page
    {
        const double payeCharge = 0.2;
        const double prsi = 0.04;
        const double usc = 0.02;
        String empname, payeename, payeeaddress, date;
        double hours, rate, wage, netpay;
        double afterPaye, afterPrsi;
        double Paye, Prsi, Usc;

        public Payslip()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame.CanGoBack)
            {
                // Show UI in title bar if opted-in and in-app backstack is not empty.
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Visible;
                SystemNavigationManager.GetForCurrentView().BackRequested += Payslip_BackRequested;
            }
            else
            {
                // Remove the UI from the title bar if in-app back stack is empty.
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Collapsed;
            }
        }

        private void Payslip_BackRequested(object sender, BackRequestedEventArgs e)
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

        private async void calcPay_Click(object sender, RoutedEventArgs e)
        {
            int validCount = 0; // counter to check if all 6 inputs are entered

            if (EmpName.Text.ToString() != "")
            {
                empname = EmpName.Text.ToString();
                validCount++;
            }

            if (PayeeName.Text.ToString() != "")
            {
                payeename = PayeeName.Text.ToString();
                validCount++;
            }

            if (PayeeAddress.Text.ToString() != "")
            {
                payeeaddress = PayeeAddress.Text.ToString();
                validCount++;
            }

            if (Date.Text.ToString() != "")
            {
                date = Date.Text.ToString();
                validCount++;
            }

            if (Rate.Text.ToString() != "")
            {
                try
                {
                    rate = Convert.ToDouble(Rate.Text);
                    validCount++;
                }
                catch
                {
                    MessageDialog Dialog = new MessageDialog("Input must be numeric");
                    await Dialog.ShowAsync();
                }
                
            }

            if (Hours.Text.ToString() != "")
            {
                try
                {
                    hours = Convert.ToDouble(Hours.Text.ToString());
                    validCount++;
                }
                catch
                {
                    MessageDialog Dialog = new MessageDialog("Input must be numeric");
                    await Dialog.ShowAsync();
                }
                
            }

            if (validCount != 6)
            {
                //if count is not 6 then some input is missing throw dialog
                MessageDialog Dialog = new MessageDialog("Input all required fields");
                await Dialog.ShowAsync();
            }
            else
            {
                //call calc method
                calculate();
            }
        }

        private void calculate()
        {
            //basic calcs
            wage = rate * hours;

            //deductions
            Paye = wage * payeCharge;
            afterPaye = wage - Paye;

            Prsi = afterPaye * prsi;
            afterPrsi = afterPaye - Prsi;

            Usc = afterPrsi * usc;
            netpay = afterPrsi - Usc;

            //display method call
            outputShow();
        }

        private void outputShow()
        {
            //make textblocks visible
            outempName.Visibility = Visibility.Visible;
            outpayeeName.Visibility = Visibility.Visible;
            outDate.Visibility = Visibility.Visible;
            outAddress.Visibility = Visibility.Visible;
            outWage.Visibility = Visibility.Visible;
            outPaye.Visibility = Visibility.Visible;
            outPrsi.Visibility = Visibility.Visible;
            outUsc.Visibility = Visibility.Visible;
            outNet.Visibility = Visibility.Visible;

            savepayslip.Visibility = Visibility.Visible;
            
            //output text to textblocks
            outempName.Text = (Localization.Get("outempName")).ToString() + empname.ToString();
            outpayeeName.Text = (Localization.Get("outpayeeName")) + payeename.ToString();
            outDate.Text = (Localization.Get("outDate")) + date.ToString();
            outAddress.Text = (Localization.Get("outAddress")) + payeeaddress.ToString();
            outWage.Text = (Localization.Get("outWage")) + wage.ToString("$0.00");
            outPaye.Text = (Localization.Get("outPaye")) + Paye.ToString("$0.00");
            outPrsi.Text = (Localization.Get("outPrsi")) + Prsi.ToString("$0.00");
            outUsc.Text = (Localization.Get("outUsc")) + Usc.ToString("$0.00");
            outNet.Text = (Localization.Get("outNet")) + netpay.ToString("$0.00");
        }

        private async void savepayslip_Click(object sender, RoutedEventArgs e)
        {
            //check if there is text in the textblock before you save empty string to a file
            if (outNet.Text.ToString() == "")
            {
                //display error 
                await new MessageDialog("No payslips calculated").ShowAsync();
            }
            else
            {
                //get a link to current folder
                StorageFolder payslipFolder = ApplicationData.Current.LocalFolder;
                //file
                StorageFile payslips;
                //string to contain current file contents
                string fileText = "";

                try
                {
                    //get the file and its contents to a string
                    payslips = await payslipFolder.GetFileAsync("payslips.txt");
                    fileText = await Windows.Storage.FileIO.ReadTextAsync(payslips);
                }
                catch (Exception E)
                {
                    //if its not there create a new file
                    string message = E.Message;
                    payslips = await payslipFolder.CreateFileAsync("payslips.txt");
                }
                //string to add on to the file contents
                string output;
                // which is equal to the text of the calculated payslip
                output = outempName.Text.ToString() + " \n" + outDate.Text.ToString() + " \n" + outpayeeName.Text.ToString() + ",  " + outAddress.Text.ToString() +
                    " \n" + outWage.Text.ToString() + " \n" + outPaye.Text.ToString() + " \n" + outPrsi.Text.ToString() + " \n" + outUsc.Text.ToString() +
                    " \n" + outNet.Text.ToString();
                //write back to the file its original contents plus the output of payslip calculation
                await Windows.Storage.FileIO.WriteTextAsync(payslips, fileText + output + System.Environment.NewLine + System.Environment.NewLine);
                //msg dialog
                await new MessageDialog("saved to file").ShowAsync();
            }
        }
    }
}
