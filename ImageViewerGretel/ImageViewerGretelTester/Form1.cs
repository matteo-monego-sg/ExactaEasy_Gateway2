using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ImageViewerGretelTester {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
            
        }

        private void label1_Click(object sender, EventArgs e) {

        }

        private void button1_Click(object sender, EventArgs e) {

            using (OpenFileDialog openFileDlg = new OpenFileDialog()) {
                if (openFileDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                    imageViewerGretel1.LoadImage(openFileDlg.FileName);
                    imageViewerGretel1.Play();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e) {

            using (OpenFileDialog openFileDlg = new OpenFileDialog()) {
                try {
                    imageViewerGretel1.SetFrameRate(33.3f);
                    imageViewerGretel1.InitImageIndex = Int32.Parse(textBox3.Text);
                    imageViewerGretel1.ImageStep = Int32.Parse(textBox4.Text);
                    if (openFileDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                        imageViewerGretel1.LoadImage(
                            openFileDlg.FileName,
                            "",//@"G:\03-SOURCE\01-ExactaEasy\ExactaEasy_Menarini2_Stable\ImageViewerGretel\images\Frames_ST0_000_2016_03_11_04_25_34_ID_007.tiff", 
                            Int32.Parse(textBox1.Text),
                            Int32.Parse(textBox2.Text),
                            ImageViewerGretel.CutType.TopBottom);
                        imageViewerGretel1.Play();
                    }
                }
                catch (Exception ex) {
                    MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e) {

            imageViewerGretel1.Destroy();
        }

        private void textBox4_TextChanged(object sender, EventArgs e) {

            try {
                imageViewerGretel1.ImageStep = Int32.Parse(textBox4.Text);
            }
            catch {
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e) {

            try {
                imageViewerGretel1.InitImageIndex = Int32.Parse(textBox3.Text);
            }
            catch {
            }
        }

        private void imageViewerGretel1_Load(object sender, EventArgs e) {

        }

        private void button3_Click(object sender, EventArgs e) {

            using (FolderBrowserDialog openFolderDlg = new FolderBrowserDialog()) {
                if (openFolderDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                    imageViewerGretel1.LoadImageFolder(openFolderDlg.SelectedPath);
                    imageViewerGretel1.CreateVideo(ImageViewerGretel.MergeType.Vertical, 0, openFolderDlg.SelectedPath + @"\merge");
                    imageViewerGretel1.Play();
                }
            }
        }
    }
}
