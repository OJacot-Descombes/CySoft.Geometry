
namespace VisualTests
{
    partial class frmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.viewModelBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.nudNumberOfPoints = new System.Windows.Forms.NumericUpDown();
            this.lblNumberOfPoints = new System.Windows.Forms.Label();
            this.btnNext = new System.Windows.Forms.Button();
            this.pnlCanvas = new System.Windows.Forms.Panel();
            this.cboPointSet = new System.Windows.Forms.ComboBox();
            this.cboResultKind = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.viewModelBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumberOfPoints)).BeginInit();
            this.SuspendLayout();
            // 
            // nudNumberOfPoints
            // 
            this.nudNumberOfPoints.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudNumberOfPoints.Location = new System.Drawing.Point(122, 7);
            this.nudNumberOfPoints.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudNumberOfPoints.Name = "nudNumberOfPoints";
            this.nudNumberOfPoints.Size = new System.Drawing.Size(57, 23);
            this.nudNumberOfPoints.TabIndex = 0;
            this.nudNumberOfPoints.ThousandsSeparator = true;
            // 
            // lblNumberOfPoints
            // 
            this.lblNumberOfPoints.AutoSize = true;
            this.lblNumberOfPoints.Location = new System.Drawing.Point(12, 9);
            this.lblNumberOfPoints.Name = "lblNumberOfPoints";
            this.lblNumberOfPoints.Size = new System.Drawing.Size(104, 15);
            this.lblNumberOfPoints.TabIndex = 1;
            this.lblNumberOfPoints.Text = "Number of points:";
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext.Location = new System.Drawing.Point(587, 5);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(75, 23);
            this.btnNext.TabIndex = 2;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = true;
            // 
            // pnlCanvas
            // 
            this.pnlCanvas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlCanvas.BackColor = System.Drawing.Color.White;
            this.pnlCanvas.Location = new System.Drawing.Point(12, 36);
            this.pnlCanvas.Name = "pnlCanvas";
            this.pnlCanvas.Size = new System.Drawing.Size(650, 650);
            this.pnlCanvas.TabIndex = 3;
            this.pnlCanvas.Resize += new System.EventHandler(this.pnlCanvas_Resize);
            // 
            // cboPointSet
            // 
            this.cboPointSet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPointSet.Location = new System.Drawing.Point(408, 6);
            this.cboPointSet.Name = "cboPointSet";
            this.cboPointSet.Size = new System.Drawing.Size(121, 23);
            this.cboPointSet.TabIndex = 7;
            // 
            // cboResultKind
            // 
            this.cboResultKind.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboResultKind.Location = new System.Drawing.Point(232, 6);
            this.cboResultKind.Name = "cboResultKind";
            this.cboResultKind.Size = new System.Drawing.Size(121, 23);
            this.cboResultKind.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(185, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 15);
            this.label1.TabIndex = 8;
            this.label1.Text = "Boxes:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(359, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 15);
            this.label2.TabIndex = 9;
            this.label2.Text = "Points:";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(674, 698);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboPointSet);
            this.Controls.Add(this.cboResultKind);
            this.Controls.Add(this.pnlCanvas);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.lblNumberOfPoints);
            this.Controls.Add(this.nudNumberOfPoints);
            this.Name = "frmMain";
            this.Text = "Convex Hull Test";
            this.Shown += new System.EventHandler(this.frmMain_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.viewModelBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumberOfPoints)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingSource viewModelBindingSource;
        private System.Windows.Forms.NumericUpDown nudNumberOfPoints;
        private System.Windows.Forms.Label lblNumberOfPoints;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Panel pnlCanvas;
        private System.Windows.Forms.ComboBox cboPointSet;
        private System.Windows.Forms.ComboBox cboResultKind;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

