using System;
using System.Threading.Tasks;

namespace DigitDetector
{
    [Serializable]
    public class Matrix
    {
        private float[][] _matrix;
        readonly Random rnd = new Random();

        public Matrix(int rows, int cols)
        {
            _matrix = new float[rows][];

            for (var i = 0; i < rows; i++)
            {
                _matrix[i] = new float[cols];
            }
        }

        private Matrix(float[][] array)
        {
            _matrix = array;
        }
        public Matrix(){}

        public Matrix ExpMinus()
        {
            Matrix res = new Matrix(Value.Length,Value[0].Length);

            for (int i = 0; i < res.Value.Length; i++)
            {
                for (int j = 0; j < res.Value[0].Length; j++)
                    res.Value[i][j] = (float) Math.Exp(-Value[i][j]);
            }
            return res;
        }

        public int MaxInd()
        {
            float max = Single.MinValue;
            int maxInd = 0;
            for (int i = 0; i < this.Value.Length; ++i)
            {
                for (int j = 0; j < this.Value[0].Length; ++j)
                {
                    if (Value[i][j] > max)
                    {
                        max = Value[i][j];
                        maxInd = i;
                    }
                }
            }
            if (max > 0.5)
                return maxInd;
            else
                return 20;
        }

        public void Randomize()
        {
            for (int i = 0; i < Value.Length; ++i)
                for (int j = 0; j < Value[0].Length; ++j)
                    Value[i][j] = (float) (SampleGaussian() / 10.0);
        }

        public float SampleGaussian()
        {
            double x1 = 1 - rnd.NextDouble();
            double x2 = 1 - rnd.NextDouble();
            return (float)Math.Sqrt(-2.0 * Math.Log(x1)) * (float)Math.Cos(2.0 * Math.PI * x2);
        }

        public static Matrix Mat1D(float[,] mat, int InputNodeN)
        {
            Matrix vec = new Matrix(InputNodeN, 1);

            for (int i = 0; i < mat.GetLength(0); ++i)
                for (int j = 0; j < mat.GetLength(1); ++j)
                {
                    vec.Value[j + i * mat.GetLength(1)][0] = mat[i, j] / 255f;
                }
            return vec;
        }

        private static float[][] CreateJagged(int rows, int cols)
        {
            var jagged = new float[rows][];

            for (var i = 0; i < rows; i++)
            {
                jagged[i] = new float[cols];
            }
            return jagged;
        }

        public static Matrix Create(float[][] array)
        {
            return new Matrix(array);
        }

        public float[][] Value
        {
            get
            {
                return _matrix;
            }
        }

        public static Matrix operator +(float b, Matrix a)
        {
            var newMatrix = CreateJagged(a.Value.Length, a.Value[0].Length);
            for (var x = 0; x < a.Value.Length; x++)
            {
                for (var y = 0; y < a.Value[x].Length; y++)
                {
                    newMatrix[x][y] = a.Value[x][y] + b;
                }
            }

            return Create(newMatrix);
        }

        public static Matrix operator * (Matrix a, Matrix b)
        {
            if (a.Value.Length == b.Value.Length && a.Value[0].Length == b.Value[0].Length)
            {
                var newMatrix1 = CreateJagged(a.Value.Length, a.Value[0].Length);

                Parallel.For(0, a.Value.Length, i =>
                {
                    for (var j = 0; j < a.Value[i].Length; j++)
                    {
                        newMatrix1[i][j] = a.Value[i][j] * b.Value[i][j];
                    }
                });

                return Create(newMatrix1);
            }

            var newMatrix = CreateJagged(a.Value.Length, b.Value[0].Length);

            if (a.Value[0].Length == b.Value.Length)
            {
                var length = a.Value[0].Length;

                Parallel.For(0, a.Value.Length, i =>
                {
                    for (var j = 0; j < b.Value[0].Length; j++)
                    {
                        var temp = 0f;

                        for (var k = 0; k < length; k++)
                        {
                            temp += a.Value[i][k] * b.Value[k][j];
                        }

                        newMatrix[i][j] = temp;
                    }
                });
            }

            return Create(newMatrix);
        }

        public static Matrix operator /(float scalar, Matrix b)
        {
            var newMatrix = CreateJagged(b.Value.Length, b.Value[0].Length);

            for (var x = 0; x < b.Value.Length; x++)
            {
                for (var y = 0; y < b.Value[x].Length; y++)
                {
                    newMatrix[x][y] = scalar / b.Value[x][y];
                }
            }

            return Create(newMatrix);
        }

    }
}