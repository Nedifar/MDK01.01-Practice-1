using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection;

namespace AnalysisExecutionSpeed
{
    delegate void TimingDelegate(int[] inArray);
    delegate void TimingHashDelegate(Hashtable hash);
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] array = new int[10000];
            Hashtable hash = new Hashtable();
            array = RandomFill(array);
            hash = RandomFill(hash);
            MethodTiming(InsertionSort, array);
            MethodTiming(ChoiceSort, array);
            MethodTiming(BubbleSort, array);
            MethodTiming(LinearSearch, array);
            MethodTiming(BinarySearch, array);
            MethodHashTableTiming(LinearSearch, hash);
        }

        static void MethodTiming(TimingDelegate timing, int[] array)
        {
            Timing objT = new Timing();
            Stopwatch stpWatch = new Stopwatch();
            objT.StartTime();
            stpWatch.Start();
            timing.Invoke(array);
            stpWatch.Stop();
            objT.StopTime();
            Console.WriteLine("StopWatch: " +
stpWatch.Elapsed.ToString());
            Console.WriteLine("Timing: " +
           objT.Result().ToString());
        }

        static void MethodHashTableTiming(TimingHashDelegate timing, Hashtable hashtable)
        {
            Timing objT = new Timing();
            Stopwatch stpWatch = new Stopwatch();
            objT.StartTime();
            stpWatch.Start();
            timing.Invoke(hashtable);
            stpWatch.Stop();
            objT.StopTime();
            Console.WriteLine("StopWatch: " +
stpWatch.Elapsed.ToString());
            Console.WriteLine("Timing: " +
           objT.Result().ToString());
        }

        static int[] RandomFill(int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = new Random().Next(1000);
            }
            return array;
        }

        static Hashtable RandomFill(Hashtable hashtable)
        {
            for (int i =0; i<10000; i++)
            {
                hashtable.Add(i, new Random().Next(1000));
            }
            return hashtable;
        }

        static void Swap(int[] array, int i, int j)
        {
            int temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
        static void InsertionSort(int[] inArray)
        {
            int x;
            int j;
            for (int i = 1; i < inArray.Length; i++)
            {
                x = inArray[i];
                j = i;
                while (j > 0 && inArray[j - 1] > x)
                {
                    Swap(inArray, j, j - 1);
                    j -= 1;
                }
                inArray[j] = x;
            }
        }

        static void ChoiceSort(int[] intArray)
        {
            int indx;
            for (int i = 0; i < intArray.Length; i++)
            {
                indx = i;
                for (int j = i; j < intArray.Length; j++)
                {
                    if (intArray[j] < intArray[indx])
                    {
                        indx = j;
                    }
                }
                if (intArray[indx] == intArray[i])
                    continue;
                int temp = intArray[i];
                intArray[i] = intArray[indx];
                intArray[indx] = temp;
            }
        }

        public static void BubbleSort(int[] anArray)
        {
            for (int i = 0; i < anArray.Length; i++)
            {
                for (int j = 0; j < anArray.Length - 1 - i; j++)
                {
                    if (anArray[j] > anArray[j + 1])
                    {
                        SwapBubble(ref anArray[j], ref anArray[j + 1]);
                    }
                }
            }
        }

        public static void SwapBubble(ref int aFirstArg, ref int aSecondArg)
        {
            int tmpParam = aFirstArg;
            aFirstArg = aSecondArg;
            aSecondArg = tmpParam;
        }

        public static void SwapSorting(ref int a, ref int b)
        {
            a = a + b;
            b = a - b;
            a = a - b;
        }

        static void LinearSearch(int[] anArray)
        {
            int b = 1;
            int k = -1;
            int counter = 0;
            for (int i = 0; i < anArray.Length; i++)
            {
                counter++;
                if (anArray[i] == b) { k = i; break; };
            }
        }

        static void LinearSearch(Hashtable anArray)
        {
            int b = 1;
            int k = -1;
            int counter = 0;
            foreach(var i in anArray.Keys)
            {
                counter++;
                if(Convert.ToInt32(i) == b) { k = 1; break; }
            }
        }

        static void BinarySearch(int[] anArray)
        {
            int b = 1;
            int k;   // с
            int L = 0;        // левая граница
            int R = anArray.Length - 1;    // правая граница
            k = (R + L) / 2;
            int counter = 0;
            while (L < R - 1)
            {
                counter++;
                k = (R + L) / 2;
                if (anArray[k] == b) { }
                counter++;
                if (anArray[k] < b)
                    L = k;
                else
                    R = k;
            }
            if (anArray[k] != b)
            {
                if (anArray[L] == b)
                    k = L;
                else
                {
                    if (anArray[R] == b)
                        k = R;
                    else
                        k = -1;
                };
            }
        }
    }


    internal class Timing
    {
        TimeSpan duration; //хранение результата измерения
        TimeSpan[] threads; // значения времени для всех потоков процесса
        public Timing()
        {
            duration = new TimeSpan(0);
            threads = new TimeSpan[Process.GetCurrentProcess().
            Threads.Count];
        }
        public void StartTime() //инициализация массива threads после вызова сборщика мусора
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            for (int i = 0; i < threads.Length; i++)
                threads[i] = Process.GetCurrentProcess().Threads[i].
                UserProcessorTime;
        }
        public void StopTime() //повторный запрос текущего времени и выбирается тот, у которого результат отличается от
        {				//предыдущего
            TimeSpan tmp;
            for (int i = 0; i < threads.Length; i++)
            {
                tmp = Process.GetCurrentProcess().Threads[i].
                UserProcessorTime.Subtract(threads[i]);
                if (tmp > TimeSpan.Zero)
                    duration = tmp;
            }
        }
        public TimeSpan Result()
        {
            return duration;
        }
    }

}
