using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteeringBehavioursCore.Model.Field
{
    public class MapEditorField : BaseField
    {
        public float GridSizeWidth { get; set; } = 30f;
        public float GridSizeHeight { get; set; } = 30f;

        public MapEditorField()
        {
            InitializeBasePoints();
            InitializeBasePaths();
        }


        public List<Path> BasePaths
        {
            get
            {
                return _basePaths;
            }
        }
        private List<Path> _basePaths = new List<Path>();

        private void InitializeBasePaths()
        {
            _basePaths.Clear();
            int count1 = (int)(Width / GridSizeWidth);
            int count2 = (int)(Height / GridSizeHeight);

            for (int i = 0; i <= count1; i++)
            {
                for (int j = 0; j < count2; j++)
                {
                    string fromCode = $"{i}-{j}";
                    PathPoint fromPoint = _basePathPoints.FirstOrDefault(p => p.Code == fromCode);
                    if (fromPoint != null)
                    {
                        string toCode1 = $"{i + 1}-{j}";
                        PathPoint toPoint1 = _basePathPoints.FirstOrDefault(p => p.Code == toCode1);
                        if (toPoint1 != null)
                        {
                            _basePaths.Add(new Path(fromPoint, toPoint1));
                        }

                        string toCode2 = $"{i + 1}-{j + 1}";
                        PathPoint toPoint2 = _basePathPoints.FirstOrDefault(p => p.Code == toCode2);
                        if (toPoint2 != null)
                        {
                            _basePaths.Add(new Path(fromPoint, toPoint2));
                        }

                        string toCode3 = $"{i}-{j + 1}";
                        PathPoint toPoint3 = _basePathPoints.FirstOrDefault(p => p.Code == toCode3);
                        if (toPoint3 != null)
                        {
                            _basePaths.Add(new Path(fromPoint, toPoint3));
                        }

                        string toCode4 = $"{i - 1}-{j + 1}";
                        PathPoint toPoint4 = _basePathPoints.FirstOrDefault(p => p.Code == toCode4);
                        if (toPoint4 != null)
                        {
                            _basePaths.Add(new Path(fromPoint, toPoint4));
                        }
                    }
                }
            }
        }

        public List<PathPoint> BasePathPoints { get { return _basePathPoints; } }
        private List<PathPoint> _basePathPoints = new List<PathPoint>();
        private void InitializeBasePoints()
        {
            _basePathPoints.Clear();
            int count1 = (int)(Width / GridSizeWidth);
            int count2 = (int)(Height / GridSizeHeight);

            for (int i = 0; i <= count1; i++)
            {
                for (int j = 0; j <= count2; j++)
                {
                    string code = $"{i}-{j}";
                    if (GridSizeWidth * i <= Width && GridSizeHeight * j <= Height)
                    {
                        Position pos = new Position(GridSizeWidth * i, GridSizeHeight * j);

                        _basePathPoints.Add(new PathPoint(code, pos));
                    }
                }
            }
        }
        public List<Line> PathLines { get; } = new List<Line>();
        public void AddLine(Line line)
        {
            if (line == null)
            {
                return;
            }
            PathLines.Add(line);
        }

        public List<Obstacle> Obstacles { get; } = new List<Obstacle>();
        public void AddObstacle(Obstacle obstacle)
        {
            if (Obstacles == null)
            {
                return;
            }

            Obstacles.Add(obstacle);
        }
    }


    public class PathPoint
    {
        public string Code { get; }
        public Position Position { get; }
        public PathPoint(string code, Position position)
        {
            Code = code;
            Position = position;
        }
    }
    public class Path
    {
        public PathPoint FromPosition { get; set; }
        public PathPoint ToPosition { get; set; }

        public float Weight { get; set; }

        public Path(PathPoint from, PathPoint to)
        {
            FromPosition = from;
            ToPosition = to;
            Weight = 0;
        }
    }
    public class Line
    {
        public List<Path> Paths { get { return _paths; } }
        private List<Path> _paths = new List<Path>();
        public Line()
        {

        }
        public Line(List<Path> paths)
        {
            _paths.AddRange(paths);
        }
    }
}
