using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matrix
{
    internal class Program
    {


        static void Main(string[] args)
        {
            float[,] A = { { 2, 1.99999f, 3, 1 }, { 1, 1, 1, 1 }, { 1, -1, -2, 1 }, { 1, 2, 1, 1 } };
            float[] b = { 7.99997f, 2, -1, 5 };

            Console.WriteLine("Изначальная матрица:\n ");
            PrintMatrix(A, b);

            GaussWithMainElement(A, b);
            GaussWithoutMainElement(A, b);

            Console.ReadLine();
        }


        ///<summary>Метод Гаусса без выбора главного элемента</summary>
        static void GaussWithoutMainElement(float[,] A, float[] b) {
            Console.WriteLine("\nМетод Гаусса без выбора главного элемента:\n ");
            (A, b) = DirectCourseWithoutMainElement(A, b);
            PrintMatrix(A, b);
            PrintRoots(ReverseCourse(A, b));
        }

        ///<summary>Метод Гаусса с выбором главного элемента по всей матрице</summary>
        static void GaussWithMainElement(float[,] A, float[] b)
        {
            Console.WriteLine("\nМетод Гаусса с выбором главного элемента по всей матрице: \n");
            int[] pos = new int[A.GetLength(0)];
            for (int i = 0; i < A.GetLength(0); i++) pos[i] = i;
            (A, b) = DirectCourseWithMainElement(A, b, pos);
            PrintMatrix(A, b);
            PrintRoots(ReverseCourse(A, b, pos));
        }

        ///<summary>Метод простых итераций</summary>
        static void SimpleIterations(float[,] A, float[] b)
        {
            (A, b) = CheckDiagonalElements(A, b);
            (A, b) = MakeMatrixForSimpleIterations(A, b);
            float[] x = new float[A.GetLength(0)], _x = new float[A.GetLength(0)];
            while (true)
            {
                for (int i = 0; i < A.GetLength(0); i++)
                {
                    x[i] = b[i];
                    for (int j = 0; j < A.GetLength(0); j++) 
                        x[i] += j >= i ? A[i, j] * b[i] : A[i, j] * _x[j];
                    _x = x;
                }
                
            }
            if (!CheckNorma(A, x)) throw new Exception("Норма >= 1!"); // ПРОВЕРИТЬ!
        }

        static bool CheckNorma(float[,] A, float[] lenVectorX)
        {
            float normaA = 0;
            for (int i = 0; i < A.GetLength(0); i++) normaA += lenVectorX[i];
            return normaA < 1;
        }

        ///<summary>Сформировать новые матрицы для простых итериций</summary>
        static (float[,], float[]) MakeMatrixForSimpleIterations(float[,] A, float[] b)
        {
            
            for (int i = 0; i < A.GetLength(0); i++)
            {
                float k = A[i, i];
                b[i] = b[i] / k ;
                for (int j = 0; j < A.GetLength(0); j++) A[i, j] = i == j ? -A[i, j] / k : 0 ;
            }
            return (A, b);
        }

        ///<summary>Проверяет диагональные элементы на ненулевое значение, при необходимости меняет строки и/или столбцы</summary>
        static (float[,], float[]) CheckDiagonalElements(float[,] A, float[] b)
        {
            for (int i = 0; i < A.GetLength(0); i++) NonzeroElement(i, A, b);
            return (A, b);
        }

        ///<summary>Проверяет выполнение условия иттераций</summary>
        /*static bool IterationConditionIsSatisfied(float[,] A)
        {
            for (int i = 0; i < A.GetLength(0); i++)
            {
               float sum = 0;
               for(int j = 0; j < A.GetLength(1); j++)
               {
                    if (i != j)
                    {
                        sum += Math.Abs(A[i, j]);
                    }
               }
               if (sum > Math.Abs(A[i, i]))
               {
                    return false;
               }
            }
            return true;
        } */

        ///<summary>Меняет местами строки с индексами index1 и index 2 в матрице и возвращает изменненую матрицу.</summary>
        static float[,] SwapRows(int index1, int index2, float[,] matrix)
        {
            if (index1 > matrix.GetLength(0) || index1 > matrix.GetLength(1)
                || index2 > matrix.GetLength(0) || index2 > matrix.GetLength(1)) throw new Exception("Индекс выходит за пределы матрицы!");
            if (index1 < 0 || index2 < 0) throw new Exception("Индекс отрицательный.");

            float k; //Мусорная переменная, нужная для того, чтобы поменять числа местами

            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                k = matrix[index1, i];
                matrix[index1, i] = matrix[index2, i];
                matrix[index2, i] = k;
            }
            return matrix;
        }


        ///<summary>Меняет местами столбцы с индексами index1 и index 2 в матрице и возвращает изменненую матрицу.</summary>
        static float[,] SwapColumns(int index1, int index2, float[,] matrix)
        {
            if (index1 > matrix.GetLength(0) || index1 > matrix.GetLength(1)
                || index2 > matrix.GetLength(0) || index2 > matrix.GetLength(1)) throw new Exception("Индекс выходит за пределы матрицы.");
            if (index1 < 0 || index2 < 0) throw new Exception("Индекс отрицательный.");

            float k; //Мусорная переменная, нужная для того, чтобы поменять числа местами
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                k = matrix[i, index1];
                matrix[i, index1] = matrix[i, index2];
                matrix[i, index2] = k;
            }
            return matrix;
        }

        ///<summary>Меняет местами элементы с индексами index1 и index 2 в массиве и возвращает изменненый массив.</summary>
        static float[] SwapElements(int index1, int index2, float[] array)
        {
            if (index1 > array.Length || index2 > array.Length) throw new Exception("Индекс выходит за пределы матрицы!");
            if (index1 < 0 || index2 < 0) throw new Exception("Индекс отрицательный.");
            float k = array[index1];
            array[index1] = array[index2];
            array[index2] = k;

            return array;
        }

        ///<summary>Меняет местами строки с индексами index1 и index 2 в матрице и возвращает изменненую матрицу.</summary>
        static int[] SwapElementsInt(int index1, int index2, int[] array)
        {
            if (index1 > array.Length || index2 > array.Length) throw new Exception("Индекс выходит за пределы матрицы!");
            if (index1 < 0 || index2 < 0) throw new Exception("Индекс отрицательный.");
            int k = array[index1];
            array[index1] = array[index2];
            array[index2] = k;

            return array;
        }

        ///<summary>Меняет строки 2-x матриц местами так, что бы [index, index] элемент у первой был ненулевой. Если он изначально был ненулевой, ничего не меняет.</summary>
        static (float[,], float[]) NonzeroElement(int index, float[,] A, float[] b)
        {
            if (index > A.GetLength(0) || index > A.GetLength(1)) throw new Exception("Индекс выходит за пределы матрицы.");
            if (index < 0) throw new Exception("Индекс отрицательный.");
            int k = index + 1; //Количество проведенных замен
            while (A[index, index] == 0)
            {
                A = SwapRows(index, k, A);
                b = SwapElements(index, k, b);
                k++;
                if (k > A.GetLength(0)) throw new Exception("В " + index.ToString() + " столбце не существет ненулевого элемента.");
            }
            return (A, b);
        }

        //Прямой ход - Преобразует матрицы A и b из уравнения Ax = b, так чтобы элементы в матрице A под главной диаганалью стали равны 0.
        ///<summary>Прямой ход без выбора главного элемента</summary>
        static (float[,], float[]) DirectCourseWithoutMainElement(float[,] A, float[] b)
        {
            int N = A.GetLength(0); //Количество строк
            for (int i = 0; i < N - 1; i++) //Цикл перебирает строки на которых мы ищем главный элемент(A[i, i])
            {
                (A, b) = NonzeroElement(i, A, b);
                for (int j = i + 1; j < N; j++)
                {
                    float k = A[j, i] / A[i, i];
                    b[j] -= k * b[i];
                    for (int q = i; q < N; q++) A[j, q] -= k * A[i, q];
                }
            }
            return (A, b);
        }

        //Прямой ход - Преобразует матрицы A и b из уравнения Ax = b, так чтобы элементы в матрице A под главной диаганалью стали равны 0.
        ///<summary>Прямой ход с выбором главного элемента по всей матрице</summary>
        static (float[,], float[]) DirectCourseWithMainElement(float[,] A, float[] b, int[] pos)
        {
            int N = A.GetLength(0); //Количество строк
            for (int i = 0; i < N - 1; i++) //Цикл перебирает строки на которых мы ищем главный элемент(A[i, i])
            {
                (A, b, pos) = SetMainElemenInRightPosition(A, b, pos, i);
                (A, b) = NonzeroElement(i, A, b);
                for (int j = i + 1; j < N; j++)
                {
                    float k = A[j, i] / A[i, i];
                    b[j] -= k * b[i];
                    for (int q = i; q < N; q++) A[j, q] -= k * A[i, q];
                }
            }
            return (A, b);
        }

        ///<summary>Находит максимальный элемент и устанавливает его на [step; step] место</summary>
        static (float[,], float[], int[]) SetMainElemenInRightPosition(float[,] A, float[] b, int[] pos, int step)
        {
            int N = A.GetLength(0); //Количество строк
            float mx = A[step, step];
            int mxI = step, mxJ = step;
            for (int i = step; i < N; i++)
            {
                for (int j = step; j < N; j++)
                {
                    if (A[i, j] > mx) (mx, mxI, mxJ) = (A[i, j], i, j);
                }
            }
            A = SwapRows(step, mxI, A);
            b = SwapElements(step, mxI, b);
            A = SwapColumns(step, mxJ, A);
            pos = SwapElementsInt(step, mxJ, pos);
            return (A, b, pos);
        }
        
        ///<summary>Обратный ход</summary>
        static float[] ReverseCourse(float[,] A, float[] b)
        {
            int N = A.GetLength(0); //Количество строк
            float[] x = new float[N];
            for (int i = N - 1; i > -1; i--)
            {
                float sum = 0;
                for (int j = N - 1; j > i; j--) sum += A[i, j] * x[j];
                x[i] = (b[i] - sum) / A[i, i];
            }
            return x;
        }

        ///<summary>Обратный ход</summary>
        static float[] ReverseCourse(float[,] A, float[] b, int[] pos)
        {
            int N = A.GetLength(0); //Количество строк
            float[] x = ReverseCourse(A, b), _x = new float[N];
            for (int i = 0; i < N; i++) _x[pos[i]] = x[i];
            return _x;
        }

        ///<summary>Выводит матрицу на экран</summary>
        static void PrintMatrix(float[,] floats)
        {
            for (int i = 0; i < floats.GetLength(0); i++)
            {
                for (int j = 0; j < floats.GetLength(0); j++) Console.Write("{0}\t", floats[i, j]);
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        ///<summary>Выводит одномерный массив рядом на экран</summary>
        static void PrintMatrix(float[] floats)
        {
            for (int i = 0; i < floats.GetLength(0); i++) Console.Write("{0}\t", floats[i]);
            Console.WriteLine();
        }

        ///<summary>Выводит матрицу и одномерный массив рядом на экран</summary>
        static void PrintMatrix(float[,] floats, float[] b)
        {
            for (int i = 0; i < floats.GetLength(0); i++)
            {
                for (int j = 0; j < floats.GetLength(0); j++) Console.Write("{0, 15}", floats[i, j]);
                Console.Write("  |  {0}\n", b[i]);
            }
            Console.WriteLine();
        }
        ///<summary>Выводит решение СЛАУ</summary>
        static void PrintRoots(float[] floats)
        {
            for (int i = 0; i < floats.Length; i++) Console.WriteLine("x{0} = {1}", i + 1, floats[i]);
            Console.WriteLine();
        }
    }
}