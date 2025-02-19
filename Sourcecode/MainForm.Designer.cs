namespace Plot3D
{
    partial class MainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.trackRho = new System.Windows.Forms.TrackBar();
            this.trackTheta = new System.Windows.Forms.TrackBar();
            this.trackPhi = new System.Windows.Forms.TrackBar();
            this.lblInfo = new System.Windows.Forms.Label();
            this.comboColors = new System.Windows.Forms.ComboBox();
            this.comboDemo = new System.Windows.Forms.ComboBox();
            this.labelMouseInfo = new System.Windows.Forms.Label();
            this.comboRaster = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnScreenshot = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.comboMouse = new System.Windows.Forms.ComboBox();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnDeselect = new System.Windows.Forms.Button();
            this.checkPointSelection = new System.Windows.Forms.CheckBox();
            this.checkMirrorX = new System.Windows.Forms.CheckBox();
            this.checkMirrorY = new System.Windows.Forms.CheckBox();
            this.checkIncludeZeroZ = new System.Windows.Forms.CheckBox();
            this.btnUndo = new System.Windows.Forms.Button();
            this.btnRedo = new System.Windows.Forms.Button();
            this.editor3D = new Plot3D.Editor3D();
            ((System.ComponentModel.ISupportInitialize)(this.trackRho)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackTheta)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackPhi)).BeginInit();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // trackRho
            // 
            this.trackRho.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.trackRho.Location = new System.Drawing.Point(9, 362);
            this.trackRho.Name = "trackRho";
            this.trackRho.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackRho.Size = new System.Drawing.Size(45, 310);
            this.trackRho.TabIndex = 20;
            this.trackRho.TickFrequency = 20;
            this.trackRho.TickStyle = System.Windows.Forms.TickStyle.None;
            // 
            // trackTheta
            // 
            this.trackTheta.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.trackTheta.Location = new System.Drawing.Point(53, 362);
            this.trackTheta.Name = "trackTheta";
            this.trackTheta.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackTheta.Size = new System.Drawing.Size(45, 310);
            this.trackTheta.TabIndex = 21;
            this.trackTheta.TickFrequency = 20;
            this.trackTheta.TickStyle = System.Windows.Forms.TickStyle.None;
            // 
            // trackPhi
            // 
            this.trackPhi.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.trackPhi.Location = new System.Drawing.Point(98, 362);
            this.trackPhi.Name = "trackPhi";
            this.trackPhi.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackPhi.Size = new System.Drawing.Size(45, 310);
            this.trackPhi.TabIndex = 22;
            this.trackPhi.TickFrequency = 20;
            this.trackPhi.TickStyle = System.Windows.Forms.TickStyle.None;
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.ForeColor = System.Drawing.Color.Blue;
            this.lblInfo.Location = new System.Drawing.Point(7, 50);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(25, 13);
            this.lblInfo.TabIndex = 0;
            this.lblInfo.Text = "Info";
            // 
            // comboColors
            // 
            this.comboColors.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboColors.FormattingEnabled = true;
            this.comboColors.Location = new System.Drawing.Point(9, 84);
            this.comboColors.MaxDropDownItems = 30;
            this.comboColors.Name = "comboColors";
            this.comboColors.Size = new System.Drawing.Size(121, 21);
            this.comboColors.TabIndex = 3;
            this.comboColors.SelectedIndexChanged += new System.EventHandler(this.comboColors_SelectedIndexChanged);
            // 
            // comboDemo
            // 
            this.comboDemo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboDemo.FormattingEnabled = true;
            this.comboDemo.Location = new System.Drawing.Point(9, 26);
            this.comboDemo.MaxDropDownItems = 30;
            this.comboDemo.Name = "comboDemo";
            this.comboDemo.Size = new System.Drawing.Size(121, 21);
            this.comboDemo.TabIndex = 2;
            this.comboDemo.SelectedIndexChanged += new System.EventHandler(this.comboDemo_SelectedIndexChanged);
            // 
            // labelMouseInfo
            // 
            this.labelMouseInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelMouseInfo.AutoSize = true;
            this.labelMouseInfo.ForeColor = System.Drawing.Color.Blue;
            this.labelMouseInfo.Location = new System.Drawing.Point(137, 676);
            this.labelMouseInfo.Name = "labelMouseInfo";
            this.labelMouseInfo.Size = new System.Drawing.Size(60, 13);
            this.labelMouseInfo.TabIndex = 0;
            this.labelMouseInfo.Text = "Mouse Info";
            // 
            // comboRaster
            // 
            this.comboRaster.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboRaster.FormattingEnabled = true;
            this.comboRaster.Location = new System.Drawing.Point(9, 123);
            this.comboRaster.MaxDropDownItems = 30;
            this.comboRaster.Name = "comboRaster";
            this.comboRaster.Size = new System.Drawing.Size(121, 21);
            this.comboRaster.TabIndex = 4;
            this.comboRaster.SelectedIndexChanged += new System.EventHandler(this.comboRaster_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "3D Demo";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Color Scheme:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 108);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(98, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Coordinate System:";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 674);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(27, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Rho";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(49, 674);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Theta";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(99, 674);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(22, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Phi";
            // 
            // btnScreenshot
            // 
            this.btnScreenshot.Location = new System.Drawing.Point(9, 255);
            this.btnScreenshot.Name = "btnScreenshot";
            this.btnScreenshot.Size = new System.Drawing.Size(121, 23);
            this.btnScreenshot.TabIndex = 10;
            this.btnScreenshot.Text = "Save Screenshot";
            this.btnScreenshot.UseVisualStyleBackColor = true;
            this.btnScreenshot.Click += new System.EventHandler(this.btnScreenshot_Click);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(9, 228);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(121, 23);
            this.btnReset.TabIndex = 9;
            this.btnReset.Text = "Reset Position";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 187);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(81, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Mouse Buttons:";
            // 
            // comboMouse
            // 
            this.comboMouse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboMouse.FormattingEnabled = true;
            this.comboMouse.Items.AddRange(new object[] {
            "Left Theta, Right Phi",
            "Left Theta and Phi",
            "Middle Theta and Phi"});
            this.comboMouse.Location = new System.Drawing.Point(9, 202);
            this.comboMouse.MaxDropDownItems = 30;
            this.comboMouse.Name = "comboMouse";
            this.comboMouse.Size = new System.Drawing.Size(121, 21);
            this.comboMouse.TabIndex = 8;
            this.comboMouse.SelectedIndexChanged += new System.EventHandler(this.comboMouse_SelectedIndexChanged);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 693);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(807, 22);
            this.statusStrip.TabIndex = 23;
            this.statusStrip.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = false;
            this.statusLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLabel.ForeColor = System.Drawing.Color.Black;
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.statusLabel.Size = new System.Drawing.Size(86, 17);
            this.statusLabel.Text = "Select a demo";
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnDeselect
            // 
            this.btnDeselect.Location = new System.Drawing.Point(9, 304);
            this.btnDeselect.Name = "btnDeselect";
            this.btnDeselect.Size = new System.Drawing.Size(121, 23);
            this.btnDeselect.TabIndex = 12;
            this.btnDeselect.Text = "Remove Selection";
            this.btnDeselect.UseVisualStyleBackColor = true;
            this.btnDeselect.Click += new System.EventHandler(this.btnDeselect_Click);
            // 
            // checkPointSelection
            // 
            this.checkPointSelection.AutoSize = true;
            this.checkPointSelection.BackColor = System.Drawing.Color.Transparent;
            this.checkPointSelection.Location = new System.Drawing.Point(10, 283);
            this.checkPointSelection.Name = "checkPointSelection";
            this.checkPointSelection.Size = new System.Drawing.Size(97, 17);
            this.checkPointSelection.TabIndex = 11;
            this.checkPointSelection.Text = "Point Selection";
            this.checkPointSelection.UseVisualStyleBackColor = false;
            this.checkPointSelection.CheckedChanged += new System.EventHandler(this.checkPointSelection_CheckedChanged);
            // 
            // checkMirrorX
            // 
            this.checkMirrorX.AutoSize = true;
            this.checkMirrorX.BackColor = System.Drawing.Color.Transparent;
            this.checkMirrorX.Location = new System.Drawing.Point(9, 167);
            this.checkMirrorX.Name = "checkMirrorX";
            this.checkMirrorX.Size = new System.Drawing.Size(62, 17);
            this.checkMirrorX.TabIndex = 6;
            this.checkMirrorX.Text = "Mirror X";
            this.checkMirrorX.UseVisualStyleBackColor = false;
            this.checkMirrorX.CheckedChanged += new System.EventHandler(this.checkMirrorX_CheckedChanged);
            // 
            // checkMirrorY
            // 
            this.checkMirrorY.AutoSize = true;
            this.checkMirrorY.BackColor = System.Drawing.Color.Transparent;
            this.checkMirrorY.Location = new System.Drawing.Point(71, 167);
            this.checkMirrorY.Name = "checkMirrorY";
            this.checkMirrorY.Size = new System.Drawing.Size(62, 17);
            this.checkMirrorY.TabIndex = 7;
            this.checkMirrorY.Text = "Mirror Y";
            this.checkMirrorY.UseVisualStyleBackColor = false;
            this.checkMirrorY.CheckedChanged += new System.EventHandler(this.checkMirrorY_CheckedChanged);
            // 
            // checkIncludeZeroZ
            // 
            this.checkIncludeZeroZ.AutoSize = true;
            this.checkIncludeZeroZ.BackColor = System.Drawing.Color.Transparent;
            this.checkIncludeZeroZ.Checked = true;
            this.checkIncludeZeroZ.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkIncludeZeroZ.Location = new System.Drawing.Point(9, 149);
            this.checkIncludeZeroZ.Name = "checkIncludeZeroZ";
            this.checkIncludeZeroZ.Size = new System.Drawing.Size(96, 17);
            this.checkIncludeZeroZ.TabIndex = 5;
            this.checkIncludeZeroZ.Text = "Include Zero Z";
            this.checkIncludeZeroZ.UseVisualStyleBackColor = false;
            this.checkIncludeZeroZ.CheckedChanged += new System.EventHandler(this.checkIncludeZeroZ_CheckedChanged);
            // 
            // btnUndo
            // 
            this.btnUndo.Location = new System.Drawing.Point(9, 333);
            this.btnUndo.Name = "btnUndo";
            this.btnUndo.Size = new System.Drawing.Size(55, 23);
            this.btnUndo.TabIndex = 13;
            this.btnUndo.Text = "Undo";
            this.btnUndo.UseVisualStyleBackColor = true;
            this.btnUndo.Click += new System.EventHandler(this.btnUndo_Click);
            // 
            // btnRedo
            // 
            this.btnRedo.Location = new System.Drawing.Point(75, 333);
            this.btnRedo.Name = "btnRedo";
            this.btnRedo.Size = new System.Drawing.Size(55, 23);
            this.btnRedo.TabIndex = 14;
            this.btnRedo.Text = "Redo";
            this.btnRedo.UseVisualStyleBackColor = true;
            this.btnRedo.Click += new System.EventHandler(this.btnRedo_Click);
            // 
            // editor3D
            // 
            this.editor3D.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.editor3D.BackColor = System.Drawing.Color.White;
            this.editor3D.BorderColorFocus = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            this.editor3D.BorderColorNormal = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.editor3D.Cursor = System.Windows.Forms.Cursors.Default;
            this.editor3D.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.editor3D.Location = new System.Drawing.Point(139, 11);
            this.editor3D.Name = "editor3D";
            this.editor3D.Normalize = Plot3D.Editor3D.eNormalize.Separate;
            this.editor3D.Raster = Plot3D.Editor3D.eRaster.Off;
            this.editor3D.Size = new System.Drawing.Size(656, 661);
            this.editor3D.TabIndex = 1;
            this.editor3D.TooltipMode = ((Plot3D.Editor3D.eTooltip)((Plot3D.Editor3D.eTooltip.UserText | Plot3D.Editor3D.eTooltip.Coord)));
            this.editor3D.TopLegendColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(150)))));
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(807, 715);
            this.Controls.Add(this.btnRedo);
            this.Controls.Add(this.btnUndo);
            this.Controls.Add(this.checkIncludeZeroZ);
            this.Controls.Add(this.checkMirrorY);
            this.Controls.Add(this.checkMirrorX);
            this.Controls.Add(this.checkPointSelection);
            this.Controls.Add(this.btnDeselect);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.comboMouse);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnScreenshot);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboRaster);
            this.Controls.Add(this.labelMouseInfo);
            this.Controls.Add(this.comboDemo);
            this.Controls.Add(this.comboColors);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.editor3D);
            this.Controls.Add(this.trackPhi);
            this.Controls.Add(this.trackTheta);
            this.Controls.Add(this.trackRho);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(400, 400);
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "3D Editor Demo";
            ((System.ComponentModel.ISupportInitialize)(this.trackRho)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackTheta)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackPhi)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar trackRho;
        private System.Windows.Forms.TrackBar trackTheta;
        private System.Windows.Forms.TrackBar trackPhi;
        private Plot3D.Editor3D editor3D;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.ComboBox comboColors;
        private System.Windows.Forms.ComboBox comboDemo;
        private System.Windows.Forms.Label labelMouseInfo;
        private System.Windows.Forms.ComboBox comboRaster;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnScreenshot;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox comboMouse;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.Button btnDeselect;
        private System.Windows.Forms.CheckBox checkPointSelection;
        private System.Windows.Forms.CheckBox checkMirrorX;
        private System.Windows.Forms.CheckBox checkMirrorY;
        private System.Windows.Forms.CheckBox checkIncludeZeroZ;
        private System.Windows.Forms.Button btnUndo;
        private System.Windows.Forms.Button btnRedo;

    }
}

