
namespace Hvld.Controls
{
    partial class HvldStackedDisplay
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.TablePanel = new System.Windows.Forms.TableLayoutPanel();
            this.SuspendLayout();
            // 
            // TablePanel
            // 
            this.TablePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(56)))));
            this.TablePanel.ColumnCount = 1;
            this.TablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TablePanel.Location = new System.Drawing.Point(0, 0);
            this.TablePanel.Margin = new System.Windows.Forms.Padding(0);
            this.TablePanel.Name = "TablePanel";
            this.TablePanel.RowCount = 1;
            this.TablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TablePanel.Size = new System.Drawing.Size(200, 200);
            this.TablePanel.TabIndex = 4;
            // 
            // HvldStackedDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TablePanel);
            this.Name = "HvldStackedDisplay";
            this.Size = new System.Drawing.Size(200, 200);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel TablePanel;
    }
}
