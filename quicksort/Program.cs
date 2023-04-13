namespace quicksort;

class Program
{
    public static void Main()
    {
        var array = new int[] { 2, 6, 8, 4, 3, 1, 5, 7 };
        var sorter = new quicksort();
        var sortedArray = sorter.Sort(array, (a, b) => b - a);

        foreach (var item in sortedArray)
        {
            Console.WriteLine(item);
        }
    }
}