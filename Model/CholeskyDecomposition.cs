using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Model
{
    class CholeskyDecomposition
    {
        private double[] x;
        private bool IsPositiveDefinite;
        public CholeskyDecomposition(double [,] A,double[] b)
        {
            x = new double[b.Length]; 
            IsPositiveDefinite = true;
            double[,] LeftTriangleMatrix = new double[b.Length, b.Length];
            for (int i=0;i<b.Length; i++)
            {
                
               
                for(int j=0; j <= i ; j++)
                {
                    if(j==i)
                    {
                        double sum = 0;
                        for(int k=0;k<j;k++)
                        {
                            sum += LeftTriangleMatrix[j, k] * LeftTriangleMatrix[j, k];
                        }
                        if (Math.Sqrt(A[j, j] - sum) > 0)
                            LeftTriangleMatrix[j, j] = Math.Sqrt(A[j, j] - sum);
                        else
                            IsPositiveDefinite = false;
                    }
                    else
                    {
                        double sum = 0;
                        for (int k = 0; k < j; k++)
                            sum += LeftTriangleMatrix[i, k] * LeftTriangleMatrix[j, k];
                        LeftTriangleMatrix[i,j] = 1.0 / LeftTriangleMatrix[j, j] * (A[i, j] - sum);
                    }
                }

            }
            if (IsPositiveDefinite)
                x = new Gaußianelimination(Transpose(LeftTriangleMatrix), new Gaußianelimination(LeftTriangleMatrix, b).GetX()/*Y*/).GetX();
            else
                x = new Gaußianelimination(A, b).GetX();
        }
        private double[,] Transpose(double[,] matrix)
        {
            int w = matrix.GetLength(0);
            int h = matrix.GetLength(1);

            double[,] result = new double[h, w];

            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    result[j, i] = matrix[i, j];
                }
            }

            return result;
        }
        public double[] GetX()
        {
            return x;
        }
    }
}
