using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteeringBehavioursCore.Model.Field
{
    internal class FieldGenerator
    {
        public float FieldWidth { get; set; } = 1200f;
        public float FieldHeight { get; set; } = 600f;
        public float GridSizeWidth { get; set; } = 30f;
        public float GridSizeHeight { get; set; } = 30f;

        public FieldGenerator()
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
            int count1 = (int)(FieldWidth / GridSizeWidth);
            int count2 = (int)(FieldHeight / GridSizeHeight);

            for (int i = 0; i < count1 - 1; i++)
            {
                for (int j = 0; j < count2 - 1; j++)
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
                    }
                }
            }
        }

        public List<PathPoint> BasePathPoints {  get { return _basePathPoints; } }
        private List<PathPoint> _basePathPoints = new List<PathPoint>();
        private void InitializeBasePoints()
        {
            _basePathPoints.Clear();
            int count1 = (int)(FieldWidth / GridSizeWidth);
            int count2 = (int)(FieldHeight / GridSizeHeight);

            for (int i = 0; i < count1; i++)
            {
                for (int j = 0; j < count2; j++)
                {
                    string code = $"{i}-{j}";
                    if (GridSizeWidth * (i + 1) <= FieldWidth && GridSizeHeight * (i + 1) <= FieldHeight)
                    {
                        Position pos = new Position(GridSizeWidth * (i + 1), GridSizeHeight * (i + 1));

                        _basePathPoints.Add(new PathPoint(code, pos));
                    }
                }
            }
        }
    }
}
