using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectingTheDotsUserControl
{
    class Ball
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
        private float left;
        private float right;
        private float up;
        private float down;
        private float front;
        private float back;


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

            this.left = distLR;
            this.right = distLR;

            this.up = distUD;
            this.down = distUD;

            this.front = distFB;
            this.back = distFB;

        }

        private float getDrawingRatio()
        {
            float temp1_x_2D = ((x_3D - radius) / z_3D) + half_screen_width;
            float temp2_x_2D = ((x_3D + radius) / z_3D) + half_screen_width;

            if (window_aspect > 1.0)
            {
                temp1_x_2D = temp1_x_2D / window_aspect;
                temp2_x_2D = temp2_x_2D / window_aspect;
            }

            // because it's a sphere, the ratio is the same in every direction
            // ratio = newdist / olddist
            return ( (Math.Abs(temp1_x_2D) + Math.Abs(temp2_x_2D)) / dist);
        }


        // -----------------
        // helping functions
        // -----------------

        private void update2DCoordinates()
        {
            x_2D = (x_3D / z_3D) + half_screen_width;
            y_2D = (-1*(y_3D / z_3D)) + half_screen_height;

            
            if (window_aspect > 1.0)
            {
                x_2D = x_2D / window_aspect;
            }
            else
            {
                y_2D = y_2D * window_aspect;
            }
            
        }

        private void update3DCoordinates()
        {
            x_3D += dx_3D;
            y_3D += dy_3D;
            z_3D += dz_3D;
        }

        private void updatePosition()
        {
            update3DCoordinates();
            update2DCoordinates();
        }

        private void xAxisRicochet()
        {
            dx_3D *= (-1);
        }
        private void yAxisRicochet()
        {
            dy_3D *= (-1);
        }
        private void zAxisRicochet()
        {
            dz_3D *= (-1);
        }

        private bool checkJointCollision(float joint_x, float joint_y, float joint_z)
        {

            bool leftHit = ( (x_3D - radius) >= joint_x );
            bool rightHit = ( joint_x <= (x_3D + radius) );

            bool downHit = ( (y_3D - radius) >= joint_y );
            bool upHit = ( joint_y <= (y_3D + radius) );

            if( leftHit || rightHit )
            {
                if( downHit || upHit )
                {
                    if( (z_3D - radius) >= joint_z || joint_z <= (z_3D + radius) )
                    {
                        
                        if( (leftHit && (dx_3D < 0) ) || (rightHit && (dx_3D > 0) ) )
                        {
                            xAxisRicochet();
                        }


                        if( (downHit && (dy_3D < 0) ) || (upHit && (dx_3D > 0) ) )
                        {
                            yAxisRicochet();
                        }

                        //collision so just reverse dz of ball
                        zAxisRicochet();

                        updatePosition();

                        return true;
                    }
                }
            }

            return false;
        }

        private void checkLeftRightWallCollisions()
        {
            if( (x_3D - radius) <= left || (x_3D + radius) >= right )
            {
                xAxisRicochet();
                updatePosition(); 
            }
        }

        private void checkUpDownCollisions()
        {
            if( (y_3D - radius) <= down || (y_3D + radius) >= up)
            {
                yAxisRicochet();
                updatePosition();
            }
        }


        private void checkFrontCollision()
        {
            if ((z_3D + radius) >= front)
            {
                zAxisRicochet();
                updatePosition();
            }
        }

        private bool checkBackCollision()
        {
            if ((z_3D - radius) <= back)
            {
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

        private bool checkWallCollisions()
        {
            checkLeftRightWallCollisions();
            checkUpDownCollisions();

            return checkFrontBackCollisions();
        }

    }
}
