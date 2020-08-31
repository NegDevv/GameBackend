using System;
using System.Threading.Tasks;

//http://api.digitransit.fi/routing/v1/routers/hsl/bike_rental

namespace Assignment1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string text = System.IO.File.ReadAllText(@"C:\Dev\VisualStudioProjects\GameBackend\Assignment1\bikedata.txt");
            //Console.WriteLine(text);

            ICityBikeDataFetcher fetcher = new RealTimeCityBikeDataFetcher();

            if(args[1] == "realtime")
            {
                fetcher = new RealTimeCityBikeDataFetcher();
            }
            else if(args[1] == "offline")
            {
                fetcher = new OfflineCityBikeDataFetcher();
            }

            Console.WriteLine("Mode: " + args[1]);
            
            try
            {
                int bikeCount = await fetcher.GetBikeCountInStation(args[0]);
                Console.WriteLine(args[0]);
                Console.WriteLine("Bike count: " + bikeCount);
            }
            catch(ArgumentException e)
            {
                Console.WriteLine("Invalid argument: " + e.Message);
            }
            catch(NotFoundException notFoundExc)
            {
                Console.WriteLine(notFoundExc.Message + notFoundExc.StationName);
            }
            
            

        }
    }
}
