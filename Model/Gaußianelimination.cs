using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Model
{
    class Gaußianelimination
    {
        
        private double[] x;
        public Gaußianelimination(double[,] A, double[] b) 
        {
            x = new double[b.Length];
            for (int k=0;k<b.Length-1;k++)
            {
                for(int i=k+1;i<b.Length;i++)
                {
                    A[i, k] = A[i, k] / A[k, k];
                    for(int j = k + 1; j < b.Length; j++)
                    {
                        A[i, j] = A[i, j] - A[i, k] * A[k, j];
                    }
                }
            }
            for(int k = 0; k < b.Length - 1; k++)
            {
                for(int i=k+1; i < b.Length; i++)
                {
                    b[i] = b[i] - A[i, k] * b[k];
                }
            }
            for(int i = b.Length - 1; i >= 0; i--)
            {
                double s = b[i];
                for(int j = i + 1; j < b.Length; j++)
                {
                    s = s - A[i, j] * x[j];
                }
                x[i] = s / A[i, i];
            }
            
        }

        public double[] GetX()
        {

            return x;
        }

    }
}
