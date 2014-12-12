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
    /// Interaction logic for XAMLPlay.xaml
    /// </summary>
    public partial class XAMLPlay : UserControl
    {

        private MainWindow mainWindow;

        public XAMLPlay()
        {
            InitializeComponent();

        }


        public void setMainWindow(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        public void setPlayHandlers()
        {
            // Choose Avatar Handlers
            PlayReturnToMainMenuL1.Click += new RoutedEventHandler(PlayReturnToMainMenuL1_Click);
            PlayReturnToMainMenuR1.Click += new RoutedEventHandler(PlayReturnToMainMenuR1_Click);
            PlayReturnToMainMenuL2.Click += new RoutedEventHandler(PlayReturnToMainMenuL2_Click);
            PlayReturnToMainMenuR2.Click += new RoutedEventHandler(PlayReturnToMainMenuR2_Click);
        }

        public void removePlayHandlers()
        {
            // Choose Avatar Handlers
            PlayReturnToMainMenuL1.Click -= new RoutedEventHandler(PlayReturnToMainMenuL1_Click);
            PlayReturnToMainMenuR1.Click -= new RoutedEventHandler(PlayReturnToMainMenuR1_Click);
            PlayReturnToMainMenuL2.Click -= new RoutedEventHandler(PlayReturnToMainMenuL2_Click);
            PlayReturnToMainMenuR2.Click -= new RoutedEventHandler(PlayReturnToMainMenuR2_Click);
        }

        public void checkPlayButtons()
        {

            // player1's hands
            mainWindow.CheckButton(PlayReturnToMainMenuL1, mainWindow.P1J7);
            mainWindow.CheckButton(PlayReturnToMainMenuR1, mainWindow.P1J8);

            // player2's hands
            mainWindow.CheckButton(PlayReturnToMainMenuL2, mainWindow.P2J7);
            mainWindow.CheckButton(PlayReturnToMainMenuR2, mainWindow.P2J8);

        }

        void PlayReturnToMainMenuL1_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.changeGameState(MainWindow.game_states_t.MAIN_MENU, mainWindow.xamlPlay, mainWindow.xamlMainMenu);
        }
        void PlayReturnToMainMenuR1_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.changeGameState(MainWindow.game_states_t.MAIN_MENU, mainWindow.xamlPlay, mainWindow.xamlMainMenu);
        }
        void PlayReturnToMainMenuL2_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.changeGameState(MainWindow.game_states_t.MAIN_MENU, mainWindow.xamlPlay, mainWindow.xamlMainMenu);
        }
        void PlayReturnToMainMenuR2_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.changeGameState(MainWindow.game_states_t.MAIN_MENU, mainWindow.xamlPlay, mainWindow.xamlMainMenu);
        }
    }
}
