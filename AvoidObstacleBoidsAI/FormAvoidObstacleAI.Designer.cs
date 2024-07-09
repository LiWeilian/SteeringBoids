namespace AvoidObstacleBoidsAI
{
    partial class FormAvoidObstacleAI
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
            this.ResultField = new SkiaSharp.Views.Desktop.SKGLControl();
            this.SuspendLayout();
            // 
            // ResultField
            // 
            this.ResultField.BackColor = System.Drawing.Color.Black;
            this.ResultField.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ResultField.Location = new System.Drawing.Point(0, 0);
            this.ResultField.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ResultField.Name = "ResultField";
            this.ResultField.Size = new System.Drawing.Size(1582, 753);
            this.ResultField.TabIndex = 2;
            this.ResultField.VSync = false;
            this.ResultField.PaintSurface += new System.EventHandler<SkiaSharp.Views.Desktop.SKPaintGLSurfaceEventArgs>(this.ResultField_PaintSurface);
            // 
            // FormAvoidObstacleAI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1582, 753);
            this.Controls.Add(this.ResultField);
            this.Name = "FormAvoidObstacleAI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Avoid Obstacle By AI";
            this.ResumeLayout(false);

        }

        #endregion

        private SkiaSharp.Views.Desktop.SKGLControl ResultField;
    }
}

