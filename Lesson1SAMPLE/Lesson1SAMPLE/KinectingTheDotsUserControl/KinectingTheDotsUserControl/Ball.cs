using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectingTheDotsUserControl
{
    public class Ball
    {
        private float radius;
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
        private int left;
        private int right;
        private int up;
        private int down;
        private int front;
        private int back;


        public Ball(float x, float y, float z,
            float dx, float dy, float dz,             
            float radius, 
            int screen_width, int screen_height, 
            int window_width, int window_height,
            float distLR, float distUD, float distFB)
        {
            this.x_3D = x;
            this.y_3D = y;
            this.z_3D = z;

            this.radius = radius;
            this.dist = 2 * radius;

            this.dx_3D = dx;
            this.dy_3D = dy;
            this.dz_3D = dz;

            this.half_screen_width = screen_width / 2;
            this.half_screen_height = screen_height / 2;

            this.window_aspect = window_width / window_height;

            this.left = (int)(-distLR/2);
            this.right = (int)(distLR/2);

            this.up = (int)(distUD/2);
            this.down = (int)(-distUD/2);

            this.front = (int)(distFB+1);
            this.back = 1;

        }

        public float getX2D()
        {
            return this.x_2D;
        }

        public float getY2D()
        {
            return this.y_2D;
        }

        public float getSize()
        {
            return radius * 2 * getDrawingRatio();
        }

        private float getDrawingRatio()
        {

            return 1 - (this.z_3D / this.front);

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
            Console.WriteLine("[2D] Ball at: x={0} y={1}, size={2}", x_2D, y_2D, getSize());
            Console.WriteLine("[3D] Ball at: x={0} y={1}, z={2}", x_3D, y_3D, z_3D);
            Console.WriteLine("");
            Console.WriteLine("");


        }

        public bool checkJointCollision(float joint_x, float joint_y, float joint_z)
        {

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

                        if ((leftHit && (this.dx_3D < 0)) || (rightHit && (this.dx_3D > 0)))
                        {
                            xAxisRicochet();
                        }


                        if ((downHit && (this.dy_3D < 0)) || (upHit && (this.dx_3D > 0)))
                        {
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
             * */


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
            if ((this.x_3D - this.radius) <= this.left || (this.x_3D + this.radius) >= this.right)
            {
                xAxisRicochet();
                updatePosition();
                Console.WriteLine("[Collision] Left/Right Wall Collision");
            }
        }

        private void checkUpDownCollisions()
        {
            if ((this.y_3D - this.radius) <= this.down || (this.y_3D + this.radius) >= this.up)
            {
                yAxisRicochet();
                updatePosition();
                Console.WriteLine("[Collision] Up or Down Wall Collision");
            }
        }


        private void checkFrontCollision()
        {
            if ((this.z_3D + this.radius) >= this.front)
            {
                zAxisRicochet();
                updatePosition();
                Console.WriteLine("[Collision] Front Wall Collision");
            }
        }

        private bool checkBackCollision()
        {
            if ((this.z_3D - this.radius) <= this.back)
            //if (this.z_3D <= this.back) // the radius was messing the initial position because it would always be colliding
            {
                zAxisRicochet();
                updatePosition();
                Console.WriteLine("[Collision] Back Wall Collision");
                return true;
            }
            return false;
        }


        // if it passes the back wall deduce some points and reset ball
        private bool checkFrontBackCollisions()
        {

            checkFrontCollision();
            return checkBackCollision(); // deduce points and reset ball

        }

    }
}
