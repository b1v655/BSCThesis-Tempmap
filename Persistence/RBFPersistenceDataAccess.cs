using System;
using System.IO;
using System.Threading.Tasks;

namespace WpfApp2.Persistence
{
    public class RBFPersistenceDataAccess:IRBFPersistenceDataAccess
    {
        public async Task<Tuple<System.Drawing.Color[], Model.Point[]>> LoadAsync(String path)
        {

            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    String line = await reader.ReadLineAsync();
                    String[] numbers = line.Split(' ');
                    System.Drawing.Color[] Colors = new System.Drawing.Color[numbers.Length / 4];
                    for (int i = 0; i < numbers.Length / 4; i++)
                    {

                        Colors[i] = System.Drawing.Color.FromArgb(byte.Parse(numbers[i * 4 + 3]),byte.Parse(numbers[i * 4 + 2]), byte.Parse(numbers[i * 4 + 1]), byte.Parse(numbers[i * 4]));
                 
                    }
                    line = await reader.ReadLineAsync();
                    numbers = line.Split(' ');

                    Model.Point[] Points = new Model.Point[numbers.Length/3];
                    for (int i=0; i <numbers.Length/3;i++)
                    {
                        Points[i] =  new Model.Point(Int32.Parse(numbers[i * 3]), Int32.Parse(numbers[i * 3+1]),Int32.Parse(numbers[i * 3+2]));
                    }

                    Tuple<System.Drawing.Color[], Model.Point[]> ToRet = Tuple.Create(Colors,Points);
                    return ToRet;
                }
            }
            catch
            {
                throw new RBFPersistenceException();
            }
        }
        public async Task SaveAsync(String path, Tuple<System.Drawing.Color[], Model.Point[]> OutPut)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path)) 
                {
                    
                   
                    string colorsToString="" ;
                    for(int i = 0; i < OutPut.Item1.Length; i++)
                    {
                        colorsToString += OutPut.Item1[i].B + " " + OutPut.Item1[i].G + " "+ OutPut.Item1[i].R + " "+ OutPut.Item1[i].A +" " ;
                    }
                    await writer.WriteLineAsync(colorsToString);
                    String pointsToString = "";
                    for (Int32 i = 0; i < OutPut.Item2.Length; i++)
                    {
                        pointsToString += OutPut.Item2[i].x + " " + OutPut.Item2[i].y + " " + OutPut.Item2[i].h + " ";
                    }
                    await writer.WriteLineAsync(pointsToString);
                }
            }
            catch
            {
                throw new RBFPersistenceException();
            }
        }

    }
}
