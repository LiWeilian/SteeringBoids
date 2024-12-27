using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SteeringBehavioursCore.Controller;
using SteeringBehavioursCore.Model;
using SteeringBehavioursCore.Model.Field;
using SteeringBehavioursCore.Renderer;
using Xamarin.Forms.Xaml;

namespace MapEditor
{
    internal class MapTool
    {
        public virtual string ToolName { get; }
        protected MapEditorController _controller;
        protected MapEditorField _field;
        protected MapEditorRenderer _renderer;
        public MapTool(MapEditorController controller)
        {
            _controller = controller;
            _field = _controller.Field as MapEditorField;
            _renderer = _controller.Renderer as MapEditorRenderer;
        }

        protected PathPoint NearestPathPoint(float x, float y)
        {
            return _field?.NearestPathPoint(new Position(x, y));
        }

        public virtual void MapMouseClick(MouseEventArgs e)
        {
            
        }

        public virtual void MapKeyUp(KeyEventArgs e)
        {

        }

        public virtual void RenderSketch()
        {

        }
    }

    internal class AddPathLineTool : MapTool
    {
        public override string ToolName { get => "添加路径点"; }

        private Line _lineSketch = null;
        private Path _pathSketch = null;
        private PathPoint _lastPathPointSketch = null;
        
        public AddPathLineTool(MapEditorController controller) : base(controller)
        {

        }

        public override void MapMouseClick(MouseEventArgs e)
        {
            base.MapMouseClick(e);

            switch (e.Button)
            {
                case MouseButtons.Left:
                    PathPoint nearestPoint = NearestPathPoint(e.X, e.Y);
                    if (nearestPoint == null)
                    {
                        return;
                    }

                    if (_lineSketch == null)
                    {
                        _lineSketch = new Line();
                    }

                    if (_lastPathPointSketch != null)
                    {
                        _pathSketch = new Path(_lastPathPointSketch, nearestPoint);
                        _lineSketch.Paths.Add(_pathSketch);
                    }
                    _lastPathPointSketch = nearestPoint;
                    break;
                case MouseButtons.None:
                    break;
                case MouseButtons.Right:
                    //保存草图
                    SaveSketchLine();

                    _lastPathPointSketch = null;
                    _pathSketch = null;
                    _lineSketch = null;
                    break;
                case MouseButtons.Middle:
                    break;
                case MouseButtons.XButton1:
                    break;
                case MouseButtons.XButton2:
                    break;
                default:
                    break;
            }
        }

        public override void MapKeyUp(KeyEventArgs e)
        {
            base.MapKeyUp(e);

            switch (e.KeyCode)
            {
                case Keys.Escape:
                    //取消当前草图
                    _lastPathPointSketch = null;
                    _pathSketch = null;
                    _lineSketch = null;
                    break;
                case Keys.Enter:
                    //保存草图
                    SaveSketchLine();

                    _lastPathPointSketch = null;
                    _pathSketch = null;
                    _lineSketch = null;
                    //刷新地图
                    break;
                default:
                    break;
            }
        }

        private void SaveSketchLine()
        {
            if (_lineSketch != null)
            {
                _field.AddLine(_lineSketch);
            }
        }

        public override void RenderSketch()
        {
            base.RenderSketch();

            float lineWidth = 4f;
            Color lineColor = new Color(255, 255, 0);

            float pointSize = 6f;
            Color pointColor = new Color(255, 255, 0);

            if (_lineSketch != null)
            {
                foreach (var path in _lineSketch.Paths)
                {
                    _renderer.DrawLine(new Point(path.FromPosition.Position.X, path.FromPosition.Position.Y),
                        new Point(path.ToPosition.Position.X, path.ToPosition.Position.Y),
                        lineWidth,
                        lineColor);


                    _renderer.FillCircle(new Point(path.FromPosition.Position.X, path.FromPosition.Position.Y),
                        pointSize,
                        pointColor);
                    _renderer.FillCircle(new Point(path.ToPosition.Position.X, path.ToPosition.Position.Y),
                        pointSize,
                        pointColor);
                }
            }

            if (_pathSketch != null)
            {
                _renderer.DrawLine(new Point(_pathSketch.FromPosition.Position.X, _pathSketch.FromPosition.Position.Y),
                    new Point(_pathSketch.ToPosition.Position.X, _pathSketch.ToPosition.Position.Y),
                    lineWidth,
                    lineColor);


                _renderer.FillCircle(new Point(_pathSketch.FromPosition.Position.X, _pathSketch.FromPosition.Position.Y),
                    pointSize,
                    pointColor);
                _renderer.FillCircle(new Point(_pathSketch.ToPosition.Position.X, _pathSketch.ToPosition.Position.Y),
                    pointSize,
                    pointColor);
            }

            if (_lastPathPointSketch != null)
            {
                _renderer.FillCircle(new Point(_lastPathPointSketch.Position.X, _lastPathPointSketch.Position.Y),
                    pointSize,
                    pointColor);
            }
        }
    }

    internal class AddObstacleTool : MapTool
    {
        public override string ToolName { get => "添加障碍物"; }
        private PathPoint _lastPathPointSketch = null;
        private Obstacle _obstacleSketch = null;
        public AddObstacleTool(MapEditorController controller) : base(controller)
        {

        }

        public override void MapMouseClick(MouseEventArgs e)
        {
            base.MapMouseClick(e);

            switch (e.Button)
            {
                case MouseButtons.Left:
                    PathPoint nearestPoint = NearestPathPoint(e.X, e.Y);
                    if (nearestPoint == null)
                    {
                        return;
                    }

                    if (_lastPathPointSketch == null)
                    {
                        _lastPathPointSketch = nearestPoint;
                        return;
                    }

                    if (_lastPathPointSketch.Position.X == nearestPoint.Position.X
                        || _lastPathPointSketch.Position.Y == nearestPoint.Position.Y)
                    {
                        return;
                    }

                    _obstacleSketch = new Obstacle(_lastPathPointSketch.Position.X,
                        _lastPathPointSketch.Position.Y,
                        nearestPoint.Position.X, 
                        nearestPoint.Position.Y);

                    _lastPathPointSketch = null;
                    break;
                case MouseButtons.None:
                    break;
                case MouseButtons.Right:
                    if (_obstacleSketch != null)
                    {
                        _field.AddObstacle(_obstacleSketch);
                    }

                    _obstacleSketch = null;
                    _lastPathPointSketch = null;
                    break;
                case MouseButtons.Middle:
                    break;
                case MouseButtons.XButton1:
                    break;
                case MouseButtons.XButton2:
                    break;
                default:
                    break;
            }
        }

        public override void MapKeyUp(KeyEventArgs e)
        {
            base.MapKeyUp(e);
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    _obstacleSketch = null;
                    _lastPathPointSketch = null;
                    break;
                case Keys.Enter:
                    if (_obstacleSketch != null)
                    {
                        _field.AddObstacle(_obstacleSketch);
                    }

                    _obstacleSketch = null;
                    _lastPathPointSketch = null;
                    break;
                default:
                    break;
            }
        }

        public override void RenderSketch()
        {
            base.RenderSketch();

            float lineWidth = 4f;
            Color lineColor = new Color(255, 255, 0);

            float pointSize = 6f;
            Color pointColor = new Color(255, 255, 0);

            if (_obstacleSketch != null)
            {
                for (int i = 0; i < _obstacleSketch.Points.Count - 1; i++)
                {
                    _renderer.DrawLine(new Point(_obstacleSketch.Points[i].X, _obstacleSketch.Points[i].Y),
                        new Point(_obstacleSketch.Points[i + 1].X, _obstacleSketch.Points[i + 1].Y),
                        lineWidth,
                        lineColor);
                }
                _renderer.DrawLine(new Point(_obstacleSketch.Points[_obstacleSketch.Points.Count - 1].X, _obstacleSketch.Points[_obstacleSketch.Points.Count - 1].Y),
                    new Point(_obstacleSketch.Points[0].X, _obstacleSketch.Points[0].Y),
                        lineWidth,
                        lineColor);
            }

            if (_lastPathPointSketch != null)
            {
                _renderer.FillCircle(new Point(_lastPathPointSketch.Position.X, _lastPathPointSketch.Position.Y),
                    pointSize,
                    pointColor);
            }
        }
    }
}
