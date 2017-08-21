namespace CP_ColorDiff
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.pbCamera = new System.Windows.Forms.PictureBox();
            this.pnDraw = new System.Windows.Forms.Panel();
            this.btStopStart = new System.Windows.Forms.Button();
            this.btChangeColor = new System.Windows.Forms.Button();
            this.btClearPanel = new System.Windows.Forms.Button();
            this.nmBrushWidth = new System.Windows.Forms.NumericUpDown();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.camCapture1 = new CameraCapture.CamCapture();
            this.btUndo = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btFingerColorSettings = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbCamera)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmBrushWidth)).BeginInit();
            this.SuspendLayout();
            // 
            // pbCamera
            // 
            this.pbCamera.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pbCamera.Location = new System.Drawing.Point(13, 13);
            this.pbCamera.Name = "pbCamera";
            this.pbCamera.Size = new System.Drawing.Size(533, 419);
            this.pbCamera.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbCamera.TabIndex = 0;
            this.pbCamera.TabStop = false;
            // 
            // pnDraw
            // 
            this.pnDraw.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnDraw.Location = new System.Drawing.Point(553, 13);
            this.pnDraw.Name = "pnDraw";
            this.pnDraw.Size = new System.Drawing.Size(688, 419);
            this.pnDraw.TabIndex = 1;
            this.pnDraw.Paint += new System.Windows.Forms.PaintEventHandler(this.pnDraw_Paint);
            this.pnDraw.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnDraw_MouseDown);
            this.pnDraw.MouseEnter += new System.EventHandler(this.panel1_MouseEnter);
            this.pnDraw.MouseLeave += new System.EventHandler(this.panel1_MouseLeave);
            this.pnDraw.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnDraw_MouseMove);
            this.pnDraw.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnDraw_MouseUp);
            // 
            // btStopStart
            // 
            this.btStopStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btStopStart.Location = new System.Drawing.Point(274, 438);
            this.btStopStart.Name = "btStopStart";
            this.btStopStart.Size = new System.Drawing.Size(160, 40);
            this.btStopStart.TabIndex = 2;
            this.btStopStart.Text = "Начать захват";
            this.btStopStart.UseVisualStyleBackColor = true;
            this.btStopStart.Click += new System.EventHandler(this.btStopStart_Click);
            // 
            // btChangeColor
            // 
            this.btChangeColor.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btChangeColor.Location = new System.Drawing.Point(274, 491);
            this.btChangeColor.Name = "btChangeColor";
            this.btChangeColor.Size = new System.Drawing.Size(160, 40);
            this.btChangeColor.TabIndex = 3;
            this.btChangeColor.Text = "Выбрать цвет";
            this.btChangeColor.UseVisualStyleBackColor = true;
            this.btChangeColor.Click += new System.EventHandler(this.btChangeColor_Click);
            // 
            // btClearPanel
            // 
            this.btClearPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btClearPanel.Location = new System.Drawing.Point(440, 438);
            this.btClearPanel.Name = "btClearPanel";
            this.btClearPanel.Size = new System.Drawing.Size(160, 40);
            this.btClearPanel.TabIndex = 4;
            this.btClearPanel.Text = "Очистить";
            this.btClearPanel.UseVisualStyleBackColor = true;
            this.btClearPanel.Click += new System.EventHandler(this.btClearPanel_Click);
            // 
            // nmBrushWidth
            // 
            this.nmBrushWidth.Location = new System.Drawing.Point(773, 447);
            this.nmBrushWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmBrushWidth.Name = "nmBrushWidth";
            this.nmBrushWidth.Size = new System.Drawing.Size(120, 26);
            this.nmBrushWidth.TabIndex = 5;
            this.nmBrushWidth.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nmBrushWidth.ValueChanged += new System.EventHandler(this.nmBrushWidth_ValueChanged);
            // 
            // camCapture1
            // 
            this.camCapture1.CaptureHeight = 240;
            this.camCapture1.CaptureWidth = 320;
            this.camCapture1.FrameNumber = ((ulong)(0ul));
            this.camCapture1.Location = new System.Drawing.Point(0, 0);
            this.camCapture1.Name = "CamCapture";
            this.camCapture1.Size = new System.Drawing.Size(342, 252);
            this.camCapture1.TabIndex = 0;
            this.camCapture1.TimeToCapture_milliseconds = 100;
            this.camCapture1.ImageCaptured += new CameraCapture.CamCapture.CamEventHandler(this.CamCapture1_ImageCaptured);
            // 
            // btUndo
            // 
            this.btUndo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btUndo.Location = new System.Drawing.Point(440, 491);
            this.btUndo.Name = "btUndo";
            this.btUndo.Size = new System.Drawing.Size(160, 40);
            this.btUndo.TabIndex = 4;
            this.btUndo.Text = "Отменить";
            this.btUndo.UseVisualStyleBackColor = true;
            this.btUndo.Click += new System.EventHandler(this.btUndo_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(606, 448);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(161, 25);
            this.label1.TabIndex = 6;
            this.label1.Text = "Толщина кисти:";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "По умолчанию",
            "Пользовательские"});
            this.comboBox1.Location = new System.Drawing.Point(773, 500);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(187, 28);
            this.comboBox1.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(606, 501);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(163, 25);
            this.label2.TabIndex = 8;
            this.label2.Text = "Настройки HSB:";
            // 
            // btFingerColorSettings
            // 
            this.btFingerColorSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btFingerColorSettings.Location = new System.Drawing.Point(1069, 438);
            this.btFingerColorSettings.Name = "btFingerColorSettings";
            this.btFingerColorSettings.Size = new System.Drawing.Size(172, 93);
            this.btFingerColorSettings.TabIndex = 9;
            this.btFingerColorSettings.Text = "Настроить цвета пальцев";
            this.btFingerColorSettings.UseVisualStyleBackColor = true;
            this.btFingerColorSettings.Click += new System.EventHandler(this.btFingerColorSettings_Click);
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1253, 543);
            this.Controls.Add(this.btFingerColorSettings);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nmBrushWidth);
            this.Controls.Add(this.btUndo);
            this.Controls.Add(this.btClearPanel);
            this.Controls.Add(this.btChangeColor);
            this.Controls.Add(this.btStopStart);
            this.Controls.Add(this.pnDraw);
            this.Controls.Add(this.pbCamera);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Распознавание движений пальцев рук на основе цветовой дифференциации";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_Closing);
            ((System.ComponentModel.ISupportInitialize)(this.pbCamera)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmBrushWidth)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbCamera;
        private System.Windows.Forms.Panel pnDraw;
        private System.Windows.Forms.Button btStopStart;
        private System.Windows.Forms.Button btChangeColor;
        private System.Windows.Forms.Button btClearPanel;
        private System.Windows.Forms.NumericUpDown nmBrushWidth;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private CameraCapture.CamCapture camCapture1;
        private System.Windows.Forms.Button btUndo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btFingerColorSettings;
    }
}

