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
using SteeringBehavioursCore.Model.Field;
using SteeringBehavioursCore.Renderer;

namespace MapEditor
{
    public partial class FormMapEditor : Form
    {
        private readonly MapEditorController _controller;
        private readonly DispatcherTimer _timer = new DispatcherTimer();
        private MapTool _mapTool = null;
        public FormMapEditor()
        {
            InitializeComponent();

            _controller = new MapEditorController();
            _controller.CreateField();
            _controller.Field.SetFieldSize(BaseField.Width, BaseField.Height);
        }

        private void ResultField_PaintSurface(object sender, SKPaintGLSurfaceEventArgs e)
        {
            _controller.CreateRenderer(new MapEditorRenderer(e.Surface.Canvas));
            _controller.Renderer.Render(_controller.Field);
            if (_mapTool != null)
            {
                _mapTool.RenderSketch();
            }
        }

        private void FormMapEditor_Shown(object sender, EventArgs e)
        {
            ResultField.Invalidate();
        }

        private void ResultField_MouseClick(object sender, MouseEventArgs e)
        {
            if (_mapTool != null)
            {
                _mapTool.MapMouseClick(e);
                ResultField.Invalidate();
            }
        }

        private void ResultField_KeyUp(object sender, KeyEventArgs e)
        {
            if (_mapTool != null)
            {
                _mapTool.MapKeyUp(e);
                ResultField.Invalidate();
            }
        }

        private void btnAddPathPoint_Click(object sender, EventArgs e)
        {
            if (_mapTool != null && !(_mapTool is AddPathLineTool))
            {
                //取消
                _mapTool.MapKeyUp(new KeyEventArgs(Keys.Escape));
                _mapTool = null;
            }
            if (_mapTool == null)
            {
                _mapTool = new AddPathLineTool(_controller);

                ResultField.Cursor = Cursors.Cross;
            }

        }

        private void btnAddObstacle_Click(object sender, EventArgs e)
        {
            if (_mapTool != null && !(_mapTool is AddObstacleTool))
            {
                //取消
                _mapTool.MapKeyUp(new KeyEventArgs(Keys.Escape));
                _mapTool = null;
            }
            if (_mapTool == null)
            {
                _mapTool = new AddObstacleTool(_controller);

                ResultField.Cursor = Cursors.Cross;
            }
        }

        private void btnClearPathPoints_Click(object sender, EventArgs e)
        {
            (_controller.Field as MapEditorField).PathLines.Clear();
            ResultField.Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            (_controller.Field as MapEditorField).Obstacles.Clear();
            ResultField.Invalidate();
        }
    }
}
