﻿using System;
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
    /// Interaction logic for ChooseAvatar.xaml
    /// </summary>
    public partial class XAMLChooseAvatar : UserControl
    {

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

            selectAvatarP1(mainWindow.selected_avatarP1);
            selectAvatarP2(mainWindow.selected_avatarP2);

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

            Avatar_1.Click += new RoutedEventHandler(Avatar_1_Click);
            Avatar_2.Click += new RoutedEventHandler(Avatar_2_Click);
            Avatar_3.Click += new RoutedEventHandler(Avatar_3_Click);
            Avatar_4.Click += new RoutedEventHandler(Avatar_4_Click);
            Avatar_5.Click += new RoutedEventHandler(Avatar_5_Click);
            Avatar_6.Click += new RoutedEventHandler(Avatar_6_Click);
            Avatar_7.Click += new RoutedEventHandler(Avatar_7_Click);
            Avatar_8.Click += new RoutedEventHandler(Avatar_8_Click);
            Avatar_9.Click += new RoutedEventHandler(Avatar_9_Click);

            AvatarReturnToMainMenu.Click += new RoutedEventHandler(AvatarReturnToMainMenu_Click);
            AvatarReturnToMainMenuP2.Click += new RoutedEventHandler(AvatarReturnToMainMenuP2_Click);
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

            Avatar_1.Click -= new RoutedEventHandler(Avatar_1_Click);
            Avatar_2.Click -= new RoutedEventHandler(Avatar_2_Click);
            Avatar_3.Click -= new RoutedEventHandler(Avatar_3_Click);
            Avatar_4.Click -= new RoutedEventHandler(Avatar_4_Click);
            Avatar_5.Click -= new RoutedEventHandler(Avatar_5_Click);
            Avatar_6.Click -= new RoutedEventHandler(Avatar_6_Click);
            Avatar_7.Click -= new RoutedEventHandler(Avatar_7_Click);
            Avatar_8.Click -= new RoutedEventHandler(Avatar_8_Click);
            Avatar_9.Click -= new RoutedEventHandler(Avatar_9_Click);

            AvatarReturnToMainMenu.Click -= new RoutedEventHandler(AvatarReturnToMainMenu_Click);
            AvatarReturnToMainMenuP2.Click -= new RoutedEventHandler(AvatarReturnToMainMenuP2_Click);
        }

        public void checkChooseAvatarButtons()
        {

            mainWindow.CheckButton(Avatar1, mainWindow.HandP1);
            mainWindow.CheckButton(Avatar2, mainWindow.HandP1);
            mainWindow.CheckButton(Avatar3, mainWindow.HandP1);
            mainWindow.CheckButton(Avatar4, mainWindow.HandP1);
            mainWindow.CheckButton(Avatar5, mainWindow.HandP1);
            mainWindow.CheckButton(Avatar6, mainWindow.HandP1);
            mainWindow.CheckButton(Avatar7, mainWindow.HandP1);
            mainWindow.CheckButton(Avatar8, mainWindow.HandP1);
            mainWindow.CheckButton(Avatar9, mainWindow.HandP1);
            mainWindow.CheckButton(AvatarReturnToMainMenu, mainWindow.HandP1);

            mainWindow.CheckButton(Avatar_1, mainWindow.HandP2);
            mainWindow.CheckButton(Avatar_2, mainWindow.HandP2);
            mainWindow.CheckButton(Avatar_3, mainWindow.HandP2);
            mainWindow.CheckButton(Avatar_4, mainWindow.HandP2);
            mainWindow.CheckButton(Avatar_5, mainWindow.HandP2);
            mainWindow.CheckButton(Avatar_6, mainWindow.HandP2);
            mainWindow.CheckButton(Avatar_7, mainWindow.HandP2);
            mainWindow.CheckButton(Avatar_8, mainWindow.HandP2);
            mainWindow.CheckButton(Avatar_9, mainWindow.HandP2);
            mainWindow.CheckButton(AvatarReturnToMainMenuP2, mainWindow.HandP2);

        }


        void unselectAvatarP1(int index)
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

        void selectAvatarP1(int index)
        {

            mainWindow.selection.Play();

            mainWindow.selected_avatarP1 = index;
            AvatarImageP1.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/avatars/avatar_body_" + index + ".png"));

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

        void unselectAvatarP2(int index)
        {
            if (index == 1)
            {
                AvatarSelected_1.Visibility = Visibility.Collapsed;
            }
            else if (index == 2)
            {
                AvatarSelected_2.Visibility = Visibility.Collapsed;
            }
            else if (index == 3)
            {
                AvatarSelected_3.Visibility = Visibility.Collapsed;
            }
            else if (index == 4)
            {
                AvatarSelected_4.Visibility = Visibility.Collapsed;
            }
            else if (index == 5)
            {
                AvatarSelected_5.Visibility = Visibility.Collapsed;
            }
            else if (index == 6)
            {
                AvatarSelected_6.Visibility = Visibility.Collapsed;
            }
            else if (index == 7)
            {
                AvatarSelected_7.Visibility = Visibility.Collapsed;
            }
            else if (index == 8)
            {
                AvatarSelected_8.Visibility = Visibility.Collapsed;
            }
            else
            {
                AvatarSelected_9.Visibility = Visibility.Collapsed;
            }
        }

        void selectAvatarP2(int index)
        {

            mainWindow.selection.Play();

            mainWindow.selected_avatarP2 = index;
            AvatarImageP2.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/avatars/avatar_body_" + index + ".png"));

            if (index == 1)
            {
                AvatarSelected_1.Visibility = Visibility.Visible;
            }
            else if (index == 2)
            {
                AvatarSelected_2.Visibility = Visibility.Visible;
            }
            else if (index == 3)
            {
                AvatarSelected_3.Visibility = Visibility.Visible;
            }
            else if (index == 4)
            {
                AvatarSelected_4.Visibility = Visibility.Visible;
            }
            else if (index == 5)
            {
                AvatarSelected_5.Visibility = Visibility.Visible;
            }
            else if (index == 6)
            {
                AvatarSelected_6.Visibility = Visibility.Visible;
            }
            else if (index == 7)
            {
                AvatarSelected_7.Visibility = Visibility.Visible;
            }
            else if (index == 8)
            {
                AvatarSelected_8.Visibility = Visibility.Visible;
            }
            else
            {
                AvatarSelected_9.Visibility = Visibility.Visible;
            }
        }


        // Avatar P1 Button EventHandlers
        void Avatar1_Click(object sender, RoutedEventArgs e)
        {
            unselectAvatarP1(mainWindow.selected_avatarP1);
            selectAvatarP1(1);
        }
        void Avatar2_Click(object sender, RoutedEventArgs e)
        {
            unselectAvatarP1(mainWindow.selected_avatarP1);
            selectAvatarP1(2);
        }
        void Avatar3_Click(object sender, RoutedEventArgs e)
        {
            unselectAvatarP1(mainWindow.selected_avatarP1);
            selectAvatarP1(3);
        }
        void Avatar4_Click(object sender, RoutedEventArgs e)
        {
            unselectAvatarP1(mainWindow.selected_avatarP1);
            selectAvatarP1(4);
        }
        void Avatar5_Click(object sender, RoutedEventArgs e)
        {
            unselectAvatarP1(mainWindow.selected_avatarP1);
            selectAvatarP1(5);
        }
        void Avatar6_Click(object sender, RoutedEventArgs e)
        {
            unselectAvatarP1(mainWindow.selected_avatarP1);
            selectAvatarP1(6);
        }
        void Avatar7_Click(object sender, RoutedEventArgs e)
        {
            unselectAvatarP1(mainWindow.selected_avatarP1);
            selectAvatarP1(7);
        }
        void Avatar8_Click(object sender, RoutedEventArgs e)
        {
            unselectAvatarP1(mainWindow.selected_avatarP1);
            selectAvatarP1(8);
        }
        void Avatar9_Click(object sender, RoutedEventArgs e)
        {
            unselectAvatarP1(mainWindow.selected_avatarP1);
            selectAvatarP1(9);
        }

        // Avatar P2 Button EventHandlers
        void Avatar_1_Click(object sender, RoutedEventArgs e)
        {
            unselectAvatarP2(mainWindow.selected_avatarP2);
            selectAvatarP2(1);
        }
        void Avatar_2_Click(object sender, RoutedEventArgs e)
        {
            unselectAvatarP2(mainWindow.selected_avatarP2);
            selectAvatarP2(2);
        }
        void Avatar_3_Click(object sender, RoutedEventArgs e)
        {
            unselectAvatarP2(mainWindow.selected_avatarP2);
            selectAvatarP2(3);
        }
        void Avatar_4_Click(object sender, RoutedEventArgs e)
        {
            unselectAvatarP2(mainWindow.selected_avatarP2);
            selectAvatarP2(4);
        }
        void Avatar_5_Click(object sender, RoutedEventArgs e)
        {
            unselectAvatarP2(mainWindow.selected_avatarP2);
            selectAvatarP2(5);
        }
        void Avatar_6_Click(object sender, RoutedEventArgs e)
        {
            unselectAvatarP2(mainWindow.selected_avatarP2);
            selectAvatarP2(6);
        }
        void Avatar_7_Click(object sender, RoutedEventArgs e)
        {
            unselectAvatarP2(mainWindow.selected_avatarP2);
            selectAvatarP2(7);
        }
        void Avatar_8_Click(object sender, RoutedEventArgs e)
        {
            unselectAvatarP2(mainWindow.selected_avatarP2);
            selectAvatarP2(8);
        }
        void Avatar_9_Click(object sender, RoutedEventArgs e)
        {
            unselectAvatarP2(mainWindow.selected_avatarP2);
            selectAvatarP2(9);
        }

        void AvatarReturnToMainMenu_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.changeGameState(MainWindow.game_states_t.MAIN_MENU, mainWindow.xamlChooseAvatar, mainWindow.xamlMainMenu);
        }

        void AvatarReturnToMainMenuP2_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.changeGameState(MainWindow.game_states_t.MAIN_MENU, mainWindow.xamlChooseAvatar, mainWindow.xamlMainMenu);
        }

    }
}
