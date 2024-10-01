using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace ExactaEasy {
    public class AppContext {

        public string CultureCode { get; set; }
        public Rectangle AppClientRect { get; set; }
        public int UserLevel { get; set; }

        AppContext() {

            CultureCode = "";
            AppClientRect = new Rectangle(Screen.PrimaryScreen.Bounds.Location, Screen.PrimaryScreen.Bounds.Size);
            UserLevel = 0;
        }
    }
}
