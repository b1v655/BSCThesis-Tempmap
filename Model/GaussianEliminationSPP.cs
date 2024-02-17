using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Model
{
    class GaussianEliminationSPP
    {
        private double[] x;
        public GaussianEliminationSPP(double[,] A, double[] b) // Scaled Partial Pivoting
        {
          
            x = new double[b.Length]; 
            double[] s = new double[b.Length];
            int[] p = new int[b.Length];
            for (int i = 0; i < b.Length; i++)
            {
                s[i] = 0;
                for (int j = 0; j < b.Length; j++)
                {
                    s[i] = Math.Max(s[i], Math.Abs(A[i, j]));
                }
                p[i] = i;
            }
            for (int k = 0; k < b.Length - 1; k++)
            {
                double r_max = 0;
                int j=0;
                for (int i = k; i < b.Length; i++)
                {
                    double r = Math.Abs((A[p[i], k] / s[p[i]]));
                    if (r > r_max)
                    {
                        r_max = r;
                        j = i;
                    }
                }
                int temp = p[k];
                p[k] = p[j];
                p[j] = temp;
                for (int i = k + 1; i < b.Length; i++)
                {
                    A[p[i], k] = A[p[i], k] / A[p[k], k];
                    for(j=k+1;j<b.Length; j++)
                    {
                        A[p[i], j] = A[p[i], j] - A[p[i], k] * A[p[k], j];
                    }
                }
            }
            for (int k = 0; k < b.Length - 1; k++)
            {
                for (int i = k + 1; i < b.Length; i++)
                    b[p[i]] = b[p[i]] - A[p[i], k] * b[p[k]];

            }
            for (int i = b.Length - 1; i >= 0; i--)
            {
                double se = b[p[i]];
                for (int j = i + 1; j < b.Length; j++)
                {
                    se = se - A[p[i], j] * x[j];
                }
                x[i] = se / A[p[i], i];
            }
        }
        public double[] GetX()
        {

            return x;
        }
    }
}
