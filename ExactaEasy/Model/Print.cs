using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using Microsoft.Reporting.WinForms;
using System.Windows.Forms;
using ExactaEasyEng;
using System.Threading.Tasks;

namespace ExactaEasy
{
    class Print : IDisposable
    {
        private IList<Stream> m_streams;
        private int m_currentPageIndex;

        //public events
        public event EventHandler PrintingMexDialog;  //the object sender is the string key of message
        public event EventHandler StartPrint;
        public event EventHandler EndPrint;

        SupervisorModeEnum memStatus = AppEngine.Current.CurrentContext.SupervisorMode;

        public async Task Print_Report(LocalReport report, int Type)//Type case 1: Print   case 2: Export PDF
        {
            if (Type == 1)
            {
                // Genera e stampa il report.
                //Before start check if exist any printer 
                PrintDocument printDoc = new PrintDocument();
                if (printDoc.PrinterSettings.IsValid == false)
                {
                    //no print, exit from method
                    PrintingMexDialog("NoDefaultPrinter", EventArgs.Empty);
                    return;
                }

                StartPrint(null, null);
                PrintingMexDialog("Printing", null);

                //try to set supervisor in busy mode
                AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.Busy);

                Exporting(report);
                Printing();
            }
            if (Type == 2)
            {
                await ExportPDF(report);
            }
        }


        // Export the given report as an EMF (Enhanced Metafile) file.
        private void Exporting(LocalReport report)
        {
            string deviceInfo =
              @"<DeviceInfo>
                <OutputFormat>EMF</OutputFormat>
                <PageWidth>8.5in</PageWidth>
                <PageHeight>11in</PageHeight>
                <MarginTop>0.25in</MarginTop>
                <MarginLeft>0.25in</MarginLeft>
                <MarginRight>0.25in</MarginRight>
                <MarginBottom>0.25in</MarginBottom>
            </DeviceInfo>";
            Warning[] warnings;
            m_streams = new List<Stream>();

            report.Render("Image", deviceInfo, CreateStream, out warnings);

            foreach (Stream stream in m_streams)
            {
                stream.Position = 0;
            }
        }


        // Routine to provide to the report renderer, in order to save an image for each page of the report.
        private Stream CreateStream(string name, string fileNameExtension, Encoding encoding, string mimeType, bool willSeek)
        {
            Stream stream = new MemoryStream();
            m_streams.Add(stream);
            return stream;
        }

        private void Printing()
        {
            if (m_streams == null || m_streams.Count == 0)
            {
                //throw new Exception("Error: no stream to print.");
                return;
            }

            PrintDocument printDoc = new PrintDocument();

            if (!printDoc.PrinterSettings.IsValid)
            {
                //throw new Exception("Error: cannot find the default printer.");
                //MessageBox.Show("Error: cannot find the default printer.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //return;
                PrintingMexDialog("NoDefaultPrinter", EventArgs.Empty);
                return;
            }
            else
            {
                printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
                printDoc.EndPrint += new PrintEventHandler(PrintEnd);
                m_currentPageIndex = 0;
                printDoc.Print();
            }
        }

        // Handler for PrintPageEvents
        private void PrintPage(object sender, PrintPageEventArgs ev)
        {
            Metafile pageImage = new
               Metafile(m_streams[m_currentPageIndex]);

            // Adjust rectangular area with printer margins.
            //Microsoft.ReportingServices.ReportRendering.Rectangle adjustedRect = new Rectangle(
            Rectangle adjustedRect = new Rectangle(
                 ev.PageBounds.Left - (int)ev.PageSettings.HardMarginX,
                 ev.PageBounds.Top - (int)ev.PageSettings.HardMarginY,
                 ev.PageBounds.Width,
                 ev.PageBounds.Height);

            // Draw a white background for the report
            ev.Graphics.FillRectangle(Brushes.White, adjustedRect);

            // Draw the report content
            ev.Graphics.DrawImage(pageImage, adjustedRect);

            // Prepare for the next page. Make sure we haven't hit the end.
            m_currentPageIndex++;
            ev.HasMorePages = (m_currentPageIndex < m_streams.Count);
        }

        private void PrintEnd(object sender, PrintEventArgs e)
        {
            //when it finish the print return at the previous status
            AppEngine.Current.TrySetSupervisorStatus(memStatus);
            EndPrint(null, null);
            PrintingMexDialog(null, null);
        }


        private async Task ExportPDF(LocalReport report)
        {
            //let the user choose the path where to save the pdf
            SaveFileDialog selectPath = new SaveFileDialog();
            selectPath.Filter = "PDF | *.pdf";
            selectPath.FileName = AppEngine.Current.CurrentContext.ActiveRecipe.RecipeName + "_" + DateTime.Now.ToString("yyyyMMdd-HHmmss");
            if (selectPath.ShowDialog() == DialogResult.OK)
            {
                // Variables
                Warning[] warnings;
                string[] streamIds;
                string mimeType = string.Empty;
                string encoding = string.Empty;
                string extension = string.Empty;
                string tipologia = "PDF";
                string deviceInfo =
                        "<DeviceInfo><ColorDepth>32</ColorDepth>" +
                        "  <PageWidth>8.5in</PageWidth>" +
                        "  <PageHeight>11in</PageHeight>" +
                        "  <MarginTop>0in</MarginTop>" +
                        "  <MarginLeft>0.2in</MarginLeft>" +
                        "  <MarginRight>0in</MarginRight>" +
                        "  <MarginBottom>0in</MarginBottom>" +
                        "</DeviceInfo>";


                byte[] bytes = null;
                await Task.Run(() =>
                {
                    bytes = report.Render(tipologia, deviceInfo, out mimeType, out encoding, out extension, out streamIds, out warnings);
                });

                //write the pdf in the selected path
                FileStream fs = new FileStream(selectPath.FileName, FileMode.Create);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();
            }
        }

        // Rilascia le risorse utilizzate dal programma.
        public void Dispose()
        {
            if (m_streams != null)
            {
                foreach (Stream stream in m_streams)
                    stream.Close();

                m_streams = null;
            }
        }
    }
}
