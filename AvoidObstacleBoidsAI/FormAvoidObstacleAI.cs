﻿using System;
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
using SteeringBehavioursCore.Model.Field;
using SteeringBehavioursCore.Renderer;

namespace AvoidObstacleBoidsAI
{
    public partial class FormAvoidObstacleAI : Form
    {
        private readonly AvoidObstacleAIController _controller;
        private readonly DispatcherTimer _timer = new DispatcherTimer();
        public FormAvoidObstacleAI()
        {
            InitializeComponent();

            _controller = new AvoidObstacleAIController();
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
            _controller.CreateRenderer(new AvoidObstacleAIRenderer(e.Surface.Canvas));
            _controller.Renderer.Render(_controller.Field);
        }
    }
}
