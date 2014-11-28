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
    /// Interaction logic for XAMLPractice.xaml
    /// </summary>
    public partial class XAMLPractice : UserControl
    {

        private MainWindow mainWindow;
        private Ball ball;


        public XAMLPractice()
        {
            InitializeComponent();
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

            float x = r.Next(-window_width/2, window_width/2);
            float y = r.Next(-window_height / 2, window_height / 2);
            float z = 10; // to be tweeked?

            float distLR = screen_width*10;
            float distUD = screen_height*10;
            float distFB = screen_width*20;

            ball = new Ball( x, y, z, dx, dy, dz, radius, screen_width, screen_height, window_width, window_height, distLR, distUD, distFB);
        
        }

        meter aqui a correr a bola e o seu "significado"

        private void checkPracticeButtons(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
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
            mainWindow.CheckButton(PracticeReturnToMainMenu, mainWindow.RightHand);

        }

        void PracticeReturnToMainMenu_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.transition.Play();

            mainWindow.changeGameState(MainWindow.game_states_t.MAIN_MENU, mainWindow.xamlPractice, mainWindow.xamlMainMenu);

        }
    }
}
