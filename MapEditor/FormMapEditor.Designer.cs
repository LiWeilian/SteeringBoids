namespace MapEditor
{
    partial class FormMapEditor
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.btnClearPathPoints = new System.Windows.Forms.Button();
            this.btnAddObstacle = new System.Windows.Forms.Button();
            this.btnAddPathPoint = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ResultField = new SkiaSharp.Views.Desktop.SKGLControl();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.btnClearPathPoints);
            this.panel1.Controls.Add(this.btnAddObstacle);
            this.panel1.Controls.Add(this.btnAddPathPoint);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1282, 85);
            this.panel1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(171, 49);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 30);
            this.button1.TabIndex = 3;
            this.button1.Text = "清空障碍物";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnClearPathPoints
            // 
            this.btnClearPathPoints.Location = new System.Drawing.Point(65, 49);
            this.btnClearPathPoints.Name = "btnClearPathPoints";
            this.btnClearPathPoints.Size = new System.Drawing.Size(100, 30);
            this.btnClearPathPoints.TabIndex = 2;
            this.btnClearPathPoints.Text = "清空路径点";
            this.btnClearPathPoints.UseVisualStyleBackColor = true;
            this.btnClearPathPoints.Click += new System.EventHandler(this.btnClearPathPoints_Click);
            // 
            // btnAddObstacle
            // 
            this.btnAddObstacle.Location = new System.Drawing.Point(171, 13);
            this.btnAddObstacle.Name = "btnAddObstacle";
            this.btnAddObstacle.Size = new System.Drawing.Size(100, 30);
            this.btnAddObstacle.TabIndex = 1;
            this.btnAddObstacle.Text = "添加障碍物";
            this.btnAddObstacle.UseVisualStyleBackColor = true;
            this.btnAddObstacle.Click += new System.EventHandler(this.btnAddObstacle_Click);
            // 
            // btnAddPathPoint
            // 
            this.btnAddPathPoint.Location = new System.Drawing.Point(65, 13);
            this.btnAddPathPoint.Name = "btnAddPathPoint";
            this.btnAddPathPoint.Size = new System.Drawing.Size(100, 30);
            this.btnAddPathPoint.TabIndex = 0;
            this.btnAddPathPoint.Text = "添加路径点";
            this.btnAddPathPoint.UseVisualStyleBackColor = true;
            this.btnAddPathPoint.Click += new System.EventHandler(this.btnAddPathPoint_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.ResultField);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 85);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1282, 668);
            this.panel2.TabIndex = 1;
            // 
            // ResultField
            // 
            this.ResultField.BackColor = System.Drawing.Color.Black;
            this.ResultField.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ResultField.Location = new System.Drawing.Point(0, 0);
            this.ResultField.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ResultField.Name = "ResultField";
            this.ResultField.Size = new System.Drawing.Size(1282, 668);
            this.ResultField.TabIndex = 5;
            this.ResultField.VSync = false;
            this.ResultField.PaintSurface += new System.EventHandler<SkiaSharp.Views.Desktop.SKPaintGLSurfaceEventArgs>(this.ResultField_PaintSurface);
            this.ResultField.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ResultField_KeyUp);
            this.ResultField.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ResultField_MouseClick);
            // 
            // FormMapEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1282, 753);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "FormMapEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Map Editor";
            this.Shown += new System.EventHandler(this.FormMapEditor_Shown);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private SkiaSharp.Views.Desktop.SKGLControl ResultField;
        private System.Windows.Forms.Button btnAddPathPoint;
        private System.Windows.Forms.Button btnAddObstacle;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnClearPathPoints;
    }
}

