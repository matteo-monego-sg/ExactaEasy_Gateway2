using System;
using System.Collections.Generic;
using System.Linq;
using ZedGraph;

namespace Hvld.Parser
{
    /// <summary>
    /// 
    /// </summary>
    internal static class HvldSignalTransformer
    {
        /// <summary>
        /// 
        /// </summary>
        public static PointPairList ApplySimpleClamping(
            PointPairList points, 
            ClampingSide side, 
            double? lowerBound, 
            double? upperBound)
        {
            // Null-empty check.
            if (points is null || points.Count == 0)
                return points;

            var dataPoints = points.ToPoint2DList();
            var clampCount = 0;

            // Params consistency check.
            switch (side)
            {
                case ClampingSide.Both:
                    {
                        if (!lowerBound.HasValue && !upperBound.HasValue)
                            return points;

                        for (var i = 0; i < dataPoints.Count; i++)
                        {
                            if (dataPoints[i].Y > upperBound.Value)
                            {
                                dataPoints[i].UpdateY(upperBound.Value);
                                clampCount++;
                            }

                            if (dataPoints[i].Y < lowerBound.Value)
                            {
                                dataPoints[i].UpdateY(lowerBound.Value);
                                clampCount++;
                            }
                        }
                    }
                    break;

                case ClampingSide.Lower:
                    {
                        if (!lowerBound.HasValue)
                            return points;

                        for (var i = 0; i < dataPoints.Count; i++)
                        {
                            if (dataPoints[i].Y < lowerBound.Value)
                            {
                                dataPoints[i].UpdateY(lowerBound.Value);
                                clampCount++;
                            }
                        }
                    }
                    break;

                case ClampingSide.Upper:
                    {
                        if (!upperBound.HasValue)
                            return points;

                        for (var i = 0; i < dataPoints.Count; i++)
                        {
                            if (dataPoints[i].Y < upperBound.Value)
                            {
                                dataPoints[i].UpdateY(upperBound.Value);
                                clampCount++;
                            }
                        }
                    }
                    break;
            }
            // If all the points are on the threshold line, then this means the signal has been filtered out!
            if (clampCount.Equals(dataPoints.Count))
                return new PointPairList();
            else
                return dataPoints.ToPointPairList();
        }
        /// <summary>
        /// Applies the accurate min/max threshold clamping of the Y values of the signal,
        /// by calculating the intersections of every single segment ogf the signal with
        /// the thresholds asyntotes.
        /// Can be time consuming (surely more than the simple version).
        /// </summary>
        public static PointPairList ApplyAccurateClamping(
            PointPairList points,
            ClampingSide side, 
            double? lowerBound, 
            double? upperBound)
        {
            // Null-empty check.
            if (points is null || points.Count == 0) 
                return points;

            var dataPoints = points.ToPoint2DList();
            //var dataPoints = points.ToArray();
            var xMin = points.Min(p => p.X);
            var xMax = points.Max(p => p.X);

            var pointsToAdd = new List<Point2D>();

            switch (side)
            {
                case ClampingSide.Both:
                    {
                        if (!lowerBound.HasValue && !upperBound.HasValue)
                            return points;

                        var yMaxThresholdSegmentStart = new Point2D(xMin, upperBound.Value);
                        var yMaxThresholdSegmentEnd = new Point2D(xMax, upperBound.Value);

                        var yMinThresholdSegmentStart = new Point2D(xMin, lowerBound.Value);
                        var yMinThresholdSegmentEnd = new Point2D(xMax, lowerBound.Value);

                        for (var i = 0; i < dataPoints.Count - 1; i++)
                        {
                            if (LineSegmentsIntersect(
                                dataPoints[i],
                                dataPoints[i + 1],
                                yMaxThresholdSegmentStart,
                                yMaxThresholdSegmentEnd,
                                out Point2D upperIntersectionPoint, 
                                false))
                            {
                                pointsToAdd.Add(upperIntersectionPoint);
                            }
                           
                            if (LineSegmentsIntersect(
                                dataPoints[i],
                                dataPoints[i + 1],
                                yMinThresholdSegmentStart,
                                yMinThresholdSegmentEnd,
                                out Point2D lowerIntersectionPoint,
                                false))
                            {
                                pointsToAdd.Add(lowerIntersectionPoint);
                            }
                        }
                        var tmp = dataPoints.ToList();
                        // Removes all the points out of the boundaries.
                        tmp.RemoveAll(p => p.Y > upperBound.Value || p.Y < lowerBound.Value);
                        // Add all the intersection points previously calculated.
                        tmp.AddRange(pointsToAdd);
                        // Orders all the points by the X coordinate.
                        return tmp.OrderBy(p => p.X).ToList().ToPointPairList();
                    }

                case ClampingSide.Lower:
                    {
                        if (!lowerBound.HasValue)
                            return points;

                        var yMinThresholdSegmentStart = new Point2D(xMin, lowerBound.Value);
                        var yMinThresholdSegmentEnd = new Point2D(xMax, lowerBound.Value);

                        for (var i = 0; i < dataPoints.Count - 1; i++)
                        {
                            if (LineSegmentsIntersect(
                                    dataPoints[i],
                                    dataPoints[i + 1],
                                    yMinThresholdSegmentStart,
                                    yMinThresholdSegmentEnd,
                                    out Point2D lowerIntersection, 
                                    false))
                            {
                                pointsToAdd.Insert(i + 1, lowerIntersection);
                            }
                        }
                        var tmp = dataPoints.ToList();
                        // Removes all the points below the lower threshold.
                        tmp.RemoveAll(p => p.Y < lowerBound.Value);
                        // Add all the intersection points previously calculated.
                        tmp.AddRange(pointsToAdd);
                        // Orders all the points by the x coordinate.
                        return tmp.OrderBy(p => p.X).ToList().ToPointPairList();
                    }

                case ClampingSide.Upper:
                    {
                        if (!upperBound.HasValue)
                            return points;

                        var yMaxThresholdSegmentStart = new Point2D(xMin, upperBound.Value);
                        var yMaxThresholdSegmentEnd = new Point2D(xMax, upperBound.Value);

                        for (var i = 0; i < points.Count - 1; i++)
                        {
                            if (LineSegmentsIntersect(
                                dataPoints[i],
                                dataPoints[i + 1],
                                yMaxThresholdSegmentStart,
                                yMaxThresholdSegmentEnd,
                                out Point2D upperIntersection, 
                                false))
                            {
                                pointsToAdd.Insert(i + 1, upperIntersection);
                            }
                        }
                        var tmp = dataPoints.ToList();
                        // Removes all the points above the upper threshold.
                        tmp.RemoveAll(p => p.Y > upperBound.Value);
                        // Orders all the points by the x coordinate.
                        return tmp.OrderBy(p => p.X).ToList().ToPointPairList();
                    }
            }
            return points;
        }
        /// <summary>
        /// Calculates the coordinate of the intersection between two segments. 
        /// https://www.codeproject.com/Tips/862988/Find-the-Intersection-Point-of-Two-Line-Segments
        /// </summary>
        public static bool LineSegmentsIntersect(
            Point2D p, 
            Point2D p2, 
            Point2D q, 
            Point2D q2, 
            out Point2D intersection, 
            bool considerCollinearOverlapAsIntersect = false)
        {
            intersection = new Point2D();

            var r = p2 - p;
            var s = q2 - q;
            var rxs = r.Cross(s);
            var qpxr = (q - p).Cross(r);
            // If r x s = 0 and (q - p) x r = 0, then the two lines are collinear.
            if (rxs.IsZero() && qpxr.IsZero())
            {
                // 1. If either  0 <= (q - p) * r <= r * r or 0 <= (p - q) * s <= * s
                // then the two lines are overlapping,
                if (considerCollinearOverlapAsIntersect)
                    if ((0 <= (q - p) * r && (q - p) * r <= r * r) || (0 <= (p - q) * s && (p - q) * s <= s * s))
                        return true;
                // 2. If neither 0 <= (q - p) * r = r * r nor 0 <= (p - q) * s <= s * s
                // then the two lines are collinear but disjoint.
                // No need to implement this expression, as it follows from the expression above.
                return false;
            }
            // 3. If r x s = 0 and (q - p) x r != 0, then the two lines are parallel and non-intersecting.
            if (rxs.IsZero() && !qpxr.IsZero())
                return false;
            // t = (q - p) x s / (r x s)
            var t = (q - p).Cross(s) / rxs;
            // u = (q - p) x r / (r x s)
            var u = (q - p).Cross(r) / rxs;
            // 4. If r x s != 0 and 0 <= t <= 1 and 0 <= u <= 1
            // the two line segments meet at the point p + t r = q + u s.
            if (!rxs.IsZero() && (0 <= t && t <= 1) && (0 <= u && u <= 1))
            {
                // We can calculate the intersection point using either t or u.
                intersection = p + t * r;
                // An intersection was found.
                return true;
            }
            // 5. Otherwise, the two line segments are not parallel but do not intersect.
            return false;
        }
    }
}
