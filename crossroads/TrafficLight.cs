using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crossroads
{
    public class TrafficLight
    {
        public string Name;
        private Semaphore _carsSemaphore = new Semaphore(3, 3);
        private BlockingCollection<Car> _carsQueue = new BlockingCollection<Car>();
        private Mutex _finishMutex;

        public TrafficLight(string name)
        {
            Name = name;
        }
        public void StartProcess(Mutex mutex)
        {
            _finishMutex = mutex;

            var passingCarsThread = new Thread(PassCars);
            passingCarsThread.Start();

            var arrivingCarsThread = new Thread(ArrivingCars);
            arrivingCarsThread.Start();
        }

        private void PassCars()
        {
            _finishMutex.WaitOne();

            foreach (var car in _carsQueue.GetConsumingEnumerable())
            {
                _carsSemaphore.WaitOne();
                var carDriving = new Thread(car.Drive(() =>
                {
                    _carsSemaphore.Release();
                }));
                carDriving.Start();
            }

            _finishMutex.ReleaseMutex();
        }

        private void ArrivingCars()
        {
            for (int i = 0; i < Random.Shared.Next(2, 10); i++)
            {
                _carsQueue.Add(new Car($"TF-{Name}-{i.ToString()}"));
                Thread.Sleep(100 * Random.Shared.Next(1, 5));
            }
            _carsQueue.CompleteAdding();
        }
    }
}
