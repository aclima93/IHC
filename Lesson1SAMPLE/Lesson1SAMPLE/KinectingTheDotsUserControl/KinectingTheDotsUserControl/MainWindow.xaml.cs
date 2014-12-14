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

        public enum game_states_t { MAIN_MENU, PLAY, PRACTICE, CHOOSE_AVATAR, NEW_SAVE_LOAD };
        private game_states_t game_state;

        public string[] game_file;

        public const bool DEBUG = true;

        public SoundPlayer transition = new SoundPlayer("swoosh.wav");
        public SoundPlayer selection = new SoundPlayer("selection-click.wav");
        public SoundPlayer bounce = new SoundPlayer("bounce.wav");
        public SoundPlayer score = new SoundPlayer("score.wav");
        public Storyboard sb;
        public Storyboard sb2;
        
        public Ball ball;

        public const int screen_width = 1366;
        public const int screen_height = 768;

        public static int window_width;
        public static int window_height;

        public static bool game_paused = false;

        public int selected_avatarP1 = 1;
        public int selected_avatarP2 = 3;
        public long player1_score = 0;
        public long player2_score = 0;

        public const int gained_points = 15;//50;
        public const int lost_points = 5;

        private static double _topBoundary;
        private static double _bottomBoundary;
        private static double _leftBoundary;
        private static double _rightBoundary;
        private static double _itemLeft;
        private static double _itemTop;

        private static Microsoft.Research.Kinect.Nui.Vector vector = new Microsoft.Research.Kinect.Nui.Vector();
        private static Joint updatedJoint = new Joint();

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

            game_state = game_states_t.MAIN_MENU;
            xamlMainMenu.Visibility = Visibility.Visible;

            xamlPractice.Visibility = Visibility.Collapsed;
            xamlChooseAvatar.Visibility = Visibility.Collapsed;
            xamlNewSaveLoad.Visibility = Visibility.Collapsed;
            xamlPlay.Visibility = Visibility.Collapsed;
            Paused1Player.Visibility = Visibility.Collapsed;
            Paused2Players.Visibility = Visibility.Collapsed;


            activateHandlersFor(game_state);


            runtime.VideoFrameReady += runtime_VideoFrameReady;
            runtime.SkeletonFrameReady += runtime_SkeletonFrameReady;

        }

        public void resetBall()
        {
            Random r = new Random();

            float dx = 50 * r.Next(-1, 2);
            float dy = 50 * r.Next(-1, 2);
            float dz = -5;//r.Next(-3, 0);

            float radius = 10;//16;//32; //hardcoded because fuck you, that's why
            int size = 64;

            // because we use fullscreen. fuck you.
            window_width = screen_width;
            window_height = screen_height;

            if (game_state == game_states_t.PLAY)
            {
                window_width /= 2;
                window_height /= 2;
            }

            int distLR = (window_width - 100)*7;
            int distUD = (window_height - 125)*10;
            int distFB = 500;


            float x = r.Next((-distLR/2) + 1, (distLR / 2));
            float y = r.Next((-distUD/2) + 1, (distUD / 2));
            float z = radius*2 + 1; // to be tweeked?

            ball = new Ball(x, y, z, dx, dy, dz, 
                radius, size, 
                screen_width, screen_height, 
                window_width, window_height, 
                distLR, distUD, distFB);
        }

        public void updateBallAndSkeleton1(SkeletonData data)
        {


            //if (ball.checkOutsideOfField())
            //    resetBall();

            ball.updatePosition();

            drawSkeleton1(data); // joint collisions with ball are made when they are updated and drawn to reduce redundance

            int collision = ball.checkWallCollisions();

            if (collision != 0)
                bounce.Play();

            if (collision == 2)
            {
                if (player1_score - lost_points >= 0)
                    player1_score = player1_score - lost_points;
            }

            Ball_2D.Height = ball.getSize();
            Ball_2D.Width = Ball_2D.Height;

            Canvas.SetTop(Ball_2D, ball.getY2D() - Ball_2D.Height / 2);

            if(game_state == game_states_t.PRACTICE)
                Canvas.SetLeft(Ball_2D, ball.getX2D() - (Ball_2D.Height / 2));
            else
                Canvas.SetLeft(Ball_2D, ball.getX2D() - (Ball_2D.Height / 2) - (window_width / 2));


        }

        public void updateBallAndSkeleton2(SkeletonData data)
        {

            drawSkeleton2(data);

            int collision = ball.checkWallCollisions();

            if (collision != 0)
                bounce.Play();

            if (collision == 1)
            {

                if (player2_score - lost_points >= 0)
                    player2_score = player2_score - lost_points;
            }

            Ball2_2D.Height = ball.getP2BallSize();
            Ball2_2D.Width = Ball2_2D.Height;

            Canvas.SetTop(Ball2_2D, ball.getY2D() - Ball2_2D.Height / 2);
            Canvas.SetLeft(Ball2_2D, ball.getX2D() - (Ball_2D.Height / 2) + (window_width / 2));

        }

        void runtime_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            SkeletonFrame skeletonSet = e.SkeletonFrame;

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

                //keep the first player on the left
                if (skeleton2 != null)
                {
                    if (skeleton1.Joints[JointID.Head].Position.X > skeleton2.Joints[JointID.Head].Position.X)
                    {
                        SkeletonData temp_skel = skeleton2;
                        skeleton2 = skeleton1;
                        skeleton1 = temp_skel;
                    }
                }


                unpauseGame1P();

                SetEllipsePosition(HandP1, skeleton1.Joints[JointID.HandRight], false, 1);

                if (skeleton2 != null)
                    SetEllipsePosition(HandP2, skeleton2.Joints[JointID.HandLeft], false, 2);
                else
                {
                    HandP2.Visibility = Visibility.Collapsed;
                }


                if (game_state == game_states_t.PRACTICE)
                {

                    xamlPractice.score.Text = "Score: " + player1_score.ToString();

                    changeP1ItemsVisibilityTo(Visibility.Visible);
                    changeP2ItemsVisibilityTo(Visibility.Collapsed);
                    HandP2.Visibility = Visibility.Collapsed;
                    updateBallAndSkeleton1(skeleton1);
                }
                else if (game_state == game_states_t.PLAY)
                {

                    if (skeleton2 != null)
                    {

                        unpauseGame2P( skeleton2 );

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
                else if (game_state == game_states_t.CHOOSE_AVATAR)
                {

                    changeP1ItemsVisibilityTo(Visibility.Collapsed);
                    changeP2ItemsVisibilityTo(Visibility.Collapsed);

                    HandP1.Visibility = Visibility.Visible;

                    if (skeleton2 != null)
                        HandP2.Visibility = Visibility.Visible;
                    else
                        HandP2.Visibility = Visibility.Collapsed;
                }
                else
                {
                    changeP1ItemsVisibilityTo(Visibility.Collapsed);
                    changeP2ItemsVisibilityTo(Visibility.Collapsed);
                    HandP1.Visibility = Visibility.Visible;
                    HandP2.Visibility = Visibility.Collapsed;
                }

            }
            else
            {
                pauseGame1P();
            }

            if(!game_paused)
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


        private int SetEllipsePosition(Ellipse ellipse, Joint joint, bool checkCollisionWithBall, int pID)
        {

            vector.X = ScaleVector(screen_width, joint.Position.X);
            vector.Y = ScaleVector(screen_height, -joint.Position.Y);

            updatedJoint.ID = joint.ID;
            updatedJoint.TrackingState = JointTrackingState.Tracked;
            updatedJoint.Position = vector;

            Canvas.SetLeft(ellipse, updatedJoint.Position.X);
            Canvas.SetTop(ellipse, updatedJoint.Position.Y);

            if (checkCollisionWithBall)
            {
                if (ball.checkJointCollision(updatedJoint.Position.X, updatedJoint.Position.Y, pID))
                {
                    score.Play();
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

        }

        void runtime_VideoFrameReady(object sender, Microsoft.Research.Kinect.Nui.ImageFrameReadyEventArgs e)
        {
            PlanarImage image = e.ImageFrame.Image;

            BitmapSource source = BitmapSource.Create(image.Width, image.Height, 0, 0,
                PixelFormats.Bgr32, null, image.Bits, image.Width * image.BytesPerPixel);

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

            sb = this.FindResource("TransitionAnimation") as Storyboard;
            sb2 = this.FindResource("TransitionAnimation2") as Storyboard;
            Storyboard.SetTarget(sb, from);
            Storyboard.SetTarget(sb2, to);

            deactivateHandlersFor(game_state);
            game_state = new_state;

            // play transition effect
            transition.Play();
            sb.Begin();

            from.Visibility = Visibility.Collapsed;
            to.Visibility = Visibility.Visible;
            sb2.Begin();

            activateHandlersFor(game_state);

            if(game_state == game_states_t.PRACTICE || game_state == game_states_t.PLAY)
                resetBall();

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

            game_paused = true;

            Paused1Player.Visibility = Visibility.Visible;
            changeSkeleton1VisibilityTo(Visibility.Collapsed);
            Ball_2D.Visibility = Visibility.Collapsed;

            if(game_state == game_states_t.PLAY || game_state == game_states_t.PRACTICE)
                HandP1.Visibility = Visibility.Collapsed;
            else
                HandP1.Visibility = Visibility.Visible;

        }
        private void unpauseGame1P()
        {
            game_paused = false;

            Paused1Player.Visibility = Visibility.Collapsed;
            changeSkeleton1VisibilityTo(Visibility.Visible);
            Ball_2D.Visibility = Visibility.Visible;

            if (game_state == game_states_t.PLAY || game_state == game_states_t.PRACTICE)
                HandP1.Visibility = Visibility.Visible;
            else
                HandP1.Visibility = Visibility.Collapsed;

        }
        private void pauseGame2P()
        {
            game_paused = true;

            Paused2Players.Visibility = Visibility.Visible;
            changeSkeleton1VisibilityTo(Visibility.Collapsed);
            changeSkeleton2VisibilityTo(Visibility.Collapsed);
            Ball_2D.Visibility = Visibility.Collapsed;
            Ball2_2D.Visibility = Visibility.Collapsed;

            HandP1.Visibility = Visibility.Collapsed;
            HandP2.Visibility = Visibility.Collapsed;
        }
        private void unpauseGame2P(SkeletonData skeleton2)
        {
            game_paused = false;
            
            Paused2Players.Visibility = Visibility.Collapsed;
            changeSkeleton1VisibilityTo(Visibility.Visible);
            changeSkeleton2VisibilityTo(Visibility.Visible);
            Ball_2D.Visibility = Visibility.Visible;
            Ball2_2D.Visibility = Visibility.Visible;

            HandP1.Visibility = Visibility.Visible;
            if (skeleton2 != null)
                HandP2.Visibility = Visibility.Visible;

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

        private void drawSkeleton1(SkeletonData data)
        {

            int num_joint_collisions = 0;

            num_joint_collisions += SetEllipsePosition(P1J1, data.Joints[JointID.AnkleLeft], true, 1);
            num_joint_collisions += SetEllipsePosition(P1J2, data.Joints[JointID.AnkleRight], true, 1);

            num_joint_collisions += SetEllipsePosition(P1J3, data.Joints[JointID.ElbowLeft], true, 1);
            num_joint_collisions += SetEllipsePosition(P1J4, data.Joints[JointID.ElbowRight], true, 1);

            num_joint_collisions += SetEllipsePosition(P1J5, data.Joints[JointID.FootLeft], true, 1);
            num_joint_collisions += SetEllipsePosition(P1J6, data.Joints[JointID.FootRight], true, 1);

            num_joint_collisions += SetEllipsePosition(P1J7, data.Joints[JointID.HandLeft], true, 1);
            num_joint_collisions += SetEllipsePosition(P1J8, data.Joints[JointID.HandRight], true, 1);

            num_joint_collisions += SetEllipsePosition(P1J9, data.Joints[JointID.Head], true, 1);

            num_joint_collisions += SetEllipsePosition(P1J10, data.Joints[JointID.HipCenter], true, 1);
            num_joint_collisions += SetEllipsePosition(P1J11, data.Joints[JointID.HipLeft], true, 1);
            num_joint_collisions += SetEllipsePosition(P1J12, data.Joints[JointID.HipRight], true, 1);

            num_joint_collisions += SetEllipsePosition(P1J13, data.Joints[JointID.KneeLeft], true, 1);
            num_joint_collisions += SetEllipsePosition(P1J14, data.Joints[JointID.KneeRight], true, 1);

            num_joint_collisions += SetEllipsePosition(P1J15, data.Joints[JointID.ShoulderCenter], true, 1);
            num_joint_collisions += SetEllipsePosition(P1J16, data.Joints[JointID.ShoulderLeft], true, 1);
            num_joint_collisions += SetEllipsePosition(P1J17, data.Joints[JointID.ShoulderRight], true, 1);

            num_joint_collisions += SetEllipsePosition(P1J18, data.Joints[JointID.Spine], true, 1);

            num_joint_collisions += SetEllipsePosition(P1J19, data.Joints[JointID.WristLeft], true, 1);
            num_joint_collisions += SetEllipsePosition(P1J20, data.Joints[JointID.WristRight], true, 1);


            player1_score = player1_score + (gained_points * num_joint_collisions);
            if (num_joint_collisions > 0)
            {
                Console.WriteLine("[P1 Score] Score: {0}", player1_score);
                Console.WriteLine("[Collisions P1] Collided with {0} joints!", num_joint_collisions);
                Console.WriteLine("");
            }

        }

        private void drawSkeleton2(SkeletonData data)
        {

            int num_joint_collisions = 0;

            num_joint_collisions += SetEllipsePosition(P2J1, data.Joints[JointID.AnkleLeft], true, 2);
            num_joint_collisions += SetEllipsePosition(P2J2, data.Joints[JointID.AnkleRight], true, 2);

            num_joint_collisions += SetEllipsePosition(P2J3, data.Joints[JointID.ElbowLeft], true, 2);
            num_joint_collisions += SetEllipsePosition(P2J4, data.Joints[JointID.ElbowRight], true, 2);

            num_joint_collisions += SetEllipsePosition(P2J5, data.Joints[JointID.FootLeft], true, 2);
            num_joint_collisions += SetEllipsePosition(P2J6, data.Joints[JointID.FootRight], true, 2);

            num_joint_collisions += SetEllipsePosition(P2J7, data.Joints[JointID.HandLeft], true, 2);
            num_joint_collisions += SetEllipsePosition(P2J8, data.Joints[JointID.HandRight], true, 2);

            num_joint_collisions += SetEllipsePosition(P2J9, data.Joints[JointID.Head], true, 2);

            num_joint_collisions += SetEllipsePosition(P2J10, data.Joints[JointID.HipCenter], true, 2);
            num_joint_collisions += SetEllipsePosition(P2J11, data.Joints[JointID.HipLeft], true, 2);
            num_joint_collisions += SetEllipsePosition(P2J12, data.Joints[JointID.HipRight], true, 2);

            num_joint_collisions += SetEllipsePosition(P2J13, data.Joints[JointID.KneeLeft], true, 2);
            num_joint_collisions += SetEllipsePosition(P2J14, data.Joints[JointID.KneeRight], true, 2);

            num_joint_collisions += SetEllipsePosition(P2J15, data.Joints[JointID.ShoulderCenter], true, 2);
            num_joint_collisions += SetEllipsePosition(P2J16, data.Joints[JointID.ShoulderLeft], true, 2);
            num_joint_collisions += SetEllipsePosition(P2J17, data.Joints[JointID.ShoulderRight], true, 2);

            num_joint_collisions += SetEllipsePosition(P2J18, data.Joints[JointID.Spine], true, 2);

            num_joint_collisions += SetEllipsePosition(P2J19, data.Joints[JointID.WristLeft], true, 2);
            num_joint_collisions += SetEllipsePosition(P2J20, data.Joints[JointID.WristRight], true, 2);


            player2_score = player2_score + (gained_points * num_joint_collisions);
            if (num_joint_collisions > 0)
            {
                Console.WriteLine("[P2 Score] Score: {0}", player2_score);
                Console.WriteLine("[Collisions P2] Collided with {0} joints!", num_joint_collisions);
                Console.WriteLine("");
            }

        }

    }
}
