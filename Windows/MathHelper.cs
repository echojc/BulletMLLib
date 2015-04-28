using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulletMLLib
{
    internal static class MathHelper
    {
        public static float WrapAngle(float angle)
        {
            double res = Math.IEEERemainder(angle, 2 * Math.PI);
            if (res > Math.PI)
                return (float)(res - (2 * Math.PI));
            else if (res <= -Math.PI)
                return (float)(res + (2 * Math.PI));
            else
                return (float)res;
        }
    }
}
