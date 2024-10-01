using System;
using System.Collections.Generic;
using System.Linq;
using ZedGraph;

namespace Hvld.Parser
{
    /// <summary>
    /// 
    /// </summary>
    internal static class ExtensionMethods
    {
        /// <summary>
        /// 
        /// </summary>
        private const double ERROR_EPSILON = 1e-10;
        /// <summary>
        /// 
        /// </summary>
        public static IList<Point2D> ToPoint2DList(this PointPairList ppl)
        {
            return ppl.Select(pp => new Point2D(pp.X, pp.Y)).ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        public static PointPairList ToPointPairList(this IList<Point2D> pts)
        {
            return new PointPairList(pts.Select(p => p.X).ToArray(), pts.Select(p => p.Y).ToArray());
        }
        /// <summary>
        /// 
        /// </summary>
        public static bool IsZero(this double d)
        {
            return Math.Abs(d) < ERROR_EPSILON;
        }
        /// <summary>
        /// 
        /// </summary>
        public static T[] SubArray<T>(this T[] array, int offset, int length)
        {
            T[] result = new T[length];
            Array.Copy(array, offset, result, 0, length);
            return result;
        }
    }
}
