namespace producerconsumer
{
    class Program4
    {
        private producerconsumer<int> queue = new producerconsumer<int>();
        public static void Main()
        {
            var program = new Program4();
            program.TestProducerConsumer();

            Console.ReadKey();
        }

        private void TestProducerConsumer()
        {
            var threads = new[] { new Thread(Consumer), new Thread(Consumer) };
            foreach (var t in threads)
                t.Start();

            for (int i = 0; i < 10; i++)
            {
                queue.Enqueue(i);
            }

            queue.Stop();

            foreach (var t in threads)
                t.Join();
        }

        private void Consumer()
        {
            while (true)
            {
                try
                {
                    var i = queue.Dequeue();

                    Console.WriteLine("Processing: {0}", i.ToString());
                    Thread.Sleep(2000);
                    Console.WriteLine("Processed: {0}", i.ToString());
                }
                catch
                {
                    break;
                }
            }
        }
    }
}
