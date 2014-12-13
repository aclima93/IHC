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
using System.Windows.Media.Animation;



namespace KinectingTheDotsUserControl
{
    public partial class MainWindow : Window
    {

        Runtime runtime = Runtime.Kinects[0];

        public enum game_states_t { MAIN_MENU, PLAY, PRACTICE, CHOOSE_AVATAR, NEW_SAVE_LOAD, GAME_ON };
        private game_states_t game_state;

        public string[] game_file;

        public const bool DEBUG = true;

        public SoundPlayer transition = new SoundPlayer("swoosh.wav");
        public SoundPlayer selection = new SoundPlayer("selection-click.wav");
        public Storyboard sb;
        public Storyboard sb2;
        
        public Ball ball;

        public const int screen_width = 1366;
        public const int screen_height = 768;

        public long player1_score = 0;
        public long player2_score = 0;

        public const int gained_points = 50;
        public const int lost_points = 0;

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
            xamlPlay.setMainWindow(this);
            xamlPractice.setMainWindow(this);


            // Hide your kids, hide your wife
            game_state = game_states_t.PRACTICE;
            xamlMainMenu.Visibility = Visibility.Collapsed;

            xamlPractice.Visibility = Visibility.Visible;
            //game_state = game_states_t.MAIN_MENU;
            //xamlMainMenu.Visibility = Visibility.Visible;

            //xamlPractice.Visibility = Visibility.Collapsed;
            xamlChooseAvatar.Visibility = Visibility.Collapsed;
            xamlNewSaveLoad.Visibility = Visibility.Collapsed;
            xamlPlay.Visibility = Visibility.Collapsed;
            Paused1Player.Visibility = Visibility.Collapsed;
            Paused2Players.Visibility = Visibility.Collapsed;


            activateHandlersFor(game_state);


            runtime.VideoFrameReady += runtime_VideoFrameReady;
            runtime.SkeletonFrameReady += runtime_SkeletonFrameReady;

            resetBall();

        }

        public void resetBall()
        {
            Random r = new Random();

            float dx = r.Next(-10, 11);
            float dy = r.Next(-10, 11);
            float dz = -7;//r.Next(-3, 0);

            float radius = 16;//32; //hardcoded because fuck you, that's why
            int size = 64;

            // because we use fullscreen. fuck you.
            int window_width = screen_width;
            int window_height = screen_height;

            int distLR = window_width - 100;
            int distUD = window_height - 125;
            int distFB = 500;

            float x = r.Next(-distLR / 2, (distLR / 2) + 1);
            float y = r.Next(-distUD / 2, (distUD / 2) + 1);
            float z = radius*2 + 1; // to be tweeked?

            ball = new Ball(x, y, z, dx, dy, dz, 
                radius, size, 
                screen_width, screen_height, 
                window_width, window_height, 
                distLR, distUD, distFB);
        }

        public void updateBallAndSkeleton1(SkeletonData data)
        {

            ball.updatePosition();

            drawSkeleton1(data); // joint collisions with ball are made when they are updated and drawn to reduce redundance

            if (ball.checkWallCollisions())
            {
                //resetBall();
                player1_score = player1_score - lost_points;
            }
            
            Console.WriteLine("");
            Console.WriteLine("");

            Ball_2D.Height = ball.getSize();
            Ball_2D.Width = Ball_2D.Height;
            
            Canvas.SetLeft(Ball_2D, ball.getX2D() - Ball_2D.Height / 2);
            Canvas.SetTop(Ball_2D, ball.getY2D() - Ball_2D.Height / 2);

            Canvas.SetLeft(xL, ball.getX2D() - Ball_2D.Height / 2);
            Canvas.SetTop(xL, ball.getY2D() - Ball_2D.Height / 2);

            Canvas.SetLeft(xR, ball.getX2D() + Ball_2D.Height / 2);
            Canvas.SetTop(xR, ball.getY2D() - Ball_2D.Height / 2);

            Canvas.SetLeft(yU, ball.getX2D() - Ball_2D.Height / 2);
            Canvas.SetTop(yU, ball.getY2D() + Ball_2D.Height / 2);

            Canvas.SetLeft(yD, ball.getX2D() + Ball_2D.Height / 2);
            Canvas.SetTop(yD, ball.getY2D() + Ball_2D.Height / 2);

        }

        public void updateBallAndSkeleton2(SkeletonData data)
        {

            drawSkeleton2(data);

            // To be added once the collisions work on the other side...
        }

        private void drawSkeleton1(SkeletonData data)
        {

            int num_joint_collisions = 0;

            num_joint_collisions += SetEllipsePosition(P1J1, data.Joints[JointID.AnkleLeft], true);
            num_joint_collisions += SetEllipsePosition(P1J2, data.Joints[JointID.AnkleRight], true);

            num_joint_collisions += SetEllipsePosition(P1J3, data.Joints[JointID.ElbowLeft], true);
            num_joint_collisions += SetEllipsePosition(P1J4, data.Joints[JointID.ElbowRight], true);

            num_joint_collisions += SetEllipsePosition(P1J5, data.Joints[JointID.FootLeft], true);
            num_joint_collisions += SetEllipsePosition(P1J6, data.Joints[JointID.FootRight], true);

            num_joint_collisions += SetEllipsePosition(P1J7, data.Joints[JointID.HandLeft], true);
            num_joint_collisions += SetEllipsePosition(P1J8, data.Joints[JointID.HandRight], true);

            num_joint_collisions += SetEllipsePosition(P1J9, data.Joints[JointID.Head], true);

            num_joint_collisions += SetEllipsePosition(P1J10, data.Joints[JointID.HipCenter], true);
            num_joint_collisions += SetEllipsePosition(P1J11, data.Joints[JointID.HipLeft], true);
            num_joint_collisions += SetEllipsePosition(P1J12, data.Joints[JointID.HipRight], true);

            num_joint_collisions += SetEllipsePosition(P1J13, data.Joints[JointID.KneeLeft], true);
            num_joint_collisions += SetEllipsePosition(P1J14, data.Joints[JointID.KneeRight], true);

            num_joint_collisions += SetEllipsePosition(P1J15, data.Joints[JointID.ShoulderCenter], true);
            num_joint_collisions += SetEllipsePosition(P1J16, data.Joints[JointID.ShoulderLeft], true);
            num_joint_collisions += SetEllipsePosition(P1J17, data.Joints[JointID.ShoulderRight], true);

            num_joint_collisions += SetEllipsePosition(P1J18, data.Joints[JointID.Spine], true);

            num_joint_collisions += SetEllipsePosition(P1J19, data.Joints[JointID.WristLeft], true);
            num_joint_collisions += SetEllipsePosition(P1J20, data.Joints[JointID.WristRight], true);


            player1_score = player1_score + (gained_points * num_joint_collisions);
            if (num_joint_collisions > 0)
            {
                Console.WriteLine("[Collision] Collided with {0} joints!", num_joint_collisions);
                Console.WriteLine("");
            }

        }

        private void drawSkeleton2(SkeletonData data)
        {

            //SetEllipsePosition(Ball_2D, data.Joints[JointID.HandRight]);

            //if (DEBUG) Console.WriteLine("Ball at: x={0} y={1}, size={2}", ball.getX2D(), ball.getY2D(), ball.getSize());

            int num_joint_collisions = 0;

            num_joint_collisions += SetEllipsePosition(P2J1, data.Joints[JointID.AnkleLeft], true);
            num_joint_collisions += SetEllipsePosition(P2J2, data.Joints[JointID.AnkleRight], true);

            num_joint_collisions += SetEllipsePosition(P2J3, data.Joints[JointID.ElbowLeft], true);
            num_joint_collisions += SetEllipsePosition(P2J4, data.Joints[JointID.ElbowRight], true);

            num_joint_collisions += SetEllipsePosition(P2J5, data.Joints[JointID.FootLeft], true);
            num_joint_collisions += SetEllipsePosition(P2J6, data.Joints[JointID.FootRight], true);

            num_joint_collisions += SetEllipsePosition(P2J7, data.Joints[JointID.HandLeft], true);
            num_joint_collisions += SetEllipsePosition(P2J8, data.Joints[JointID.HandRight], true);

            num_joint_collisions += SetEllipsePosition(P2J9, data.Joints[JointID.Head], true);

            num_joint_collisions += SetEllipsePosition(P2J10, data.Joints[JointID.HipCenter], true);
            num_joint_collisions += SetEllipsePosition(P2J11, data.Joints[JointID.HipLeft], true);
            num_joint_collisions += SetEllipsePosition(P2J12, data.Joints[JointID.HipRight], true);

            num_joint_collisions += SetEllipsePosition(P2J13, data.Joints[JointID.KneeLeft], true);
            num_joint_collisions += SetEllipsePosition(P2J14, data.Joints[JointID.KneeRight], true);

            num_joint_collisions += SetEllipsePosition(P2J15, data.Joints[JointID.ShoulderCenter], true);
            num_joint_collisions += SetEllipsePosition(P2J16, data.Joints[JointID.ShoulderLeft], true);
            num_joint_collisions += SetEllipsePosition(P2J17, data.Joints[JointID.ShoulderRight], true);

            num_joint_collisions += SetEllipsePosition(P2J18, data.Joints[JointID.Spine], true);

            num_joint_collisions += SetEllipsePosition(P2J19, data.Joints[JointID.WristLeft], true);
            num_joint_collisions += SetEllipsePosition(P2J20, data.Joints[JointID.WristRight], true);


            player2_score = player2_score + (gained_points * num_joint_collisions);
            if (num_joint_collisions > 0)
            {
                Console.WriteLine("[Collision] Collided with {0} joints!", num_joint_collisions);
                Console.WriteLine("");
            }

        }

        void runtime_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            SkeletonFrame skeletonSet = e.SkeletonFrame;

            /*
            SkeletonData data = (from s in skeletonSet.Skeletons
                                 where s.TrackingState == SkeletonTrackingState.Tracked
                                 select s).FirstOrDefault();
            */

            SkeletonData skeleton1 = (from s in skeletonSet.Skeletons
                        where s.TrackingState == SkeletonTrackingState.Tracked
                        select s).FirstOrDefault();

            int count = (from s in skeletonSet.Skeletons
            where s.TrackingState != SkeletonTrackingState.NotTracked
            select s).Count();

            SkeletonData skeleton2 = null;
            if( count > 1 )
                skeleton2 = (from s in skeletonSet.Skeletons
                        where s.TrackingState != SkeletonTrackingState.NotTracked
                        select s).ElementAt(1);


            if (skeleton1 != null)
            {

                unpauseGame1P();

                SetEllipsePosition(HandP1, skeleton1.Joints[JointID.HandRight], false);

                if (skeleton2 != null)
                    SetEllipsePosition(HandP2, skeleton2.Joints[JointID.HandLeft], false);


                if (game_state == game_states_t.PRACTICE)
                {

                    xamlPractice.score.Text = "Score: " + player1_score.ToString();

                    changeP1ItemsVisibilityTo(Visibility.Visible);
                    changeP2ItemsVisibilityTo(Visibility.Visible);
                    updateBallAndSkeleton1(skeleton1);
                }
                else if (game_state == game_states_t.PLAY)
                {

                    if (skeleton2 != null)
                    {

                        unpauseGame2P();

                        // update game score indicators
                        xamlPlay.scorePlayer1.Text = "P1 Score: " + player1_score.ToString();
                        xamlPlay.scorePlayer2.Text = "P2 Score: " + player2_score.ToString();

                        changeP1ItemsVisibilityTo(Visibility.Visible);
                        changeP2ItemsVisibilityTo(Visibility.Visible);
                        updateBallAndSkeleton1(skeleton1);
                        updateBallAndSkeleton2(skeleton2);
                    }
                    else
                    {
                        pauseGame2P();
                    }
                }
                else
                {
                    changeP1ItemsVisibilityTo(Visibility.Collapsed);
                    changeP2ItemsVisibilityTo(Visibility.Collapsed);
                    HandP2.Visibility = Visibility.Collapsed; //ignore the second hand
                }

            }
            else
            {
                pauseGame1P();
            }

            checkGameStateButtons(game_state);

        }

        private void changeP1ItemsVisibilityTo(Visibility v){
        
            if(v == Visibility.Visible)
                HandP1.Visibility = Visibility.Collapsed;
            else if(v == Visibility.Collapsed)
                HandP1.Visibility = Visibility.Visible;

            Ball_2D.Visibility = v;

            changeSkeleton1VisibilityTo(v);
        
        }

        private void changeP2ItemsVisibilityTo(Visibility v)
        {

            if (v == Visibility.Visible)
                HandP2.Visibility = Visibility.Collapsed;
            else if (v == Visibility.Collapsed)
                HandP2.Visibility = Visibility.Visible;

            Ball2_2D.Visibility = v;

            changeSkeleton2VisibilityTo(v);

        }


        private int SetEllipsePosition(Ellipse ellipse, Joint joint, bool checkCollisionWithBall)
        {

            Microsoft.Research.Kinect.Nui.Vector vector = new Microsoft.Research.Kinect.Nui.Vector();
            vector.X = ScaleVector(screen_width, joint.Position.X);
            vector.Y = ScaleVector(screen_height, -joint.Position.Y);

            Joint updatedJoint = new Joint();
            updatedJoint.ID = joint.ID;
            updatedJoint.TrackingState = JointTrackingState.Tracked;
            updatedJoint.Position = vector;

            Canvas.SetLeft(ellipse, updatedJoint.Position.X);
            Canvas.SetTop(ellipse, updatedJoint.Position.Y);

            if (checkCollisionWithBall)
            {
                if (ball.checkJointCollision(updatedJoint.Position.X, updatedJoint.Position.Y))
                {
                    return 1;
                }
            }
            return 0;
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
            runtime.Initialize(Microsoft.Research.Kinect.Nui.RuntimeOptions.UseColor | RuntimeOptions.UseSkeletalTracking | RuntimeOptions.UseDepthAndPlayerIndex | RuntimeOptions.UseDepth);

            //You can adjust the resolution here.
            runtime.VideoStream.Open(ImageStreamType.Video, 2, ImageResolution.Resolution1280x1024, ImageType.Color);

            /*
            sb = this.FindResource("TransitionAnimation") as Storyboard;
            sb2 = this.FindResource("TransitionAnimation2") as Storyboard;
            */
        }

        void runtime_VideoFrameReady(object sender, Microsoft.Research.Kinect.Nui.ImageFrameReadyEventArgs e)
        {
            PlanarImage image = e.ImageFrame.Image;

            BitmapSource source = BitmapSource.Create(image.Width, image.Height, 0, 0,
                PixelFormats.Bgr32, null, image.Bits, image.Width * image.BytesPerPixel);

            if (DEBUG)
            {
                //xamlPractice.videoImage.Source = source;
            }
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

            // play transition effect
            transition.Play();
            //Storyboard.SetTarget(sb, from);
            //sb.Begin();

            from.Visibility = Visibility.Collapsed;
            to.Visibility = Visibility.Visible;

            //Storyboard.SetTarget(sb2, to);
            //sb2.Begin();

            activateHandlersFor(game_state);
        }

        private void checkGameStateButtons(game_states_t game_state)
        {
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
            else if (game_state == game_states_t.PRACTICE)
            {
                xamlPractice.checkPracticeButtons();
            }
            else if (game_state == game_states_t.PLAY)
            {
                xamlPlay.checkPlayButtons();
            }
        }

        private void activateHandlersFor(game_states_t state)
        {

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
            else if (state == game_states_t.PRACTICE)
            {
                xamlPractice.setPracticeHandlers();
            }
            else if (state == game_states_t.PLAY)
            {
                xamlPlay.setPlayHandlers();
            }

        }

        private void deactivateHandlersFor(game_states_t state)
        {

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
            else if (state == game_states_t.PRACTICE)
            {
                xamlPractice.removePracticeHandlers();
            }
            else if (state == game_states_t.PLAY)
            {
                xamlPlay.removePlayHandlers();
            }

        }

        private void pauseGame1P()
        {
            Paused1Player.Visibility = Visibility.Visible;
            changeSkeleton1VisibilityTo(Visibility.Collapsed);
            Ball_2D.Visibility = Visibility.Collapsed;
        }
        private void unpauseGame1P()
        {
            Paused1Player.Visibility = Visibility.Collapsed;
            changeSkeleton1VisibilityTo(Visibility.Visible);
            Ball_2D.Visibility = Visibility.Visible;
        }
        private void pauseGame2P()
        {
            Paused2Players.Visibility = Visibility.Visible;
            changeSkeleton1VisibilityTo(Visibility.Collapsed);
            changeSkeleton2VisibilityTo(Visibility.Collapsed);
            Ball_2D.Visibility = Visibility.Collapsed;
            Ball2_2D.Visibility = Visibility.Collapsed;
        }
        private void unpauseGame2P()
        {
            Paused2Players.Visibility = Visibility.Collapsed;
            changeSkeleton1VisibilityTo(Visibility.Visible);
            changeSkeleton2VisibilityTo(Visibility.Visible);
            Ball_2D.Visibility = Visibility.Visible;
            Ball2_2D.Visibility = Visibility.Visible;
        }

        private void changeSkeleton1VisibilityTo(Visibility v)
        {
            P1J1.Visibility = v;
            P1J2.Visibility = v;
            P1J3.Visibility = v;
            P1J4.Visibility = v;
            P1J5.Visibility = v;
            P1J6.Visibility = v;
            P1J7.Visibility = v;
            P1J8.Visibility = v;
            P1J9.Visibility = v;
            P1J10.Visibility = v;
            P1J11.Visibility = v;
            P1J12.Visibility = v;
            P1J13.Visibility = v;
            P1J14.Visibility = v;
            P1J15.Visibility = v;
            P1J16.Visibility = v;
            P1J17.Visibility = v;
            P1J18.Visibility = v;
            P1J19.Visibility = v;
            P1J20.Visibility = v;

        }

        private void changeSkeleton2VisibilityTo(Visibility v)
        {
            P2J1.Visibility = v;
            P2J2.Visibility = v;
            P2J3.Visibility = v;
            P2J4.Visibility = v;
            P2J5.Visibility = v;
            P2J6.Visibility = v;
            P2J7.Visibility = v;
            P2J8.Visibility = v;
            P2J9.Visibility = v;
            P2J10.Visibility = v;
            P2J11.Visibility = v;
            P2J12.Visibility = v;
            P2J13.Visibility = v;
            P2J14.Visibility = v;
            P2J15.Visibility = v;
            P2J16.Visibility = v;
            P2J17.Visibility = v;
            P2J18.Visibility = v;
            P2J19.Visibility = v;
            P2J20.Visibility = v;
        }

    }
}
