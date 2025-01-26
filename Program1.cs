using System;

namespace matrix_c
{
    public class Matrix
    {


        public int N { get; private set; }
        public int M { get; private set; }
        internal double[,] data;

        public Matrix(int n, int m)
        {
            if (n <= 0 || m <= 0)
            {
                throw new Exception("Размеры матрицы должны быть больше 0");
            }
            N = n;
            M = m;
            data = new double[n, m];
        }

        public void FillMatrix()
        {
            Random rnd = new Random();
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    data[i, j] = rnd.Next(1, 10);
                }
            }
        }

        public void ShowMatrix()
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    Console.Write($"{data[i, j]} ");
                }
                Console.WriteLine();
            }
        }

        public static Matrix operator +(Matrix a, Matrix b)
        {
            if (a.N != b.N || a.M != b.M)
            {
                throw new ArgumentException("Матрицы должны быть одинакового размера для сложения.");
            }
            Matrix result = new Matrix(a.N, a.M);
            for (int i = 0; i < a.N; i++)
            {
                for (int j = 0; j < a.M; j++)
                {
                    result.data[i, j] = a.data[i, j] + b.data[i, j];
                }
            }
            return result;
        }

        public static Matrix operator *(Matrix a, double num)
        {
            Matrix result = new Matrix(a.N, a.M);
            for (int i = 0; i < a.N; i++)
            {
                for (int j = 0; j < a.M; j++)
                {
                    result.data[i, j] = a.data[i, j] * num;
                }
            }
            return result;
        }

        public static Matrix operator *(double num, Matrix a)
        {
            return a * num;
        }

        public static Matrix operator *(Matrix a, Matrix b)
        {
            if (a.M != b.N)
            {
                throw new ArgumentException("Количество столбцов первой матрицы должно быть равно количеству строк второй матрицы.");
            }
            Matrix result = new Matrix(a.N, b.M);
            for (int i = 0; i < a.N; i++)
            {
                for (int j = 0; j < b.M; j++)
                {
                    for (int k = 0; k < a.M; k++)
                    {
                        result.data[i, j] += a.data[i, k] * b.data[k, j];
                    }
                }
            }
            return result;
        }

        public static Matrix operator /(Matrix a, double num)
        {
            if (num == 0)
            {
                throw new DivideByZeroException("Деление на ноль недопустимо.");
            }
            Matrix result = new Matrix(a.N, a.M);
            for (int i = 0; i < a.N; i++)
            {
                for (int j = 0; j < a.M; j++)
                {
                    result.data[i, j] = a.data[i, j] / num;
                }
            }
            return result;
        }
        public int Rank()
         {
             var reducedMatrix = ReduceToRowEchelonForm();  // сруз ступ вид
             int rank = 0;

             // проходим по строкам и проверяем  является ли строка нулевой
             for (int i = 0; i < N; i++)
             {
                 bool rowIsZero = true;
                 for (int j = 0; j < M; j++)
                 {
                     if (Math.Abs(reducedMatrix.data[i, j]) > 0)  
                     {
                         rowIsZero = false;
                         break;
                     }
                 }

                 // если строка не нулева увеличиваем ранг
                 if (!rowIsZero)
                 {
                     rank++;
                 }
             }

             return rank;
         }
        
        public Matrix ReduceToRowEchelonForm()
        {
            double[,] matrixCopy = (double[,])data.Clone(); 
            int lead = 0; 
            for (int row = 0; row < N; row++)
            {
                if (lead >= M)
                    return new Matrix(N, M) { data = matrixCopy }; 

                
                int i = row;
                while (Math.Abs(matrixCopy[i, lead]) <0)
                {
                    i++;
                    if (i == N) 
                    {
                        lead++;
                        if (lead == M)
                            return new Matrix(N, M) { data = matrixCopy }; // выходим 
                        i = row; // возвращаемся к текущей строке
                    }
                }

                // свап строк
                if (i != row)
                {
                    swapRows(matrixCopy, row, i);
                    //Console.WriteLine($"Обмен строк {row + 1} и {i + 1}"
                    //);
                }

                double div = matrixCopy[row, lead];
                if (Math.Abs(div) <0)
                    throw new Exception("Деление на ноль!");

                for (int j = 0; j < M; j++)
                {
                    matrixCopy[row, j] /= div; 
                }

                // обнуляем элементы ниже лида
                for (int i2 = row + 1; i2 < N; i2++)
                {
                    double factor = matrixCopy[i2, lead];
                    for (int j2 = 0; j2 < M; j2++)
                    {
                        matrixCopy[i2, j2] -= factor * matrixCopy[row, j2]; 
                    }
                }

                lead++; 
            }
            // преходим к исхдодной матрце
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    data[i, j] = matrixCopy[i, j];
                }
            }

            return this; 
        }


        private static void swapRows(double[,] matrix, int row1, int row2)
        {
            int cols = matrix.GetLength(1);
            for (int i = 0; i < cols; i++)
            {
                double temp = matrix[row1, i];
                matrix[row1, i] = matrix[row2, i];
                matrix[row2, i] = temp;
            }
        }

        private static double[] GetRow(double[,] matrix, int rowIndex)
        {
            int cols = matrix.GetLength(1);
            double[] row = new double[cols];
            for (int j = 0; j < cols; j++)
            {
                row[j] = matrix[rowIndex, j];
            }
            return row;
        }



        public Matrix tranponation()
        {
            Matrix result = new Matrix(M, N);
            for (int i = 0; i < M; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    result.data[i, j] = data[j, i];
                }
            }
            return result;

        }

        public class SquareMatrix : Matrix
        {
            public SquareMatrix(int n) : base(n, n)
            {
            }

            public static SquareMatrix operator *(SquareMatrix a, SquareMatrix b)
            {
                if (a.N != b.N)
                {
                    throw new ArgumentException("Обе матрицы должны быть квадратными и одинакового размера.");
                }
                Matrix result = (Matrix)a * (Matrix)b;
                return new SquareMatrix(result.N)
                {
                    data = result.data
                };
            }

            public static SquareMatrix operator *(SquareMatrix a, double num)
            {
                Matrix result = (Matrix)a * num;
                return new SquareMatrix(result.N)
                {
                    data = result.data
                };
            }

            public static SquareMatrix operator *(double num, SquareMatrix a)
            {
                return a * num;
            }

            public static SquareMatrix operator +(SquareMatrix a, SquareMatrix b)
            {
                if (a.N != b.N)
                {
                    throw new ArgumentException("Обе матрицы должны быть квадратными и одинакового размера.");
                }
                Matrix result = (Matrix)a + (Matrix)b;
                return new SquareMatrix(result.N)
                {
                    data = result.data
                };
            }

            public static SquareMatrix operator /(SquareMatrix a, double num)
            {
                Matrix result = (Matrix)a / num;
                return new SquareMatrix(result.N)
                {
                    data = result.data
                };
            }

            public double Determinant()
            {
                if (N == 1) return data[0, 0];
                if (N == 2) return data[0, 0] * data[1, 1] - data[0, 1] * data[1, 0];

                double det = 0;
                for (int i = 0; i < N; i++)
                {
                    det += Math.Pow(-1, i) * data[0, i] * Minor(0, i).Determinant();
                }
                return det;
            }

            private SquareMatrix Minor(int row, int col)
            {
                SquareMatrix minor = new SquareMatrix(N - 1);
                for (int i = 0, mi = 0; i < N; i++)
                {
                    if (i == row) continue;
                    for (int j = 0, mj = 0; j < N; j++)
                    {
                        if (j == col) continue;
                        minor.data[mi, mj] = data[i, j];
                        mj++;
                    }
                    mi++;
                }
                return minor;
            }
            public SquareMatrix Inverse()
            {
               

                double det = Determinant();
                if (Math.Abs(det) == 0)
                {
                    throw new Exception("Матрица вырожденная, обратной не существует.");
                }
              
                SquareMatrix inverseMatrix = new SquareMatrix(N);

                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        double minorDet = Minor(i, j).Determinant();
                        inverseMatrix.data[j, i] = Math.Pow(-1, i + j) * minorDet;
                    }
                }

                return inverseMatrix / det;
            }

        }

        internal class Program
        {
            static void Main(string[] args)
            {
                Console.WriteLine("Матрицы");
                Console.WriteLine("Введите n");
                int n = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Введите m");
                int m = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Матрица А");
                Matrix A = new Matrix(n, m);
                A.FillMatrix();
                A.ShowMatrix();
                Console.WriteLine();

                Console.WriteLine("Матрица А транспонированная");
                Matrix At = A.tranponation();
                At.ShowMatrix();
                Console.WriteLine();

                Console.WriteLine("Матрица А в ступенчатом ввиде");
                At.tranponation();
                At.ReduceToRowEchelonForm();
                At.ShowMatrix();
                Console.WriteLine();

                Console.WriteLine("Введите n1");
                int n1 = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Введите m1");
                int m1 = Convert.ToInt32(Console.ReadLine());
                Matrix B = new Matrix(n1, m1);
                Console.WriteLine();
                Console.WriteLine("Матрица B");
                B.FillMatrix();
                B.ShowMatrix();
                Matrix C = A + B;
                Console.WriteLine("");
                Console.WriteLine("Матрица C = A + B");
                C.ShowMatrix();
                Console.WriteLine();

                Matrix D = A * B;
                Matrix D1 = A * 3;
                Matrix D2 = A / 2;

                Console.WriteLine("Matrix D = A * B");
                D.ShowMatrix();
                Console.WriteLine();
                Console.WriteLine("Matrix D1 = A * 3");
                D1.ShowMatrix();
                Console.WriteLine();
                Console.WriteLine("Matrix D2 = A / 2");
                D2.ShowMatrix();
                Console.WriteLine();

               Console.WriteLine($"Ранк матрицы А ={A.Rank()}");

                Console.WriteLine("Введите размер квадратной матрицы sqrn:");
                int sqrn = Convert.ToInt32(Console.ReadLine());

                SquareMatrix sqrA = new SquareMatrix(sqrn);
                sqrA.FillMatrix();

                Console.WriteLine("Матрица A:");
                sqrA.ShowMatrix();
                Console.WriteLine();

                Console.WriteLine("Матрица А транспонированная");
                Matrix sqrAt = sqrA.tranponation();
                sqrAt.ShowMatrix();
                Console.WriteLine();

                Console.WriteLine("Матрица А в ступенчатом ввиде");
                SquareMatrix sqrAstup = new SquareMatrix(sqrn);
                sqrAstup.FillMatrix();
                sqrAstup.ReduceToRowEchelonForm();
                sqrAstup.ShowMatrix();
                Console.WriteLine();

                SquareMatrix sqrB = new SquareMatrix(sqrn);
                sqrB.FillMatrix();

                Console.WriteLine("Матрица B:");
                sqrB.ShowMatrix();

                SquareMatrix sqrC = sqrA + sqrB;
                Console.WriteLine("Матрица C = A + B:");
                sqrC.ShowMatrix();
                Console.WriteLine();

                SquareMatrix sqrD = sqrA * sqrB;
                Console.WriteLine("Матрица D = A * B:");
                sqrD.ShowMatrix();
                Console.WriteLine();

                Console.WriteLine("Определитель матрицы A: " + sqrA.Determinant());
                Console.WriteLine();

                Console.WriteLine("Обратная к матрице A");
                SquareMatrix A_inverse = sqrA;
                A_inverse.Inverse().ShowMatrix();
                Console.WriteLine();
            }
        }
    }
}
