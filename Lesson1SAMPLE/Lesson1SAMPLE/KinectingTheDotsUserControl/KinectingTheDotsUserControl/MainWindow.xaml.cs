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
    public partial class MainWindow : Window
    {

        Runtime runtime = Runtime.Kinects[0];

        public enum game_states_t { MAIN_MENU, PLAY, PRACTICE, CHOOSE_AVATAR, NEW_SAVE_LOAD, GAME_ON };
        
        private game_states_t game_state = game_states_t.MAIN_MENU;
        public string[] game_file;
        private const bool DEBUG = true;

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


            xamlMainMenu.setMainWindow(this);
            xamlChooseAvatar.setMainWindow(this);
            xamlNewSaveLoad.setMainWindow(this);


            // Hide your kids, hide your wife
            xamlMainMenu.MainMenu.Visibility = Visibility.Visible;
            Play.Visibility = Visibility.Collapsed;
            Practice.Visibility = Visibility.Collapsed;
            GameOn.Visibility = Visibility.Collapsed;
            xamlChooseAvatar.ChooseAvatar.Visibility = Visibility.Collapsed;
            xamlNewSaveLoad.NewSaveLoad.Visibility = Visibility.Collapsed;

            activateHandlersFor(game_state);

            runtime.VideoFrameReady += runtime_VideoFrameReady;
            runtime.SkeletonFrameReady += runtime_SkeletonFrameReady;

        }


        private void drawSkeletons(SkeletonData data)
        {

            SetEllipsePosition(Joint1, data.Joints[JointID.AnkleLeft]);
            SetEllipsePosition(Joint2, data.Joints[JointID.AnkleRight]);

            SetEllipsePosition(Joint3, data.Joints[JointID.ElbowLeft]);
            SetEllipsePosition(Joint4, data.Joints[JointID.ElbowRight]);

            SetEllipsePosition(Joint5, data.Joints[JointID.FootLeft]);
            SetEllipsePosition(Joint6, data.Joints[JointID.FootRight]);

            SetEllipsePosition(Joint7, data.Joints[JointID.HandLeft]);
            SetEllipsePosition(Joint8, data.Joints[JointID.HandRight]);

            SetEllipsePosition(Joint9, data.Joints[JointID.Head]);

            SetEllipsePosition(Joint10, data.Joints[JointID.HipCenter]);
            SetEllipsePosition(Joint11, data.Joints[JointID.HipLeft]);
            SetEllipsePosition(Joint12, data.Joints[JointID.HipRight]);

            SetEllipsePosition(Joint13, data.Joints[JointID.KneeLeft]);
            SetEllipsePosition(Joint14, data.Joints[JointID.KneeRight]);

            SetEllipsePosition(Joint15, data.Joints[JointID.ShoulderCenter]);
            SetEllipsePosition(Joint16, data.Joints[JointID.ShoulderLeft]);
            SetEllipsePosition(Joint17, data.Joints[JointID.ShoulderRight]);

            SetEllipsePosition(Joint18, data.Joints[JointID.Spine]);

            SetEllipsePosition(Joint19, data.Joints[JointID.WristLeft]);
            SetEllipsePosition(Joint20, data.Joints[JointID.WristRight]);

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
                    //if (game_state != game_states_t.GAME_ON)
                    if (game_state != game_states_t.PLAY && game_state != game_states_t.PRACTICE)
                    {
                        SetEllipsePosition(RightHand, data.Joints[JointID.HandRight]);
                    }
                    else
                    {
                        RightHand.Visibility = Visibility.Collapsed;
                        drawSkeletons(data);
                    }
                }
                else
                {

                    drawSkeletons(data);

                }

            }

            if (game_state == game_states_t.MAIN_MENU)
            {
                xamlMainMenu.checkMainMenuButtons();
            }
            else if (game_state == game_states_t.CHOOSE_AVATAR)
            {
                xamlChooseAvatar.checkChooseAvatarButtons();
            }
            else if (game_state == game_states_t.NEW_SAVE_LOAD)
            {
                xamlNewSaveLoad.checkNewSaveLoadButtons();
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


        public void CheckButton(HoverButton button, Ellipse thumbStick)
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

        public void changeGameState(game_states_t new_state, UIElement from, UIElement to)
        {
            deactivateHandlersFor(game_state);
            game_state = new_state;
            transitionFromTo(from, to);
            activateHandlersFor(game_state);
        }

        private void activateHandlersFor(game_states_t state)
        {

            if (DEBUG) Console.WriteLine("Activating event handlers for game state: {0}", state);

            if (state == game_states_t.MAIN_MENU)
            {
                xamlMainMenu.setMainMenuHandlers();
            }
            else if (state == game_states_t.CHOOSE_AVATAR)
            {
                xamlChooseAvatar.setChooseAvatarHandlers();
            }
            else if (state == game_states_t.NEW_SAVE_LOAD)
            {
                xamlNewSaveLoad.setNewSaveLoadHandlers();
            }

        }

        private void deactivateHandlersFor(game_states_t state)
        {

            if (DEBUG) Console.WriteLine("Deactivating event handlers for game state: {0}", state);

            if (state == game_states_t.MAIN_MENU)
            {
                xamlMainMenu.removeMainMenuHandlers();
            }
            else if (state == game_states_t.CHOOSE_AVATAR)
            {
                xamlChooseAvatar.removeChooseAvatarHandlers();
            }
            else if (state == game_states_t.NEW_SAVE_LOAD)
            {
                xamlNewSaveLoad.removeNewSaveLoadHandlers();
            }

        }

        private void transitionFromTo(UIElement from, UIElement to)
        {
            from.Visibility = Visibility.Collapsed;
            to.Visibility = Visibility.Visible;
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



    }
}
