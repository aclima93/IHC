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
//using Microsoft.Kinect;

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
    /// Interaction logic for XAMLPractice.xaml
    /// </summary>
    public partial class XAMLPractice : UserControl
    {

        private MainWindow mainWindow;

        public XAMLPractice()
        {
            InitializeComponent();

        }


        public void setMainWindow(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        public void setPracticeHandlers()
        {
            mainWindow.player1_score = 0;

            // Choose Avatar Handlers
            PracticeReturnToMainMenu.Click += new RoutedEventHandler(PracticeReturnToMainMenu_Click);
        }

        public void removePracticeHandlers()
        {
            // Choose Avatar Handlers
            PracticeReturnToMainMenu.Click -= new RoutedEventHandler(PracticeReturnToMainMenu_Click);
        }

        public void checkPracticeButtons()
        {

            // player1's hand
            mainWindow.CheckButton(PracticeReturnToMainMenu, mainWindow.P1J7);

        }

        void PracticeReturnToMainMenu_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.changeGameState(MainWindow.game_states_t.MAIN_MENU, mainWindow.xamlPractice, mainWindow.xamlMainMenu);

        }
    }
}
