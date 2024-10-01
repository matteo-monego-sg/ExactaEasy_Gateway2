using System.Drawing;

namespace DisplayManager
{
    internal class ImagePositionInfo
    {

        public int RowIndex { get; set; }
        public int ColIndex { get; set; }
        public int PageNumber { get; set; }
        public RectangleF ImagePosition { get; set; }
        public PointF HeaderAbsPosition { get; set; }
        public PointF HeaderRelPosition { get; set; }
        public bool Dirty { get; set; }
        public IStation Station { get; set; }
    }
}
