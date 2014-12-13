using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectingTheDotsUserControl
{
    public class Ball
    {
        private float radius;
        private int size;
        private float dist;

        private float x_3D;
        private float y_3D;
        private float z_3D;

        private float dx_3D;
        private float dy_3D;
        private float dz_3D;

        private float x_2D;
        private float y_2D;

        private float half_screen_width;
        private float half_screen_height;

        private float window_aspect;

        // assuming http://www.povray.org/documentation/images/tutorial/handed.png coordinate system
        private int leftWallPlane;
        private int rightWallPlane;
        private int upWallPlane;
        private int downWallPlane;
        private int frontWallPlane;
        private int backWallPlane;


        public Ball(float x, float y, float z,
            float dx, float dy, float dz,             
            float radius, int size,
            int screen_width, int screen_height, 
            int window_width, int window_height,
            float distLR, float distUD, float distFB)
        {
            this.x_3D = x;
            this.y_3D = y;
            this.z_3D = z;

            this.radius = radius;
            this.size = size;
            this.dist = 2 * radius;

            this.dx_3D = dx;
            this.dy_3D = dy;
            this.dz_3D = dz;

            this.half_screen_width = screen_width / 2;
            this.half_screen_height = screen_height / 2;

            this.window_aspect = window_width / window_height;

            this.leftWallPlane = 0;//(int)(-distLR/2);
            this.rightWallPlane = (int)(distLR);//(int)(distLR/2);

            this.upWallPlane = 0;//(int)(distUD/2);
            this.downWallPlane = (int)(distUD);//(int)(-distUD/2);

            this.frontWallPlane = (int)(distFB+1);
            this.backWallPlane = 1;

        }

        public float getX2D()
        {
            return this.x_2D;
        }

        public float getY2D()
        {
            return this.y_2D;
        }

        public int getSize()
        {
            return (int)(Math.Abs(size * getDrawingRatio()));
        }

        private float getDrawingRatio()
        {

            // como um pedreiro de verdade, mas não quero saber. if it works don't fuck with it!
            return 1 - (this.z_3D / this.frontWallPlane);

            /*
            float temp1_x_2D = (((this.x_3D - this.radius) / this.z_3D) + this.half_screen_width) * this.half_screen_width;
            float temp2_x_2D = (((this.x_3D + this.radius) / this.z_3D) + this.half_screen_width) * this.half_screen_width;

            
            if (this.window_aspect > 1.0)
            {
                temp1_x_2D = temp1_x_2D / this.window_aspect;
                temp2_x_2D = temp2_x_2D / this.window_aspect;
            }
            

            // because it's a sphere, the ratio is the same in every direction
            // // fully aware that it isn't a linear transformation and that the new distances might mean nothing. but fuck it!
            return dist / (Math.Abs(temp1_x_2D) + Math.Abs(temp2_x_2D));
            */

        }

        public void updatePosition()
        {

            //Console.WriteLine("[PRE] Ball at: x={0} y={1}, size={2}", getX2D(), getY2D(), getSize());
            update3DCoordinates();
            update2DCoordinates();
            //Console.WriteLine("[POS] Ball at: x={0} y={1}, size={2}", getX2D(), getY2D(), getSize());
            Console.WriteLine("[3D] Ball at: x={0} y={1}, z={2}", x_3D, y_3D, z_3D);
            Console.WriteLine("[2D] Ball at: x={0} y={1}, size={2}", x_2D, y_2D, getSize());
            Console.WriteLine("");
            Console.WriteLine("");


        }

        public bool checkJointCollision(float joint_x, float joint_y)
        {

            float joint_z = backWallPlane +1 ;

            bool leftHit = ((this.x_3D - this.radius) >= joint_x);
            bool rightHit = (joint_x <= (this.x_3D + this.radius));

            bool downHit = ((this.y_3D - this.radius) >= joint_y);
            bool upHit = (joint_y <= (this.y_3D + this.radius));

            if (leftHit || rightHit)
            {
                if (downHit || upHit)
                {
                    if ((this.z_3D - this.radius) >= joint_z || joint_z <= (this.z_3D + this.radius))
                    {


                        if (leftHit )//&& (this.dx_3D < 0))
                        {
                            Console.WriteLine("[LeftHit]");
                            xAxisRicochet();
                        }
                        else if (rightHit )//&& (this.dx_3D > 0))
                        {
                            Console.WriteLine("[RightHit]");
                            xAxisRicochet();
                        }


                        if (downHit )//&& (this.dy_3D < 0))
                        {
                            Console.WriteLine("[DownHit]");
                            yAxisRicochet();
                        }
                        else if (upHit )//&& (this.dx_3D > 0))
                        {
                            Console.WriteLine("[UpHit]");
                            yAxisRicochet();
                        }

                        //collision so just reverse dz of ball
                        zAxisRicochet();

                        updatePosition();

                        Console.WriteLine("[Collision] Hit joint at: x={0} y={1}, z={2}", joint_x, joint_y, joint_z);
                        return true;
                    }
                }
            }

            return false;
        }

        public bool checkWallCollisions()
        {
            checkLeftRightWallCollisions();
            checkUpDownCollisions();

            return checkFrontBackCollisions();
        }


        // -----------------
        // helping functions
        // -----------------

        private void update2DCoordinates()
        {
            
            this.x_2D = (this.x_3D / this.z_3D) + this.half_screen_width;
            this.y_2D = (-1 * (this.y_3D / this.z_3D)) + this.half_screen_height;
            
            /*
            this.x_2D = ((this.x_3D / this.z_3D) + this.half_screen_width) * this.half_screen_width;
            this.y_2D = ((-1 * (this.y_3D / this.z_3D)) + this.half_screen_height) * this.half_screen_height;
            */


            if (this.window_aspect > 1.0)
            {
                this.x_2D = this.x_2D / this.window_aspect;
            }
            else
            {
                this.y_2D = this.y_2D * this.window_aspect;
            }
            
        }

        private void update3DCoordinates()
        {
            this.x_3D = this.x_3D + this.dx_3D;
            this.y_3D = this.y_3D + this.dy_3D;
            this.z_3D = this.z_3D + this.dz_3D;
        }

        private void xAxisRicochet()
        {
            this.dx_3D = this.dx_3D * (-1);
        }
        private void yAxisRicochet()
        {
            this.dy_3D = this.dy_3D * (-1);
        }
        private void zAxisRicochet()
        {
            this.dz_3D = this.dz_3D * (-1);
        }

        private void checkLeftRightWallCollisions()
        {
            if ((this.x_3D - this.radius) <= this.leftWallPlane )
            {
                xAxisRicochet();
                updatePosition();
                Console.WriteLine("[Collision] Left Wall Collision");
            }
            if((this.x_3D + this.radius) >= this.rightWallPlane)
            {
                xAxisRicochet();
                updatePosition();
                Console.WriteLine("[Collision] Right Wall Collision");
            }
        }

        private void checkUpDownCollisions()
        {
            if ((this.y_3D - this.radius) <= this.downWallPlane )
            {
                yAxisRicochet();
                updatePosition();
                Console.WriteLine("[Collision] Down Wall Collision");
            }    
            if( (this.y_3D + this.radius) >= this.upWallPlane)
            {
                yAxisRicochet();
                updatePosition();
                Console.WriteLine("[Collision] Up Wall Collision");
            }
        }


        private void checkFrontCollision()
        {
            if ((this.z_3D + this.radius) >= this.frontWallPlane)
            {
                zAxisRicochet();
                updatePosition();
                Console.WriteLine("[Collision] Front Wall Collision");
            }
        }

        private bool checkBackCollision()
        {
            if ((this.z_3D - this.radius) <= this.backWallPlane)
            //if (this.z_3D <= this.backWallPlane) // the radius was messing the initial position because it would always be colliding
            {
                zAxisRicochet();
                updatePosition();
                Console.WriteLine("[Collision] Back Wall Collision");
                return true;
            }
            return false;
        }


        // if it passes the backWallPlane wall deduce some points and reset ball
        private bool checkFrontBackCollisions()
        {

            checkFrontCollision();
            return checkBackCollision(); // deduce points and reset ball

        }

    }
}
