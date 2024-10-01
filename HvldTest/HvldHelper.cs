using Hvld.Controls;
using Hvld.Parser;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace HvldTest
{
    /// <summary>
    /// Helper functions used by HVLD 2.0.
    /// </summary>
    internal static class HvldHelper
    {
        /// <summary>
        /// Reads a dump file into a byte array.
        /// Used for debug purposes.
        /// </summary>
        public static byte[] ReadDumpByteArray(string dumpFile, out long timeTaken)
        {
            var watch = Stopwatch.StartNew();
            var bytes = File.ReadAllBytes(dumpFile);
            watch.Stop();
            timeTaken = watch.ElapsedMilliseconds;
            return bytes;
        }
        /// <summary>
        /// Creates an HvldFrame from a byte array.
        /// Used for debug purposes.
        /// </summary>
        public static HvldFrame CreateHvldFrameFromByteArray(byte[] frameData, out long timeTaken)
        {
            var watch = Stopwatch.StartNew();
            var frame = new HvldFrame(frameData);
            watch.Stop();
            timeTaken = watch.ElapsedMilliseconds;
            return frame;
        }
        /// <summary>
        /// Extracts all the OptrelSignals from an HvldFrame and updates the HvldDisplayControl passed as parameter.
        /// </summary>
        public static void LoadSignalsFromHvldFrame(HvldFrame frame, HvldDisplayControl controlToUpdate, out long timeTaken)
        {
            timeTaken = 0;
            if (controlToUpdate is null || frame is null)
                return;

            // Creates the settings to tweak the appearance of the control.
            var hvldSettings = new HvldControlUpdateData()
            {
                XAxisTitle = "Sample Count",
                YAxisTitle = "Measure",
                BorderSize = 5,
                ShowSignalBoundingLines = true,
                ScientificNotationOnYAxis = false,
                InSpecBorderColor = Color.Green,
                OutSpecBorderColor = Color.Red,
                SignalColorOverride = null
            };

            var watch = Stopwatch.StartNew();
            // Adds the signal to the control.
            // MM-08/02/2024: email request by virginia t.: x-axis title should be 'sample count'
            controlToUpdate.Invoke(new MethodInvoker(() =>
            {
                // Adds the signal to the control.
                // MM-08/02/2024: email request by virginia t.: x-axis title should be 'sample count'
                controlToUpdate.UpdateControlFromHvldFrame(frame, hvldSettings);
            }));
            watch.Stop();
            timeTaken = watch.ElapsedMilliseconds;
        }
        /// <summary>
        /// Gets a screen of the currentlydiplay signal and puts it on a PictureBox.
        /// </summary>
        public static void ShotHvldIntoPictureBox(HvldDisplayControl source, PictureBox destination, out long timeTaken)
        {
            timeTaken = 0;
            if (source is null || destination is null)
                return;

            var watch = Stopwatch.StartNew();
            Bitmap bmp = null;
            // Creates an image of the currently shown graph for the MAIN area.
            source.Invoke(new MethodInvoker(() =>
            {
                try
                {
                    // Gets an image from the control.
                    using (var currentGraph = source.GetImage())
                    {
                        // Null-check on the image object.
                        if (!(currentGraph is null))
                        {
                            // Creates a new OpenCV image from the Bitmap.
                            using (var openCvImage = new Emgu.CV.Image<Emgu.CV.Structure.Rgb, byte>(currentGraph))
                                bmp = openCvImage.ToBitmap();
                        }
                    }
                }
                catch { }
            }));

            destination.Invoke(new MethodInvoker(() =>
            {
                try
                {
                    if (destination.Image != null)
                        destination.Image.Dispose();
                    destination.Image = bmp;
                }
                catch { }
            }));
            watch.Stop();
            timeTaken = watch.ElapsedMilliseconds;
        }
    }
}
