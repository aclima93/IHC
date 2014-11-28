using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Media;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using Coding4Fun.Kinect.Wpf.Controls;
using Microsoft.Research.Kinect.Nui;

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;

using System.Xml;
using System.Windows.Markup;
using System.IO;
using System.Text;

using Microsoft.Win32;

namespace KinectingTheDotsUserControl
{
    /// <summary>
    /// Interaction logic for NewSaveLoad.xaml
    /// </summary>
    public partial class XAMLNewSaveLoad : UserControl
    {

        private MainWindow mainWindow;

        public XAMLNewSaveLoad(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            InitializeComponent();
        }

        private void openFileDialog()
        {
            // Create an instance of the open file dialog box.
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            // Set filter options and filter index.
            openFileDialog1.Filter = "Text Files (.txt)|*.txt|All Files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.Multiselect = false;
            //openFileDialog1.InitialDirectory = @"pack://application:,,,/Resources/game_files";
            openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);


            // Call the ShowDialog method to show the dialog box.
            bool? userClickedOK = openFileDialog1.ShowDialog();

            // Process input if the user clicked OK.
            if (userClickedOK == true)
            {
                /*
                // Open the selected file to read.
                System.IO.Stream fileStream = openFileDialog1.File.OpenRead();
                using (System.IO.StreamReader reader = new System.IO.StreamReader(fileStream))
                */
                using (System.IO.StreamReader reader = new System.IO.StreamReader(openFileDialog1.FileName))
                {
                    // Read lines into a string array
                    mainWindow.game_file = reader.ReadToEnd().Split('\n');
                }
                //fileStream.Close();
            }
        }

        public void checkNewSaveLoadButtons()
        {
            mainWindow.CheckButton(NewSaveLoadItem1, mainWindow.RightHand);
            mainWindow.CheckButton(NewSaveLoadItem2, mainWindow.RightHand);
            mainWindow.CheckButton(NewSaveLoadItem3, mainWindow.RightHand);
            mainWindow.CheckButton(NewSaveLoadItem4, mainWindow.RightHand);
        }

        public void setNewSaveLoadHandlers()
        {
            // New Save Load Handlers
            NewSaveLoadItem1.Click += new RoutedEventHandler(NewSaveLoadItem1_Click);
            NewSaveLoadItem2.Click += new RoutedEventHandler(NewSaveLoadItem2_Click);
            NewSaveLoadItem3.Click += new RoutedEventHandler(NewSaveLoadItem3_Click);
            NewSaveLoadItem4.Click += new RoutedEventHandler(NewSaveLoadItem4_Click);
        }

        public void removeNewSaveLoadHandlers()
        {
            // New Save Load Handlers
            NewSaveLoadItem1.Click -= new RoutedEventHandler(NewSaveLoadItem1_Click);
            NewSaveLoadItem2.Click -= new RoutedEventHandler(NewSaveLoadItem2_Click);
            NewSaveLoadItem3.Click -= new RoutedEventHandler(NewSaveLoadItem3_Click);
            NewSaveLoadItem4.Click -= new RoutedEventHandler(NewSaveLoadItem4_Click);
        }

        private void NewSaveLoadItem1_Click(object sender, RoutedEventArgs e)
        {
            // create something with the file browsing thingamajiggy
        }
        private void NewSaveLoadItem2_Click(object sender, RoutedEventArgs e)
        {
            // save something with the file browsing thingamajiggy
        }
        private void NewSaveLoadItem3_Click(object sender, RoutedEventArgs e)
        {
            // select the file to load
            openFileDialog();
        }

        private void NewSaveLoadItem4_Click(object sender, RoutedEventArgs e)
        {

            SoundPlayer correct = new SoundPlayer("swoosh.wav");
            correct.Play();

            mainWindow.changeGameState(MainWindow.game_states_t.MAIN_MENU, NewSaveLoad, mainWindow.xamlMainMenu);

        }

    }
}
