using System;
using System.Windows.Forms;
using System.Windows.Threading;
using SkiaSharp.Views.Desktop;
using SteeringBehavioursCore.Controller;
using SteeringBehavioursCore.Model.Field;
using SteeringBehavioursCore.Renderer;

namespace PredatorBoids
{
    public partial class FormPredatorBoids : Form
    {
        private readonly PredatorBoidsController _controller;
        private readonly DispatcherTimer _timer = new DispatcherTimer();
        public FormPredatorBoids()
        {
            InitializeComponent(); _controller = new PredatorBoidsController();
            _controller.CreateField();
            _controller.Field.SetFieldSize(BaseField.Width, BaseField.Height);

            _timer.Interval = TimeSpan.FromMilliseconds(10);
            _timer.Tick += TimerTick;
            _timer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            _controller.Field.Advance();
            ResultField.Invalidate();
        }

        private void ResultField_PaintSurface(object sender, SKPaintGLSurfaceEventArgs e)
        {
            _controller.CreateRenderer(new PredatorBoidsRenderer(e.Surface.Canvas));
            _controller.Renderer.Render(_controller.Field);
        }
    }
}
