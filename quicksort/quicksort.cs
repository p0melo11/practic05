using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace quicksort
{
    public class quicksort
    {
        public T[] Sort<T>(T[] array, Func<T, T, int> compareFunction)
        {
            if (array.Length <= 1)
            {
                return array;
            }

            var anchor = array[0];
            var smaller = new List<T>();
            var bigger = new List<T>();

            for (int i = 1; i < array.Length; i++)
            {
                var item = array[i];

                if (compareFunction(anchor, item) < 0)
                {
                    smaller.Add(item);
                }
                else
                {
                    bigger.Add(item);
                }
            }

            T[] smallerSorted = new T[smaller.Count + 1];
            T[] biggerSorted = new T[bigger.Count];

            var threadShouldDone = 2;

            var smallerSortingThread = new Thread(() =>
            {
                smallerSorted = Sort<T>(smaller.ToArray(), compareFunction).Concat(new T[] { anchor }).ToArray();
                threadShouldDone -= 1;
            });
            var biggerSortingThread = new Thread(() =>
            {
                biggerSorted = Sort<T>(bigger.ToArray(), compareFunction);
                threadShouldDone -= 1;
            });

            smallerSortingThread.Start();
            biggerSortingThread.Start();

            while (threadShouldDone != 0)
            { }

            return smallerSorted.Concat(biggerSorted).ToArray();
        }
    }
}
