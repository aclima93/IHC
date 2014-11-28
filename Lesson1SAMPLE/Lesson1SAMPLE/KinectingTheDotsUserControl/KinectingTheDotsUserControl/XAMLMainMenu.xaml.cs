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
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class XAMLMainMenu : UserControl
    {

        private MainWindow mainWindow;

        public XAMLMainMenu()
        {
            InitializeComponent();
        }

        public void setMainWindow(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        public void checkMainMenuButtons()
        {
            mainWindow.CheckButton(MainMenuItem1, mainWindow.RightHand);
            mainWindow.CheckButton(MainMenuItem2, mainWindow.RightHand);
            mainWindow.CheckButton(MainMenuItem3, mainWindow.RightHand);
            mainWindow.CheckButton(MainMenuItem4, mainWindow.RightHand);
        }

        public void setMainMenuHandlers()
        {
            // Main Memu Handlers
            MainMenuItem1.Click += new RoutedEventHandler(MainMenuItem1_Click);
            MainMenuItem2.Click += new RoutedEventHandler(MainMenuItem2_Click);
            MainMenuItem3.Click += new RoutedEventHandler(MainMenuItem3_Click);
            MainMenuItem4.Click += new RoutedEventHandler(MainMenuItem4_Click);
        }
        public void removeMainMenuHandlers()
        {
            // Main Memu Handlers
            MainMenuItem1.Click -= new RoutedEventHandler(MainMenuItem1_Click);
            MainMenuItem2.Click -= new RoutedEventHandler(MainMenuItem2_Click);
            MainMenuItem3.Click -= new RoutedEventHandler(MainMenuItem3_Click);
            MainMenuItem4.Click -= new RoutedEventHandler(MainMenuItem4_Click);
        }


        // Main Menu Button EventHandlers
        private void MainMenuItem1_Click(object sender, RoutedEventArgs e)
        {

            SoundPlayer correct = new SoundPlayer("swoosh.wav");
            correct.Play();

            mainWindow.changeGameState(MainWindow.game_states_t.PLAY, MainMenu, mainWindow.Play);

        }
        private void MainMenuItem2_Click(object sender, RoutedEventArgs e)
        {

            SoundPlayer correct = new SoundPlayer("swoosh.wav");
            correct.Play();

            mainWindow.changeGameState(MainWindow.game_states_t.PRACTICE, MainMenu, mainWindow.Practice);

        }
        private void MainMenuItem3_Click(object sender, RoutedEventArgs e)
        {

            SoundPlayer correct = new SoundPlayer("swoosh.wav");
            correct.Play();

            mainWindow.changeGameState(MainWindow.game_states_t.CHOOSE_AVATAR, MainMenu, mainWindow.xamlChooseAvatar.ChooseAvatar);

        }
        private void MainMenuItem4_Click(object sender, RoutedEventArgs e)
        {

            SoundPlayer correct = new SoundPlayer("swoosh.wav");
            correct.Play();

            mainWindow.changeGameState(MainWindow.game_states_t.NEW_SAVE_LOAD, MainMenu, mainWindow.xamlNewSaveLoad.NewSaveLoad);
        }


    }
}
