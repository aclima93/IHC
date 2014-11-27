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
    public partial class MainWindow : Window
    {

        Runtime runtime = Runtime.Kinects[0];

        public enum game_states_t { MAIN_MENU, PLAY, PRACTICE, CHOOSE_AVATAR, NEW_SAVE_LOAD, GAME_ON };
        public game_states_t game_state = game_states_t.MAIN_MENU;
        public int selected_avatar = 1;
        public string[] game_file = File.ReadAllLines("/Resources/game_files/saved_game_1.txt");

        private static double _topBoundary;
        private static double _bottomBoundary;
        private static double _leftBoundary;
        private static double _rightBoundary;
        private static double _itemLeft;
        private static double _itemTop;


      public MainWindow()
        {

            //Runtime runtime = new Runtime();

            //Runtime runtime;

            InitializeComponent();
            this.WindowState = System.Windows.WindowState.Maximized;
            this.WindowStyle = System.Windows.WindowStyle.None;

            Loaded += new RoutedEventHandler(MainWindow_Loaded);
            Unloaded += new RoutedEventHandler(MainWindow_Unloaded);


            /*
            StreamReader mysr = new StreamReader("xaml");
            WindowContent.Content = (Page)XamlReader.Load(mysr.BaseStream);
            */

            
            MainMenu.Visibility = Visibility.Visible;
            Play.Visibility = Visibility.Collapsed;
            Practice.Visibility = Visibility.Collapsed;
            GameOn.Visibility = Visibility.Collapsed;
            ChooseAvatar.Visibility = Visibility.Collapsed;
            NewSaveLoad.Visibility = Visibility.Collapsed;
            

            // Main Memu Handlers
            MainMenuItem1.Click += new RoutedEventHandler(MainMenuItem1_Click);
            MainMenuItem2.Click += new RoutedEventHandler(MainMenuItem2_Click);
            MainMenuItem3.Click += new RoutedEventHandler(MainMenuItem3_Click);
            MainMenuItem4.Click += new RoutedEventHandler(MainMenuItem4_Click);


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


            // New Save Load Handlers
            NewSaveLoadItem1.Click += new RoutedEventHandler(NewSaveLoadItem1_Click);
            NewSaveLoadItem2.Click += new RoutedEventHandler(NewSaveLoadItem2_Click);
            NewSaveLoadItem3.Click += new RoutedEventHandler(NewSaveLoadItem3_Click);
            NewSaveLoadItem4.Click += new RoutedEventHandler(NewSaveLoadItem4_Click);



            runtime.VideoFrameReady += runtime_VideoFrameReady;
            runtime.SkeletonFrameReady += runtime_SkeletonFrameReady;

        }

      private void transitionFromTo(UIElement from, UIElement to)
      {
          from.Visibility = Visibility.Collapsed;
          to.Visibility = Visibility.Visible;
      }

      private void checkMainMenuButtons()
      {

          CheckButton(MainMenuItem1, RightHand);
          CheckButton(MainMenuItem2, RightHand);
          CheckButton(MainMenuItem3, RightHand);
          CheckButton(MainMenuItem4, RightHand);

      }

      private void checkChooseAvatarButtons()
      {

          CheckButton(Avatar1, RightHand);
          CheckButton(Avatar2, RightHand);
          CheckButton(Avatar3, RightHand);
          CheckButton(Avatar4, RightHand);
          CheckButton(Avatar5, RightHand);
          CheckButton(Avatar6, RightHand);
          CheckButton(Avatar7, RightHand);
          CheckButton(Avatar8, RightHand);
          CheckButton(Avatar9, RightHand);

      }

      private void checkPlayButtons()
      {
      }

      private void checkPracticeButtons()
      {
      }

      private void checkGameOnButtons()
      {
      }

      private void checkNewSaveLoadButtons()
      {
      }

      private void UnregisterEvents()
      {

          /*
          KinectSensor.KinectSensors.StatusChanged -= KinectSensors_StatusChanged;
          */

          /*
          runtime.VideoFrameReady -= runtime_VideoFrameReady;
          runtime.SkeletonFrameReady -= runtime_SkeletonFrameReady;
          */
          
           
      } 

       
        void runtime_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            SkeletonFrame skeletonSet = e.SkeletonFrame;

            SkeletonData data = (from s in skeletonSet.Skeletons
                                 where s.TrackingState == SkeletonTrackingState.Tracked
                                 select s).FirstOrDefault();


            if (data != null)
            {
                if (RightHand.Visibility != Visibility.Collapsed)
                {
                    if (game_state != game_states_t.PLAY && game_state != game_states_t.PRACTICE)
                    {
                        SetEllipsePosition(RightHand, data.Joints[JointID.HandRight]);
                    }
                    else
                    {
                        RightHand.Visibility = Visibility.Collapsed;
                    }
                }
            }

            if (game_state == game_states_t.MAIN_MENU)
            {
                checkMainMenuButtons();
            }
            else if (game_state == game_states_t.CHOOSE_AVATAR)
            {
                checkChooseAvatarButtons();
            }
            else if (game_state == game_states_t.NEW_SAVE_LOAD)
            {
                checkNewSaveLoadButtons();
            }
            else if (game_state == game_states_t.PLAY)
            {
                checkPlayButtons();
            }
            else if (game_state == game_states_t.PRACTICE)
            {
                checkPracticeButtons();
            }

        }


        private void SetEllipsePosition(Ellipse ellipse, Joint joint)
        {

            Microsoft.Research.Kinect.Nui.Vector vector = new Microsoft.Research.Kinect.Nui.Vector();
            vector.X = ScaleVector(1280, joint.Position.X);
            vector.Y = ScaleVector(1024, -joint.Position.Y);
            vector.Z = joint.Position.Z;

            Joint updatedJoint = new Joint();
            updatedJoint.ID = joint.ID;
            updatedJoint.TrackingState = JointTrackingState.Tracked;
            updatedJoint.Position = vector;

            Canvas.SetLeft(ellipse, updatedJoint.Position.X);
            Canvas.SetTop(ellipse, updatedJoint.Position.Y);
        }

        private float ScaleVector(int length, float position)
        {
            float value = (((((float)length) / 1f) / 2f) * position) + (length / 2);
            if (value > length)
            {
                return (float)length;
            }
            if (value < 0f)
            {
                return 0f;
            }
            return value;
        }

        void MainWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            runtime.Uninitialize();
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //Since only a color video stream is needed, RuntimeOptions.UseColor is used.
            runtime.Initialize(Microsoft.Research.Kinect.Nui.RuntimeOptions.UseColor | RuntimeOptions.UseSkeletalTracking);

            //You can adjust the resolution here.
            runtime.VideoStream.Open(ImageStreamType.Video, 2, ImageResolution.Resolution1280x1024, ImageType.Color);
        }

        void runtime_VideoFrameReady(object sender, Microsoft.Research.Kinect.Nui.ImageFrameReadyEventArgs e)
        {
            PlanarImage image = e.ImageFrame.Image;
        
            BitmapSource source = BitmapSource.Create(image.Width, image.Height, 0, 0,
                PixelFormats.Bgr32, null, image.Bits, image.Width * image.BytesPerPixel);
            videoImage.Source = source;
        }


        private static void CheckButton(HoverButton button, Ellipse thumbStick)
        {


            if (IsItemMidpointInContainer(button, thumbStick))
            {
                button.Hovering();
            }
            else
            {
                button.Release();
            }
        }


        public static bool IsItemMidpointInContainer(FrameworkElement container, FrameworkElement target)
        {
            FindValues(container, target);

            if (_itemTop < _topBoundary || _bottomBoundary < _itemTop)
            {
                //Midpoint of target is outside of top or bottom
                return false;
            }

            if (_itemLeft < _leftBoundary || _rightBoundary < _itemLeft)
            {
                //Midpoint of target is outside of left or right
                return false;
            }

            return true;
        }


        private static void FindValues(FrameworkElement container, FrameworkElement target)
        {
            var containerTopLeft = container.PointToScreen(new Point());
            var itemTopLeft = target.PointToScreen(new Point());

            _topBoundary = containerTopLeft.Y;
            _bottomBoundary = _topBoundary + container.ActualHeight;
            _leftBoundary = containerTopLeft.X;
            _rightBoundary = _leftBoundary + container.ActualWidth;

            //use midpoint of item (width or height divided by 2)
            _itemLeft = itemTopLeft.X + (target.ActualWidth / 2);
            _itemTop = itemTopLeft.Y + (target.ActualHeight / 2);
        }


        // --------
        // Button EventHandlers
        // --------

        // Main Menu Button EventHandlers
        void MainMenuItem1_Click(object sender, RoutedEventArgs e)
        {

            SoundPlayer correct = new SoundPlayer("swoosh.wav");
            correct.Play();

            game_state = game_states_t.PLAY;
            transitionFromTo(MainMenu, Play);

        }
        void MainMenuItem2_Click(object sender, RoutedEventArgs e)
        {

            SoundPlayer correct = new SoundPlayer("swoosh.wav");
            correct.Play();

            game_state = game_states_t.PRACTICE;
            transitionFromTo(MainMenu, Practice);

        }
        void MainMenuItem3_Click(object sender, RoutedEventArgs e)
        {

            SoundPlayer correct = new SoundPlayer("swoosh.wav");
            correct.Play();

            game_state = game_states_t.CHOOSE_AVATAR;
            transitionFromTo(MainMenu, ChooseAvatar);

        }
        void MainMenuItem4_Click(object sender, RoutedEventArgs e)
        {

            SoundPlayer correct = new SoundPlayer("swoosh.wav");
            correct.Play();

            game_state = game_states_t.NEW_SAVE_LOAD;
            transitionFromTo(MainMenu, NewSaveLoad);
        }


        // Avatar Button EventHandlers
        void Avatar1_Click(object sender, RoutedEventArgs e)
        {

            SoundPlayer correct = new SoundPlayer("selection-click.wav");
            correct.Play();

            game_state = game_states_t.CHOOSE_AVATAR;

            unselectAvatar(selected_avatar);
            selectAvatar(1);

        }
        void Avatar2_Click(object sender, RoutedEventArgs e)
        {

            SoundPlayer correct = new SoundPlayer("selection-click.wav");
            correct.Play();

            game_state = game_states_t.CHOOSE_AVATAR;

            unselectAvatar(selected_avatar);
            selectAvatar(2);

        }
        void Avatar3_Click(object sender, RoutedEventArgs e)
        {

            SoundPlayer correct = new SoundPlayer("selection-click.wav");
            correct.Play();

            game_state = game_states_t.CHOOSE_AVATAR;

            unselectAvatar(selected_avatar);
            selectAvatar(3);

        }
        void Avatar4_Click(object sender, RoutedEventArgs e)
        {

            SoundPlayer correct = new SoundPlayer("selection-click.wav");
            correct.Play();

            game_state = game_states_t.CHOOSE_AVATAR;

            unselectAvatar(selected_avatar);
            selectAvatar(4);

        }
        void Avatar5_Click(object sender, RoutedEventArgs e)
        {

            SoundPlayer correct = new SoundPlayer("selection-click.wav");
            correct.Play();

            game_state = game_states_t.CHOOSE_AVATAR;

            unselectAvatar(selected_avatar);
            selectAvatar(5);

        }
        void Avatar6_Click(object sender, RoutedEventArgs e)
        {

            SoundPlayer correct = new SoundPlayer("selection-click.wav");
            correct.Play();

            game_state = game_states_t.CHOOSE_AVATAR;

            unselectAvatar(selected_avatar);
            selectAvatar(6);

        }
        void Avatar7_Click(object sender, RoutedEventArgs e)
        {

            SoundPlayer correct = new SoundPlayer("selection-click.wav");
            correct.Play();

            game_state = game_states_t.CHOOSE_AVATAR;

            unselectAvatar(selected_avatar);
            selectAvatar(7);

        }
        void Avatar8_Click(object sender, RoutedEventArgs e)
        {

            SoundPlayer correct = new SoundPlayer("selection-click.wav");
            correct.Play();

            game_state = game_states_t.CHOOSE_AVATAR;

            unselectAvatar(selected_avatar);
            selectAvatar(8);

        }
        void Avatar9_Click(object sender, RoutedEventArgs e)
        {

            SoundPlayer correct = new SoundPlayer("selection-click.wav");
            correct.Play();

            game_state = game_states_t.CHOOSE_AVATAR;

            unselectAvatar(selected_avatar);
            selectAvatar(9);

        }
        void AvatarReturnToMainMenu_Click(object sender, RoutedEventArgs e)
        {

            SoundPlayer correct = new SoundPlayer("swoosh.wav");
            correct.Play();

            game_state = game_states_t.MAIN_MENU;
            transitionFromTo(ChooseAvatar, MainMenu);

        }


        void NewSaveLoadItem1_Click(object sender, RoutedEventArgs e)
        {
            // do something to call the file browsing thingamajiggy
        }
        void NewSaveLoadItem2_Click(object sender, RoutedEventArgs e)
        {
            // do something to call the file browsing thingamajiggy
        }
        void NewSaveLoadItem3_Click(object sender, RoutedEventArgs e){
            // do something to call the file browsing thingamajiggy
        }

        void NewSaveLoadItem4_Click(object sender, RoutedEventArgs e)
        {

            SoundPlayer correct = new SoundPlayer("swoosh.wav");
            correct.Play();

            game_state = game_states_t.MAIN_MENU;
            transitionFromTo(NewSaveLoad, MainMenu);

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
            AvatarImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/avatars/avatar_body_"+index+".png"));

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



    }
}
