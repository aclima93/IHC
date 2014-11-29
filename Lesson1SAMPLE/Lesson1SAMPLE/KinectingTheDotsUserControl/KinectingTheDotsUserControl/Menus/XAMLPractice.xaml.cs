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
        private Ball ball;

        public int renderingCounter = 0;
        public int renderingStep = 10;


        public XAMLPractice()
        {
            InitializeComponent();

            //ball_image.ImageSource = new BitmapImage( new Uri("ball1.png", UriKind.Relative));

            Random r = new Random();

            float dx = r.Next(-1000, 1000)/1000;
            float dy = r.Next(-1000, 1000)/1000;
            float dz = r.Next(-1000, 1000)/1000;

            float radius = 64/2; //hardcoded because fuck you, that's why
            
            int screen_width = 1366;
            int screen_height = 768; 

            // because we use fullscreen. fuck you.
            int window_width = screen_width;
            int window_height = screen_height;

            float x = 0;//r.Next(-window_width/2, window_width/2);
            float y = 0;//r.Next(-window_height / 2, window_height / 2);
            float z = 10; // to be tweeked?

            float distLR = screen_width*10;
            float distUD = screen_height*10;
            float distFB = screen_width*20;

            this.ball = new Ball( x, y, z, dx, dy, dz, radius, screen_width, screen_height, window_width, window_height, distLR, distUD, distFB);
        
        }

        
        public void setBallPosition(Ellipse ellipse, float x, float y)
        {
            Canvas.SetLeft(ellipse, x);
            Canvas.SetTop(ellipse, y);
        }

        public void updateBall()
        {
            ball.updatePosition();
            ball.checkWallCollisions();
            //ball.checkJointCollision();

            renderingCounter += 1;
            if( (renderingCounter%renderingStep) == 0)
            {
                float size = ball.getSize();
                Ball_2D.Height = size;
                Ball_2D.Width = size;
                setBallPosition(Ball_2D, ball.getX2D(), ball.getY2D());
            }
        }


        public void setMainWindow(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        public void setChooseAvatarHandlers()
        {
            // Choose Avatar Handlers
            PracticeReturnToMainMenu.Click += new RoutedEventHandler(PracticeReturnToMainMenu_Click);
        }

        public void removeChooseAvatarHandlers()
        {
            // Choose Avatar Handlers
            PracticeReturnToMainMenu.Click -= new RoutedEventHandler(PracticeReturnToMainMenu_Click);
        }

        public void checkPracticeButtons()
        {
            //mainWindow.CheckButton(PracticeReturnToMainMenu, mainWindow.RightHand);
            mainWindow.CheckButton(PracticeReturnToMainMenu, mainWindow.P1J7);
            mainWindow.CheckButton(PracticeReturnToMainMenu, mainWindow.P1J8);
            mainWindow.CheckButton(PracticeReturnToMainMenu, mainWindow.P2J7);
            mainWindow.CheckButton(PracticeReturnToMainMenu, mainWindow.P2J8);
            updateBall();

        }

        void PracticeReturnToMainMenu_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.transition.Play();

            mainWindow.changeGameState(MainWindow.game_states_t.MAIN_MENU, mainWindow.xamlPractice, mainWindow.xamlMainMenu);

        }
    }
}
