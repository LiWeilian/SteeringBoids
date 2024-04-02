using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;
using SkiaSharp.Views.Desktop;
using SteeringBehavioursCore.Controller;
using SteeringBehavioursCore.Model;
using SteeringBehavioursCore.Model.Behaviour;
using SteeringBehavioursCore.Renderer;
using SteeringBehavioursCore.Model.Field;

namespace ArriveBoids
{
    public partial class FormArriveBoids : System.Windows.Forms.Form
    {
        private readonly ArriveController _controller;
        private readonly DispatcherTimer _timer = new DispatcherTimer();
        public FormArriveBoids()
        {
            InitializeComponent();

            _controller = new ArriveController();
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

        private void SKElement_PaintSurface(object sender,
            SKPaintSurfaceEventArgs e)
        {
            _controller.CreateRenderer(new ArriveRenderer(e.Surface.Canvas));
            _controller.Renderer.Render(_controller.Field);
        }

        private void ResultField_MouseDown(object sender, MouseEventArgs e)
        {
            _controller.Field.Interaction.OnMouseDown((int)e.Button, e.X, e.Y);
        }
    }
}
