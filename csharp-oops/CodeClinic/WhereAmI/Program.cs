using System;
using System.Device.Location;

namespace WhereAmI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting GeoCoordinate Watcher...");

            // 1. Add a Reference to System.Device.dll - Done ( to access geocoordinate watcher class)

            // 2. Use the GeoCoordinate Watcher
            var watcher = new GeoCoordinateWatcher();
            watcher.StatusChanged += (s, e) =>
            {
                Console.WriteLine($"GorCoordinateWatcher:StatusChanged: {e.Status}");
            };


            watcher.PositionChanged += (s, e) =>
            {
                watcher.Stop();
                Console.WriteLine($"GorCoordinateWatcher:PositionChanged: {e.Position.Location}");

                // 3. Use the Map Image REST API
                MapImage.Show(e.Position.Location);
            };

            watcher.MovementThreshold = 100;
            
            watcher.Start();

            

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
