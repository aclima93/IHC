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


namespace KinectingTheDotsUserControl
{
    /// <summary>
    /// Interaction logic for Page2.xaml
    /// </summary>
    public partial class Page2 : Page
    {
        
        public enum game_states_t { MAIN_MENU, PLAY, PRACTICE, CHOOSE_AVATAR, NEW_SAVE_LOAD, GAME_ON };
        
        public Page2()
        {
            InitializeComponent();
        }

        public void setEventHandlers()
        {
            MainMenuItem1.Click += new RoutedEventHandler(MainMenuItem1_Click);
            MainMenuItem2.Click += new RoutedEventHandler(MainMenuItem2_Click);

            MainMenuItem3.Click += new RoutedEventHandler(MainMenuItem3_Click);
            MainMenuItem4.Click += new RoutedEventHandler(MainMenuItem4_Click);
        }

        int MainMenuItem1_Click(object sender, RoutedEventArgs e)
        {

            SoundPlayer correct = new SoundPlayer("swoosh.wav");
            correct.Play();

            return (int)game_states_t.PLAY;

        }
        void MainMenuItem2_Click(object sender, RoutedEventArgs e)
        {

            SoundPlayer correct = new SoundPlayer("swoosh.wav");
            correct.Play();

            game_state = game_states_t.PRACTICE;

        }
        void MainMenuItem3_Click(object sender, RoutedEventArgs e)
        {

            SoundPlayer correct = new SoundPlayer("swoosh.wav");
            correct.Play();

            game_state = game_states_t.CHOOSE_AVATAR;

        }
        void MainMenuItem4_Click(object sender, RoutedEventArgs e)
        {

            SoundPlayer correct = new SoundPlayer("swoosh.wav");
            correct.Play();

            game_state = game_states_t.NEW_SAVE_LOAD;
        }
    }
}
