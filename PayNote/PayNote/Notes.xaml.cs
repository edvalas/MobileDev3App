using System;
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
    public sealed partial class Notes : Page
    {
        public Notes()
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
                SystemNavigationManager.GetForCurrentView().BackRequested += Notes_BackRequested;
            }
            else
            {
                // Remove the UI from the title bar if in-app back stack is empty.
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Collapsed;
            }
        }

        private void Notes_BackRequested(object sender, BackRequestedEventArgs e)
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

        private async void saveNotes_Click(object sender, RoutedEventArgs e)
        {
            //get a link to current folder
            StorageFolder notesFolder = ApplicationData.Current.LocalFolder;
            //file
            StorageFile notes;

            try
            {
                //get the file
                notes = await notesFolder.GetFileAsync("notes.txt");
            }
            catch (Exception E)
            {
                //if its not there create a new file
                notes = await notesFolder.CreateFileAsync("notes.txt");
            }
            //string to add to the file
            string output;
            // which is equal to the text of the notes textbox
            output = notesText.Text.ToString();
            //write to the file contents of the textbox
            await Windows.Storage.FileIO.WriteTextAsync(notes, output + " ");
            //msg dialog
            await new MessageDialog("Notes Saved").ShowAsync();
        }

        private async void checkFileContent()
        {
            //get a link to current folder
            StorageFolder notesFolder = ApplicationData.Current.LocalFolder;
            //file
            StorageFile notes;
            try
            {
                //get the file
                notes = await notesFolder.GetFileAsync("notes.txt");
            }
            catch (Exception E)
            {
                string message = E.Message;
                return;
            }
            //read from the file
            string fileText = await Windows.Storage.FileIO.ReadTextAsync(notes);
            //output file contents to textbox
            notesText.Text = fileText;
        }
    }
}
