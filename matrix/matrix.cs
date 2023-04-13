using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace matrix
{
    public class matrix
    {
        private Semaphore _threadsSemaphore;

        public matrix(int maxThreads)
        {
            _threadsSemaphore = new Semaphore(maxThreads, maxThreads);
        }

        public T[,] Multiply<T>(T[,] matrix1, T[,] matrix2, Func<T, T, T> multiplyItems, Func<T, T, T> sumItems)
        {
            if (!IsMatrixValid(matrix1) || !IsMatrixValid(matrix2))
            {
                throw new ArgumentException("Matrix is not valid");
            }

            if (!CanBeMultiplied(matrix1, matrix2))
            {
                throw new ArgumentException("matrix can't be multiplied");
            }

            var rowsCount = GetMatrixRowsCount(matrix1);
            var columnCount = GetMatrixColumnsCount(matrix2);
            var result = new T[rowsCount, columnCount];
            var doneRows = 0;

            for (int rowIndex = 0; rowIndex < rowsCount; rowIndex++)
            {

                Console.WriteLine(rowIndex);
                Thread.Sleep(100);

                var thread = new Thread(() =>
                {
                    _threadsSemaphore.WaitOne();

                    for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
                    {
                        var accumulator = default(T);

                        for (int cellIndex = 0; cellIndex < columnCount; cellIndex++)
                        {
                            var value = multiplyItems(matrix2[cellIndex, columnIndex], matrix1[rowIndex, cellIndex]);

                            if (accumulator == null)
                            {
                                accumulator = value;
                            }
                            else
                            {
                                accumulator = sumItems(accumulator, value);
                            }
                        }

                        if (accumulator != null)
                        {
                            result[rowIndex, columnIndex] = accumulator;
                        }
                    }

                    doneRows += 1;
                    _threadsSemaphore.Release();
                });

                thread.Start();
                thread.Join();
            }

            while (doneRows != rowsCount)
            { }

            return result;
        }

        public static int GetMatrixRowsCount<T>(T[,] matrix)
        {
            return matrix.GetLength(0);
        }
        public static int GetMatrixColumnsCount<T>(T[,] matrix)
        {
            return matrix.GetLength(1);
        }

        private bool IsMatrixValid<T>(T[,] matrix)
        {
            if (GetMatrixRowsCount(matrix) == 0 || GetMatrixColumnsCount(matrix) == 0)
            {
                return false;
            }

            return true;
        }

        private bool CanBeMultiplied<T>(T[,] matrix1, T[,] matrix2)
        {
            return GetMatrixColumnsCount(matrix1) == GetMatrixRowsCount(matrix2);
        }
    }
}
