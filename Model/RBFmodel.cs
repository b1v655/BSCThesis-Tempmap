using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Forms;
namespace WpfApp2.Model
{
    public enum psiFunctionType
    {
        MultiQuadric,
        InverseQuadratic,
        Gaußian,
        InverseMultiQuadric,
        ThinPlateSpline

    };
    public enum LESCalculate
    {
        CD,
        GE,
        GESPP
    }
    public class RBFmodel
    {
        public System.Drawing.Color[] colorize=
        {
            System.Drawing.Color.DarkBlue,
            System.Drawing.Color.Blue,
            System.Drawing.Color.Green,
            System.Drawing.Color.Yellow,
            System.Drawing.Color.Red,
            System.Drawing.Color.DarkRed
        };
        public bool cab=false;
        public double epsylon = 0.005;
        private double TableWidth, TableHeight;
        private List<Point> points;
        private byte[] pixels1d;
        private double[,] A;
        private double[] b;
        private double[] w;
        private LESCalculate LESSolve = LESCalculate.GE;
        private psiFunctionType BasisFunctionType = psiFunctionType.Gaußian;
        private Persistence.IRBFPersistenceDataAccess _dataAccess;
        public Point _choosedPoint;

        public void NewTable()
        {
            points = null;
        }
        public void ChangeRBFType(psiFunctionType param)
        {
            BasisFunctionType = param;
        }
        public void ChangeFT(LESCalculate param)
        {
            LESSolve = param;
            if (points != null)
            {
                CalculateVectors();
            }
        }
        public double Value(int x, int y)
        {
            if (cab)
                return FunctionValue(x, y);
            return Math.Round(FunctionValue(x, y));
        }

        public void AddPoint(double x, double y, double h)
        {
            if(points==null)
                points = new List<Point> { new Point(x, y, h) };
            else if (!points.Contains(new Point(x, y, h)))
                points.Add(new Point(x, y, h));
        }
        private void CalculateVectors()
        {
            A = new double[points.Count, points.Count];
            b = new double[points.Count];
            w = new double[points.Count];
            for (int i = 0; i < points.Count; i++)
            {
                for (int j = 0; j < points.Count; j++)
                {
                    A[i, j] = Phi(points[i], points[j], BasisFunctionType);
                }
                b[i] = points[i].h;
            }
            w = VectorCalculate(LESSolve, A, b);
        }
        private double[] VectorCalculate(LESCalculate t, double[,] A, double[] b)
        {
            switch (t)
            {
                case LESCalculate.CD: return new CholeskyDecomposition(A, b).GetX();
                case LESCalculate.GE: return new Gaußianelimination(A, b).GetX();
                case LESCalculate.GESPP: return new GaussianEliminationSPP(A, b).GetX();
            }
            return new double[0];
        }


        public bool RemovePoint()
        {
            if (_choosedPoint == null) return false;
            points.Remove(_choosedPoint);
            _choosedPoint = null;
            CalculateVectors();
            return true;
            
        }
        private double Phi(Point x, Point y, psiFunctionType t)
        {
            switch (t)
            {
                case psiFunctionType.Gaußian:
                    return Math.Pow(Math.E, -Math.Pow(epsylon * TwoNorm(new Point(x.x-y.x,x.y-y.y,0)), 2));
                case psiFunctionType.InverseMultiQuadric:
                    return 1 / Math.Sqrt(1 + Math.Pow(epsylon * TwoNorm(new Point(x.x - y.x, x.y - y.y, 0)), 2));
                case psiFunctionType.MultiQuadric:
                    return Math.Sqrt(1 + Math.Pow(epsylon * TwoNorm(new Point(x.x - y.x, x.y - y.y, 0)), 2));
                case psiFunctionType.InverseQuadratic:
                    return 1 / (1 + Math.Pow(epsylon * TwoNorm(new Point(x.x - y.x, x.y - y.y, 0)), 2));
                case psiFunctionType.ThinPlateSpline:
                    if(TwoNorm(new Point(x.x - y.x, x.y - y.y, 0)) <= 0)
                    {
                        return 0;
                    }
                    return Math.Pow(TwoNorm(new Point(x.x - y.x, x.y - y.y, 0)), 2) * Math.Log(TwoNorm(new Point(x.x - y.x, x.y - y.y, 0)));
            }
            return 0;

        }

        private double TwoNorm(Point z)
        {return Math.Sqrt(Math.Pow(z.x, 2) + Math.Pow(z.y , 2));
        }

        private double FunctionValue(double x, double y)
        {
            double sum = 0.0;
            for (int i = 0; i < w.Length; i++)
            {
                sum = sum + w[i] * Phi(new Point(x, y, 0), points[i], BasisFunctionType);
            }
            return sum;
        }

        public RBFmodel(Persistence.IRBFPersistenceDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }
        private System.Drawing.Color SetColor()
        {
            System.Drawing.Color mycolor = new System.Drawing.Color();

            ColorDialog MyDialog = new ColorDialog();
            MyDialog.FullOpen = true;
            
            if (MyDialog.ShowDialog() == DialogResult.OK)
                mycolor=MyDialog.Color;
            
            return mycolor;
        }
       
        private double[] FindGlobalMaximumandMinimumValue()
        {
            double[] minmax= { -10, 10 };
            if (cab)
            {
                minmax[0] = -10;
                minmax[1] = 10;
            }

            for (int i = 0; i < TableHeight; i+=1)
            {
                for(int j=0; j < TableWidth; j+=1)
                {
                    double fxy;
                    if(!cab)
                        fxy = Math.Round(FunctionValue(i, j));
                    else
                        fxy = FunctionValue(i, j);
                    if (fxy > minmax[1])
                        minmax[1] = fxy;
                    if (fxy < minmax[0])
                        minmax[0] = fxy;
                }
            }
            return minmax;
        }
        public WriteableBitmap GetColors(int width, int height) //optimalizálni
        {


            double globalMaximum = FindGlobalMaximumandMinimumValue()[1];
            double globalMinimum = FindGlobalMaximumandMinimumValue()[0];
            WriteableBitmap wbitmap = new WriteableBitmap(
               width, height, 96, 96, PixelFormats.Bgra32, null);
            Int32Rect rect = new Int32Rect(0, 0, width, height);
            Int32 stride = 4 * width;
            byte[] pixels1d = new byte[height * width * 4];
            int index = 0;
            for (Int32 i = 0; i < height; i++)
            {
                for (Int32 j = 0; j < width; j++)

                {

                    int fxy =  1;

                    for (int k = 0; k < 5; k++)
                    {
                        if (fxy >= globalMinimum + (globalMaximum - globalMinimum) / (colorize.Length - 1) * k && fxy < globalMinimum + (globalMaximum - globalMinimum) / (colorize.Length - 1) * (k + 1))
                        {
                            double actualMin = globalMinimum + (globalMaximum - globalMinimum) / (colorize.Length - 1) * k;
                            double actualMax = globalMinimum + (globalMaximum - globalMinimum) / (colorize.Length - 1) * (k + 1);
                            pixels1d[index++] = (byte)(colorize[k].B + (fxy - actualMin) / (actualMax - actualMin) * (colorize[k + 1].B - colorize[k].B));
                            pixels1d[index++] = (byte)(colorize[k].G + (fxy - actualMin) / (actualMax - actualMin) * (colorize[k + 1].G - colorize[k].G));
                            pixels1d[index++] = (byte)(colorize[k].R + (fxy - actualMin) / (actualMax - actualMin) * (colorize[k + 1].R - colorize[k].R));
                            pixels1d[index++] = (byte)(colorize[k].A + (fxy - actualMin) / (actualMax - actualMin) * (colorize[k + 1].A - colorize[k].A));
                        }
                    }
                    if (fxy == globalMaximum)
                    {
                        pixels1d[index++] = (byte)colorize[colorize.Length - 1].B;
                        pixels1d[index++] = (byte)colorize[colorize.Length - 1].G;
                        pixels1d[index++] = (byte)colorize[colorize.Length - 1].R;
                        pixels1d[index++] = (byte)colorize[colorize.Length - 1].A;
                    }
                }

            }

            wbitmap.WritePixels(rect, pixels1d, stride, 0);
            return wbitmap;
        }
        public byte[] writePoints(int width, int height, byte[] array)
        {
            int r=5;
            foreach (Point _point in points)
            {
                for (int i = (int)_point.x - r; i <= (int)_point.x + r; i++)
                {
                    for (int j = (int)_point.y - r; j <= (int)_point.y + r; j++)
                    {
                        if (i < height && j < width && i > 0 && j > 0 && Math.Sqrt(Math.Pow((int)_point.x - i, 2) + Math.Pow((int)_point.y - j, 2)) < r)
                        {
                            if (_choosedPoint!= _point)
                            {
                                array[i * width * 4 + j * 4] = 255;
                                array[i * width * 4 + j * 4 + 1] = 255;
                                array[i * width * 4 + j * 4 + 2] = 255;
                                array[i * width * 4 + j * 4 + 3] = 0;
                            }
                            else
                            {
                                array[i * width * 4 + j * 4] = 100;
                                array[i * width * 4 + j * 4 + 1] = 100;
                                array[i * width * 4 + j * 4 + 2] = 100;
                                array[i * width * 4 + j * 4 + 3] = 255;
                            }

                        }
                        if (i < height && j < width && i > 0 && j > 0 && Math.Sqrt(Math.Pow((int)_point.x - i, 2) + Math.Pow((int)_point.y - j, 2)) < r+1&& Math.Sqrt(Math.Pow((int)_point.x - i, 2) + Math.Pow((int)_point.y - j, 2)) >= r )
                        {
                            array[i * width * 4 + j * 4] = 0;
                            array[i * width * 4 + j * 4 + 1] = 0;
                            array[i * width * 4 + j * 4 + 2] = 0;

                            array[i * width * 4 + j * 4 + 3] = 255;
                        }
                    }
                }
            }
            return array;
        }
        public void PointIncrease()
        {
            if (_choosedPoint == null)
            {
                return;
            }
            foreach(Point _point in points)
            {
                if(_point==_choosedPoint)
                    _point.h++;
            }
            
        }
        public void PointDecrease()
        {
            if (_choosedPoint == null)
            {
                return;
            }
            foreach (Point _point in points)
            {
                if (_point == _choosedPoint)
                    _point.h--;
            }

        }
        public WriteableBitmap SelectPoint(int width, int height,int x, int y)
        {
            if (points == null)
            {
                return null;
            }

            WriteableBitmap wbitmap = new WriteableBitmap(
                width, height, 96, 96, PixelFormats.Bgra32, null);
            if (pixels1d.Length!= height * width * 4) wbitmap= GetMap(width, height);
            _choosedPoint = null;
            pixels1d = writePoints(width, height, pixels1d);
            int r = 5;
            foreach (Point _point in points)
            {
                if (Math.Sqrt(Math.Pow((int)_point.x - x, 2) + Math.Pow((int)_point.y - y, 2)) < r + 1)
                {
                    
                    for (int i = (int)_point.x - r; i <= (int)_point.x + r; i++)
                    {
                        for (int j = (int)_point.y - r; j <= (int)_point.y + r; j++)
                        {
                            if (i < height && j < width && i > 0 && j > 0 && Math.Sqrt(Math.Pow((int)_point.x - i, 2) + Math.Pow((int)_point.y - j, 2)) < r)
                            {
                                pixels1d[i * width * 4 + j * 4] = 100;
                                pixels1d[i * width * 4 + j * 4 + 1] = 100;
                                pixels1d[i * width * 4 + j * 4 + 2] = 100;
                                pixels1d[i * width * 4 + j * 4 + 3] = 255;
                            }
                            if (i < height && j < width && i > 0 && j > 0 && Math.Sqrt(Math.Pow((int)_point.x - i, 2) + Math.Pow((int)_point.y - j, 2)) < r + 1 && Math.Sqrt(Math.Pow((int)_point.x - i, 2) + Math.Pow((int)_point.y - j, 2)) >= r)
                            {
                                pixels1d[i * width * 4 + j * 4] = 0;
                                pixels1d[i * width * 4 + j * 4 + 1] = 0;
                                pixels1d[i * width * 4 + j * 4 + 2] = 0;
                                pixels1d[i * width * 4 + j * 4 + 3] = 255;
                            }
                        }
                    }
                    _choosedPoint = _point;
                }
            }
            Int32Rect rect = new Int32Rect(0, 0, width, height);
            Int32 stride = 4 * width;
            wbitmap.WritePixels(rect, pixels1d, stride, 0);
            return wbitmap;
        }
        public WriteableBitmap GetMap(int width, int height)
        {
            if (points == null)
            {
                return null;
            }

            CalculateVectors();
            TableWidth = width;
            TableHeight = height;
            double globalMaximum = FindGlobalMaximumandMinimumValue()[1];
            double globalMinimum = FindGlobalMaximumandMinimumValue()[0];
            WriteableBitmap wbitmap = new WriteableBitmap(
                width, height, 96, 96, PixelFormats.Bgra32, null);
            pixels1d = new byte[height * width * 4];
            for (Int32 i = 0; i < height; i++)
            {
                for (Int32 j = 0; j < width; j++)
                {
                    double fxy;
                    if (cab)
                        fxy = FunctionValue(i, j);
                    else
                        fxy = Math.Round(FunctionValue(i, j));

                    for (int k = 0; k < 5; k++)
                    {
                        if (fxy >= globalMinimum + (globalMaximum - globalMinimum) / (colorize.Length - 1) * k && fxy < globalMinimum + (globalMaximum - globalMinimum) / (colorize.Length - 1) * (k + 1))
                        {
                            double actualMin = globalMinimum + (globalMaximum - globalMinimum) / (colorize.Length - 1) * k;
                            double actualMax = globalMinimum + (globalMaximum - globalMinimum) / (colorize.Length - 1) * (k + 1);
                            pixels1d[i*width*4+j*4] = (byte)(colorize[k].B + (fxy - actualMin) / (actualMax - actualMin) * (colorize[k + 1].B - colorize[k].B));
                            pixels1d[i * width * 4 + j * 4+1] = (byte)(colorize[k].G + (fxy - actualMin) / (actualMax - actualMin) * (colorize[k + 1].G - colorize[k].G));
                            pixels1d[i * width * 4 + j * 4+2] = (byte)(colorize[k].R + (fxy - actualMin) / (actualMax - actualMin) * (colorize[k + 1].R - colorize[k].R));
                            pixels1d[i * width * 4 + j * 4+3] = (byte)(colorize[k].A + (fxy - actualMin) / (actualMax - actualMin) * (colorize[k + 1].A - colorize[k].A));
                        }
                    }
                    if (fxy == globalMaximum)
                    {
                        pixels1d[i * width * 4 + j * 4 ] = (byte)colorize[colorize.Length - 1].B;
                        pixels1d[i * width * 4 + j * 4 + 1] = (byte)colorize[colorize.Length - 1].G;
                        pixels1d[i * width * 4 + j * 4 + 2] = (byte)colorize[colorize.Length - 1].R;
                        pixels1d[i * width * 4 + j * 4 + 3] = (byte)colorize[colorize.Length - 1].A;
                    }
                }
            }
            pixels1d=writePoints(width, height, pixels1d);
            Int32Rect rect = new Int32Rect(0, 0, width, height);
            Int32 stride = 4 * width;
            wbitmap.WritePixels(rect, pixels1d, stride, 0);
            return wbitmap;

        }
        public WriteableBitmap GetThreeDMap(int width, int height, int unit)
        {
            if (points == null)
            {
                return null;
            }

            double globalMaximum = FindGlobalMaximumandMinimumValue()[1];
            double globalMinimum = FindGlobalMaximumandMinimumValue()[0];
            TableWidth = width;
            TableHeight = height;
            WriteableBitmap wbitmap = new WriteableBitmap(
                width, height, 96, 96, PixelFormats.Bgra32, null);
            byte[] TDPixels = new byte[height * width * 4];
            for (Int32 x = 0; x < width; x+=unit)
            {
                for (Int32 y = 0; y < height; y ++)
                {

                    double fxy = FunctionValue(y, x);
                    int v = 160 + x / 2 + y / 4;
                    int u = 120 + y / 4 ;
                    double uz = 120 + y / 4 - 5 * fxy;
                    for (int k = 0; k < 5; k++)
                    {
                        if (fxy >= globalMinimum + (globalMaximum - globalMinimum) / (colorize.Length - 1) * k && fxy < globalMinimum + (globalMaximum - globalMinimum) / (colorize.Length - 1) * (k + 1))
                        {
                            //TDPixels[u*width*4 + v * 4] = 0;
                            //TDPixels[u * width * 4 + v * 4 + 1] = 0;
                            //TDPixels[u * width * 4 + v * 4 + 2] = 0;
                            //TDPixels[u * width * 4 + v * 4 + 3] = 255;
                            double actualMin = globalMinimum + (globalMaximum - globalMinimum) / (colorize.Length - 1) * k;
                            double actualMax = globalMinimum + (globalMaximum - globalMinimum) / (colorize.Length - 1) * (k + 1);
                            TDPixels[(int)uz * width * 4 + v * 4] = (byte)(colorize[k].B + (fxy - actualMin) / (actualMax - actualMin) * (colorize[k + 1].B - colorize[k].B));
                            TDPixels[(int)uz * width * 4 + v * 4 + 1] = (byte)(colorize[k].G + (fxy - actualMin) / (actualMax - actualMin) * (colorize[k + 1].G - colorize[k].G));
                            TDPixels[(int)uz * width * 4 + v * 4 + 2] = (byte)(colorize[k].R + (fxy - actualMin) / (actualMax - actualMin) * (colorize[k + 1].R - colorize[k].R));
                            TDPixels[(int)uz * width * 4 + v * 4 + 3] = (byte)(colorize[k].A + (fxy - actualMin) / (actualMax - actualMin) * (colorize[k + 1].A - colorize[k].A));
                        }
                    }
                }
            }
            for (Int32 x = 0; x <width; x ++)
            {
                for (Int32 y = 0; y < height; y+=unit)
                {
                    double fxy = FunctionValue(y, x);
                    int v = 160 + x / 2 + y / 4;
                    int u = 120 + y / 4;
                    double uz = 120 + y / 4 - 5 * fxy;
                    for (int k = 0; k < 5; k++)
                    {
                        if (fxy >= globalMinimum + (globalMaximum - globalMinimum) / (colorize.Length - 1) * k && fxy < globalMinimum + (globalMaximum - globalMinimum) / (colorize.Length - 1) * (k + 1))
                        {
                            //TDPixels[u*width*4 + v * 4] = 0;
                            //TDPixels[u * width * 4 + v * 4 + 1] = 0;
                            //TDPixels[u * width * 4 + v * 4 + 2] = 0;
                            //TDPixels[u * width * 4 + v * 4 + 3] = 255;
                            double actualMin = globalMinimum + (globalMaximum - globalMinimum) / (colorize.Length - 1) * k;
                            double actualMax = globalMinimum + (globalMaximum - globalMinimum) / (colorize.Length - 1) * (k + 1);
                            TDPixels[(int)uz * width * 4 + v * 4] = (byte)(colorize[k].B + (fxy - actualMin) / (actualMax - actualMin) * (colorize[k + 1].B - colorize[k].B));
                            TDPixels[(int)uz * width * 4 + v * 4 + 1] = (byte)(colorize[k].G + (fxy - actualMin) / (actualMax - actualMin) * (colorize[k + 1].G - colorize[k].G));
                            TDPixels[(int)uz * width * 4 + v * 4 + 2] = (byte)(colorize[k].R + (fxy - actualMin) / (actualMax - actualMin) * (colorize[k + 1].R - colorize[k].R));
                            TDPixels[(int)uz * width * 4 + v * 4 + 3] = (byte)(colorize[k].A + (fxy - actualMin) / (actualMax - actualMin) * (colorize[k + 1].A - colorize[k].A));
                        }
                    }
                }
            }
            pixels1d = writePoints(width, height, pixels1d);
            Int32Rect rect = new Int32Rect(0, 0, width, height);
            Int32 stride = 4 * width;
            wbitmap.WritePixels(rect, TDPixels, stride, 0);
            return wbitmap;

        }
        public void SetFirstColor()
        {
            colorize[0] = SetColor();
        }
        public void SetSecondColor()
        {
            colorize[1] = SetColor();
        }
        public void SetThirdColor()
        {
            colorize[2] = SetColor();
        }
        public void SetForthColor()
        {
            colorize[3] = SetColor();
        }
        public void SetFifthColor()
        {
            colorize[4] = SetColor();
        }
        public void SetSixthColor()
        {
            colorize[5] = SetColor();
        }
        public async Task LoadGame(String path)
        {
           
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");
            points = null;
            Tuple<System.Drawing.Color[] , Point[]> Loaded_data = await _dataAccess.LoadAsync(path);
            colorize = Loaded_data.Item1;
           
            foreach (Point i in Loaded_data.Item2)
            {
                if (points == null)
                    points = new List<Point> { i };
                else
                    points.Add(i);
            }
            CalculateVectors();
        }
        public async Task SaveGame(String path)
        {
            
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");
            if (points == null)
                throw new InvalidOperationException("No data access is provided.");
            await _dataAccess.SaveAsync(path, Tuple.Create(colorize,points.ToArray()));
            
        }
        public void saveMap(System.Windows.Controls.Canvas ParentCanvas,String path)
        {
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)ParentCanvas.Width,
        (int)ParentCanvas.Height, 96d, 96d, System.Windows.Media.PixelFormats.Default);
            rtb.Render(ParentCanvas);

            var crop = new CroppedBitmap(rtb, new Int32Rect(0, 0, (int)ParentCanvas.Width, (int)ParentCanvas.Height));

            BitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(crop));

            using (var fs = System.IO.File.OpenWrite(path))
            {
                pngEncoder.Save(fs);
            }
        }

    }
}
