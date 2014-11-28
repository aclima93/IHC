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
    /// Interaction logic for ChooseAvatar.xaml
    /// </summary>
    public partial class XAMLChooseAvatar : UserControl
    {

        public int selected_avatar = 1;
        private MainWindow mainWindow;

        public XAMLChooseAvatar()
        {
            InitializeComponent();
        }

        public void setMainWindow(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        public void setChooseAvatarHandlers()
        {
            // Choose Avatar Handlers
            Avatar1.Click += new RoutedEventHandler(Avatar1_Click);
            Avatar2.Click += new RoutedEventHandler(Avatar2_Click);
            Avatar3.Click += new RoutedEventHandler(Avatar3_Click);
            Avatar4.Click += new RoutedEventHandler(Avatar4_Click);
            Avatar5.Click += new RoutedEventHandler(Avatar5_Click);
            Avatar6.Click += new RoutedEventHandler(Avatar6_Click);
            Avatar7.Click += new RoutedEventHandler(Avatar7_Click);
            Avatar8.Click += new RoutedEventHandler(Avatar8_Click);
            Avatar9.Click += new RoutedEventHandler(Avatar9_Click);
            AvatarReturnToMainMenu.Click += new RoutedEventHandler(AvatarReturnToMainMenu_Click);
        }

        public void removeChooseAvatarHandlers()
        {
            // Choose Avatar Handlers
            Avatar1.Click -= new RoutedEventHandler(Avatar1_Click);
            Avatar2.Click -= new RoutedEventHandler(Avatar2_Click);
            Avatar3.Click -= new RoutedEventHandler(Avatar3_Click);
            Avatar4.Click -= new RoutedEventHandler(Avatar4_Click);
            Avatar5.Click -= new RoutedEventHandler(Avatar5_Click);
            Avatar6.Click -= new RoutedEventHandler(Avatar6_Click);
            Avatar7.Click -= new RoutedEventHandler(Avatar7_Click);
            Avatar8.Click -= new RoutedEventHandler(Avatar8_Click);
            Avatar9.Click -= new RoutedEventHandler(Avatar9_Click);
            AvatarReturnToMainMenu.Click -= new RoutedEventHandler(AvatarReturnToMainMenu_Click);
        }

        public void checkChooseAvatarButtons()
        {

            mainWindow.CheckButton(Avatar1, mainWindow.RightHand);
            mainWindow.CheckButton(Avatar2, mainWindow.RightHand);
            mainWindow.CheckButton(Avatar3, mainWindow.RightHand);
            mainWindow.CheckButton(Avatar4, mainWindow.RightHand);
            mainWindow.CheckButton(Avatar5, mainWindow.RightHand);
            mainWindow.CheckButton(Avatar6, mainWindow.RightHand);
            mainWindow.CheckButton(Avatar7, mainWindow.RightHand);
            mainWindow.CheckButton(Avatar8, mainWindow.RightHand);
            mainWindow.CheckButton(Avatar9, mainWindow.RightHand);
            mainWindow.CheckButton(AvatarReturnToMainMenu, mainWindow.RightHand);

        }


        void unselectAvatar(int index)
        {
            if (index == 1)
            {
                AvatarSelected1.Visibility = Visibility.Collapsed;
            }
            else if (index == 2)
            {
                AvatarSelected2.Visibility = Visibility.Collapsed;
            }
            else if (index == 3)
            {
                AvatarSelected3.Visibility = Visibility.Collapsed;
            }
            else if (index == 4)
            {
                AvatarSelected4.Visibility = Visibility.Collapsed;
            }
            else if (index == 5)
            {
                AvatarSelected5.Visibility = Visibility.Collapsed;
            }
            else if (index == 6)
            {
                AvatarSelected6.Visibility = Visibility.Collapsed;
            }
            else if (index == 7)
            {
                AvatarSelected7.Visibility = Visibility.Collapsed;
            }
            else if (index == 8)
            {
                AvatarSelected8.Visibility = Visibility.Collapsed;
            }
            else
            {
                AvatarSelected9.Visibility = Visibility.Collapsed;
            }
        }

        void selectAvatar(int index)
        {

            selected_avatar = index;
            AvatarImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/avatars/avatar_body_" + index + ".png"));

            if (index == 1)
            {
                AvatarSelected1.Visibility = Visibility.Visible;
            }
            else if (index == 2)
            {
                AvatarSelected2.Visibility = Visibility.Visible;
            }
            else if (index == 3)
            {
                AvatarSelected3.Visibility = Visibility.Visible;
            }
            else if (index == 4)
            {
                AvatarSelected4.Visibility = Visibility.Visible;
            }
            else if (index == 5)
            {
                AvatarSelected5.Visibility = Visibility.Visible;
            }
            else if (index == 6)
            {
                AvatarSelected6.Visibility = Visibility.Visible;
            }
            else if (index == 7)
            {
                AvatarSelected7.Visibility = Visibility.Visible;
            }
            else if (index == 8)
            {
                AvatarSelected8.Visibility = Visibility.Visible;
            }
            else
            {
                AvatarSelected9.Visibility = Visibility.Visible;
            }
        }


        // Avatar Button EventHandlers
        void Avatar1_Click(object sender, RoutedEventArgs e)
        {

            SoundPlayer correct = new SoundPlayer("selection-click.wav");
            correct.Play();

            unselectAvatar(selected_avatar);
            selectAvatar(1);

        }
        void Avatar2_Click(object sender, RoutedEventArgs e)
        {

            SoundPlayer correct = new SoundPlayer("selection-click.wav");
            correct.Play();

            unselectAvatar(selected_avatar);
            selectAvatar(2);

        }
        void Avatar3_Click(object sender, RoutedEventArgs e)
        {

            SoundPlayer correct = new SoundPlayer("selection-click.wav");
            correct.Play();

            unselectAvatar(selected_avatar);
            selectAvatar(3);

        }
        void Avatar4_Click(object sender, RoutedEventArgs e)
        {

            SoundPlayer correct = new SoundPlayer("selection-click.wav");
            correct.Play();

            unselectAvatar(selected_avatar);
            selectAvatar(4);

        }
        void Avatar5_Click(object sender, RoutedEventArgs e)
        {

            SoundPlayer correct = new SoundPlayer("selection-click.wav");
            correct.Play();

            unselectAvatar(selected_avatar);
            selectAvatar(5);

        }
        void Avatar6_Click(object sender, RoutedEventArgs e)
        {

            SoundPlayer correct = new SoundPlayer("selection-click.wav");
            correct.Play();

            unselectAvatar(selected_avatar);
            selectAvatar(6);

        }
        void Avatar7_Click(object sender, RoutedEventArgs e)
        {

            SoundPlayer correct = new SoundPlayer("selection-click.wav");
            correct.Play();

            unselectAvatar(selected_avatar);
            selectAvatar(7);

        }
        void Avatar8_Click(object sender, RoutedEventArgs e)
        {

            SoundPlayer correct = new SoundPlayer("selection-click.wav");
            correct.Play();

            unselectAvatar(selected_avatar);
            selectAvatar(8);

        }
        void Avatar9_Click(object sender, RoutedEventArgs e)
        {

            SoundPlayer correct = new SoundPlayer("selection-click.wav");
            correct.Play();

            unselectAvatar(selected_avatar);
            selectAvatar(9);

        }
        void AvatarReturnToMainMenu_Click(object sender, RoutedEventArgs e)
        {

            SoundPlayer correct = new SoundPlayer("swoosh.wav");
            correct.Play();

            mainWindow.changeGameState(MainWindow.game_states_t.MAIN_MENU, ChooseAvatar, mainWindow.xamlMainMenu.MainMenu);

        }

    }
}
