using System;
using System.Numerics;

namespace SA3D.Common
{
    /// <summary>
    /// Math helper functions
    /// </summary>
    public static class MathHelper
    {
        /* These constants have been changed to static readonly, as the APIAnalyzer is unable to verify
         * their correctness.
         * See https://github.com/dotnet/roslyn-analyzers/issues/6747
         * They will be changed back to constants once the issue is resolved
         */

        /// <summary>
        /// Half Pi
        /// </summary>
        public static readonly float HalfPi = float.Pi * 0.5f;

        /// <summary>
        /// BAMS to Degree ratio
        /// </summary>
        public static readonly float BAMS2Deg = 0x10000 / 360f;

        /// <summary>
        /// BAMS to Radians ratio
        /// </summary>
        public static readonly float BAMS2Rad = 0x10000 / float.Tau;

        /// <summary>
        /// Radians to Degree ratio
        /// </summary>
        public static readonly float Rad2Deg = 180.0f / float.Pi;

        /// <summary>
        /// Converts an angle from BAMS to radians.
        /// </summary>
        /// <param name="BAMS"></param>
        /// <returns></returns>
        public static float BAMSToRad(int BAMS)
        {
            return BAMS / BAMS2Rad;
        }

        /// <summary>
        /// Converts an angle from radians to BAMS.
        /// </summary>
        /// <param name="rad"></param>
        /// <returns></returns>
        public static int RadToBAMS(float rad)
        {
            return (int)Math.Round(rad * BAMS2Rad);
        }

        /// <summary>
        /// Converts an angle from BAMS to Degrees.
        /// </summary>
        public static float BAMSToDeg(int BAMS)
        {
            return BAMS / BAMS2Deg;
        }

        /// <summary>
        /// Converts an angle from degrees to BAMS.
        /// </summary>
        public static int DegToBAMS(float deg)
        {
            return (int)Math.Round(deg * BAMS2Deg);
        }

        /// <summary>
        /// Converts an angle from degrees to radians.
        /// </summary>
        public static float DegToRad(float deg)
        {
            return deg / Rad2Deg;
        }

        /// <summary>
        /// Converts an angle from radians to degrees.
        /// </summary>
        public static float RadToDeg(float rad)
        {
            return rad * Rad2Deg;
        }

        /// <summary>
        /// Linear interpolation between two values
        /// </summary>
        /// <param name="from">Value to interpolate from</param>
        /// <param name="to">Value to interpolate to</param>
        /// <param name="time">Interpolation time</param>
        /// <returns></returns>
        public static float Lerp(float from, float to, float time)
        {
            return from + ((to - from) * time);
        }

        /// <summary>
        /// Checks if a signed integer is a power of 2.
        /// </summary>
        /// <param name="number">Number to check.</param>
        /// <returns>Whether the number is a power of 2.</returns>
        public static bool IsPow2(int number)
        {
            return (number & (number - 1)) == 0 && number > 0;
        }

        /// <summary>
        /// Checks if an unsigned integer is a power of 2.
        /// </summary>
        /// <param name="number">Number to check.</param>
        /// <returns>Whether the number is a power of 2.</returns>
        public static bool IsPow2(uint number)
        {
            return (number & (number - 1)) == 0 && number > 0;
        }

        /// <summary>
        /// Rotates a normal around an axis.
        /// </summary>
        /// <param name="axis">The axis normal to rotate around.</param>
        /// <param name="target">The normal to rotate.</param>
        /// <param name="angleRadians">The angle to rotate by in radians.</param>
        /// <returns></returns>
        public static Vector3 RotateNormal(Vector3 axis, Vector3 target, float angleRadians)
        {
            axis = Vector3.Normalize(axis);
            target = Vector3.Normalize(target);

            float sine = MathF.Sin(angleRadians);
            float cosine = MathF.Cos(angleRadians);
            float oneMinusCosine = (1.0f - cosine) * Vector3.Dot(axis, target);

            float Formula(int c, int c1, int c2)
            {
                return (target[c] * cosine)
                + (((target[c1] * axis[c2]) - (target[c2] * axis[c1])) * sine)
                + (axis[c] * oneMinusCosine);
            }

            return Vector3.Normalize(new(
                Formula(0, 1, 2),
                Formula(1, 2, 0),
                Formula(2, 0, 1)
            ));
        }
    }
}