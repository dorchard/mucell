using System;
using System.Collections.Generic;
using System.Text;

namespace MuCell.Model
{
    /// <summary>
    /// Helper class containing a few basic math functions used for distribution
    /// calculations.
    /// </summary>
    public static class MathHelper
    {
        /// <summary>
        /// Returns the square of input x, however it maintains the sign of the input.
        /// For example SignSquare(-5) will return -25.
        /// </summary>
        /// <param name="x">input variable x</param>
        /// <returns>square of input x with the same sign as x</returns>
        public static float SignSquare(float x)
        {
            return (x < 0) ? -x * x : x * x;
        }


        /// <summary>
        /// Returns a random number that is normally distributed in the range.
        /// </summary>
        /// <param name="rand">Random number generator</param>
        /// <returns>normally distributed float in with standard deviation 1 and mean of 0</returns>
        public static double RandBoxMuller(Random rand)
        {
            double a = (double)(rand.Next() % 1000000) / 1000000;
            double b = (double)(rand.Next() % 1000000) / 1000000;

            //avoid log of 0
            if (a == 0)
            {
                return 0;
            }

            return Math.Sqrt(-2.0f * Math.Log(a)) * Math.Cos(2 * Math.PI * b);
        }


        /// <summary>
        /// This function computes the squared distance between two points in 3D space. Note
        /// that is is considerably faster to compute the squared distance than the actual distance,
        /// so this is useful for distance checks that need to be done rapidly
        /// </summary>
        /// <param name="x1">first point's x coord</param>
        /// <param name="y1">first point's y coord</param>
        /// <param name="z1">first point's z coord</param>
        /// <param name="x2">second point's x coord</param>
        /// <param name="y2">second point's y coord</param>
        /// <param name="z2">second point's z coord</param>
        /// <returns></returns>
        public static float SquaredDist(float x1, float y1, float z1,
                                        float x2, float y2, float z2)
        {
            return (x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1) + (z2 - z1) * (z2 - z1);

        }

        /// <summary>
        /// This function computes the squared distance between two points in 2D space. Note
        /// that is is considerably faster to compute the squared distance than the actual distance,
        /// so this is useful for distance checks that need to be done rapidly
        /// </summary>
        /// <param name="x1">first point's x coord</param>
        /// <param name="y1">first point's y coord</param>
        /// <param name="x2">second point's x coord</param>
        /// <param name="y2">second point's y coord</param>
        /// <returns></returns>
        public static float SquaredDist(float x1, float y1, float x2, float y2)
        {
            return (x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1);
        }



    }

}
