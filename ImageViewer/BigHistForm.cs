using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Emgu.CV;
using SPAMI.Util.UI;

using ZedGraph;

namespace SPAMI.Util.EmguImageViewer
{
    public partial class BigHistForm : ToolForm
    {
        private Point FirstLClick = new Point(0, 0);
        private DenseHistogram DenseHist;

        public BigHistForm()
        {
            InitializeComponent();
            histogramBoxLarge.ZedGraphControl.DoubleClick += ZedGraphControl_DoubleClick;
        }

        public void Refresh(DenseHistogram denseHist)
        {
            DenseHist = denseHist;
            if (histogramBoxLarge.InvokeRequired)
                histogramBoxLarge.BeginInvoke(new MethodInvoker(RefreshHist));
            else
                RefreshHist();
        }

        private void RefreshHist()
        {
            histogramBoxLarge.ClearHistogram();
            histogramBoxLarge.AddHistogram("", Color.DarkBlue, DenseHist);
            ((LineItem)histogramBoxLarge.ZedGraphControl.GraphPane.CurveList[0]).Symbol.Type = SymbolType.None;
            histogramBoxLarge.Refresh();
        }

        private void BigHistForm_DoubleClick(object sender, EventArgs e)
        {
            if (this.FormBorderStyle == System.Windows.Forms.FormBorderStyle.SizableToolWindow)
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            else
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
        }

        private void ZedGraphControl_DoubleClick(object sender, EventArgs e)
        {
            if (this.FormBorderStyle == System.Windows.Forms.FormBorderStyle.SizableToolWindow)
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            else
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
        }
    }
}
