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
using SteeringBehavioursCore.Model.Field;
using SteeringBehavioursCore.Renderer;

namespace KeySteeringBoids
{
    public partial class FormKeySteeringBoids : Form
    {
        private readonly SteeringByKeysController _controller;
        private readonly DispatcherTimer _timer = new DispatcherTimer();
        public FormKeySteeringBoids()
        {
            InitializeComponent();

            _controller = new SteeringByKeysController();
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
            _controller.CreateRenderer(new KeySteeringRenderer(e.Surface.Canvas));
            _controller.Renderer.Render(_controller.Field);
        }

        private void ResultField_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.R:
                    _controller.CreateField();
                    break;
                default:
                    break;
            }
        }

        private void ResultField_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    _controller.KeySteering(1);
                    break;
                case Keys.S:
                    _controller.KeySteering(2);
                    break;
                case Keys.A:
                    _controller.KeySteering(3);
                    break;
                case Keys.D:
                    _controller.KeySteering(4);
                    break;
                default:
                    break;
            }
        }
    }
}
