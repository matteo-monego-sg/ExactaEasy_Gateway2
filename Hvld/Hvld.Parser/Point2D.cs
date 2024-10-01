namespace Hvld.Parser
{
    /// <summary>
    /// A 2D plot point.
    /// </summary>
    public struct Point2D
    {
        /// <summary>
        /// 
        /// </summary>
        public Point2D(double x, double y)
        {
            X = x;
            Y = y;
        }
        /// <summary>
        /// X-coordinate.
        /// </summary>
        public double X { get; private set; }
        /// <summary>
        /// Y-coordinate.
        /// </summary>
        public double Y { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public void Update(double x, double y)
        {
            X = x;
            Y = y;
        }
        /// <summary>
        /// 
        /// </summary>
        public void UpdateX(double x)
        {
            X = x;
        }
        /// <summary>
        /// 
        /// </summary>
        public void UpdateY(double y)
        {
            Y = y;
        }
        /// <summary>
        /// 
        /// </summary>
        public static Point2D operator -(Point2D v, Point2D w)
        {
            return new Point2D(v.X - w.X, v.Y - w.Y);
        }
        /// <summary>
        /// 
        /// </summary>
        public static Point2D operator +(Point2D v, Point2D w)
        {
            return new Point2D(v.X + w.X, v.Y + w.Y);
        }
        /// <summary>
        /// 
        /// </summary>
        public static double operator *(Point2D v, Point2D w)
        {
            return v.X * w.X + v.Y * w.Y;
        }
        /// <summary>
        /// 
        /// </summary>
        public static Point2D operator *(Point2D v, double mult)
        {
            return new Point2D(v.X * mult, v.Y * mult);
        }
        /// <summary>
        /// 
        /// </summary>
        public static Point2D operator *(double mult, Point2D v)
        {
            return new Point2D(v.X * mult, v.Y * mult);
        }
        /// <summary>
        /// 
        /// </summary>
        public double Cross(Point2D v)
        {
            return X * v.Y - Y * v.X;
        }
        /// <summary>
        /// 
        /// </summary>
        public override bool Equals(object obj)
        {
            var v = (Point2D)obj;
            return (X - v.X).IsZero() && (Y - v.Y).IsZero();
        }
        /// <summary>
        /// 
        /// </summary>
        public override int GetHashCode()
        {
            // TODO: correct??
            return GetHashCode();
        }
    }
}
