namespace matrix
{
    class Program3
    {
        public static void Main()
        {
            var multiplier = new matrix(3);

            var result = multiplier.Multiply(
                new int[,]
                {
                { 1, 2, 3 },
                { 4, 5, 6},
                { 7, 8, 9}
                },
                    new int[,]
                {
                { 1, 8, 4 },
                { 6, 3, 1 },
                { 9, 2, 7 }
                },
                (a, b) => a * b,
                (a, b) => a + b);

            for (int i = 0; i < result.GetLength(0); i++)
            {
                for (int j = 0; j < result.GetLength(1); j++)
                {
                    Console.Write($"{result[i, j].ToString()} ");
                }

                Console.Write("\n");
            }
        }
    }
}
