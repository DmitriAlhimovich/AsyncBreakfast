using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncBreakfast
{
    class Program
    {
        public const int Delay = 3000;
        static async Task Main(string[] args)
        {
            if (args.Any())
                CookSync();
            else
                await CookAsync();
        }

        private static async Task CookAsync()
        {
            Console.WriteLine("--- Start Asynchronous cooking ---");

            var start = DateTime.Now;

            Coffee cup = PourCoffee();
            Console.WriteLine("coffee is ready");

            var eggTask = FryEggsAsync(2)
                .ContinueWith(_ => Console.WriteLine($"eggs are ready({(DateTime.Now - start).TotalMilliseconds})"));

            var baconTask = FryBaconAsync(3)
                .ContinueWith(_ => Console.WriteLine($"bacon is ready({(DateTime.Now - start).TotalMilliseconds})"));

            var toastTask = ToastBreadWithButterAndJamAsync()
                .ContinueWith(_ => Console.WriteLine($"toast is ready({(DateTime.Now - start).TotalMilliseconds})"));

            var tasks = new List<Task>() { eggTask, baconTask, toastTask };

            //await Task.WhenAll(tasks);

            while (tasks.Any())
            {
                var task = await Task.WhenAny(tasks);

                tasks.Remove(task);
            }

            Juice oj = PourOJ();
            Console.WriteLine("oj is ready");
            Console.WriteLine($"Breakfast is ready! ({(DateTime.Now - start).TotalMilliseconds})");
        }

        private static async Task ToastBreadWithButterAndJamAsync()
        {
            Toast toast = await ToastBreadAsync(2);
            ApplyButter(toast);
            ApplyJam(toast);
        }

        private static Juice PourOJ()
        {
            Console.WriteLine("Pouring orange juice");
            return new Juice();
        }

        private static void ApplyJam(Toast toast) =>
            Console.WriteLine("Putting jam on the toast");

        private static void ApplyButter(Toast toast) =>
            Console.WriteLine("Putting butter on the toast");

        private static async Task<Toast> ToastBreadAsync(int slices)
        {
            for (int slice = 0; slice < slices; slice++)
            {
                Console.WriteLine("Putting a slice of bread in the toaster");
            }
            Console.WriteLine("Start toasting...");
            await Task.Delay(Delay);
            Console.WriteLine("Remove toast from toaster");

            return new Toast();
        }

        private static async Task<Bacon> FryBaconAsync(int slices)
        {
            Console.WriteLine($"putting {slices} slices of bacon in the pan");
            Console.WriteLine("cooking first side of bacon...");
            await Task.Delay(Delay);
            for (int slice = 0; slice < slices; slice++)
            {
                Console.WriteLine("flipping a slice of bacon");
            }
            Console.WriteLine("cooking the second side of bacon...");
            await Task.Delay(Delay);
            Console.WriteLine("Put bacon on plate");

            return new Bacon();
        }

        private static async Task<Egg> FryEggsAsync(int howMany)
        {
            Console.WriteLine("Warming the egg pan...");
            await Task.Delay(Delay);
            Console.WriteLine($"cracking {howMany} eggs");
            Console.WriteLine("cooking the eggs ...");
            await Task.Delay(Delay);
            Console.WriteLine("Put eggs on plate");

            return new Egg();
        }

        private static Coffee PourCoffee()
        {
            Console.WriteLine("Pouring coffee");
            return new Coffee();
        }

        private static void CookSync()
        {
            Console.WriteLine("--- Start Synchronous cooking ---");

            var start = DateTime.Now;

            Coffee cup = PourCoffee();
            Console.WriteLine("coffee is ready");

            Egg eggs = FryEggs(2);
            Console.WriteLine("eggs are ready");

            Bacon bacon = FryBacon(3);
            Console.WriteLine("bacon is ready");

            Toast toast = ToastBread(2);
            ApplyButter(toast);
            ApplyJam(toast);
            Console.WriteLine("toast is ready");

            Juice oj = PourOJ();
            Console.WriteLine("oj is ready");
            Console.WriteLine($"Breakfast is ready! ({(DateTime.Now - start).TotalMilliseconds})");
        }

        private static Toast ToastBread(int slices)
        {
            for (int slice = 0; slice < slices; slice++)
            {
                Console.WriteLine("Putting a slice of bread in the toaster");
            }
            Console.WriteLine("Start toasting...");
            Task.Delay(Delay).Wait();
            Console.WriteLine("Remove toast from toaster");

            return new Toast();
        }

        private static Bacon FryBacon(int slices)
        {
            Console.WriteLine($"putting {slices} slices of bacon in the pan");
            Console.WriteLine("cooking first side of bacon...");
            Task.Delay(Delay).Wait();
            for (int slice = 0; slice < slices; slice++)
            {
                Console.WriteLine("flipping a slice of bacon");
            }
            Console.WriteLine("cooking the second side of bacon...");
            Task.Delay(Delay).Wait();
            Console.WriteLine("Put bacon on plate");

            return new Bacon();
        }

        private static Egg FryEggs(int howMany)
        {
            Console.WriteLine("Warming the egg pan...");
            Task.Delay(Delay).Wait();
            Console.WriteLine($"cracking {howMany} eggs");
            Console.WriteLine("cooking the eggs ...");
            Task.Delay(Delay).Wait();
            Console.WriteLine("Put eggs on plate");

            return new Egg();
        }
    }
}