using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


//http://api.digitransit.fi/routing/v1/routers/hsl/bike_rental

using System.Net.Http;

namespace Assignment1
{
    [Serializable]
    class NotFoundException : Exception
    {
        public string StationName { get; }

        public NotFoundException(string message)
        : base(message) { }

        public NotFoundException(string message, string stationName) : this(message)
        {
            StationName = stationName;
        }
    }

    class RealTimeCityBikeDataFetcher : ICityBikeDataFetcher
    {
        readonly string apiUrl = "http://api.digitransit.fi/routing/v1/routers/hsl/bike_rental";
        string numbers = "0123456789";
        public async Task<int> GetBikeCountInStation(string stationName)
        {
            for(int i = 0; i < stationName.Length; i++)
            {

                //if (Char.IsDigit(stationName[i]))
                //{
                //    throw new ArgumentException();
                //}

                if (numbers.Contains(stationName[i]))
                {
                    throw new ArgumentException(String.Format("\"{0}\"\nArgument cannot contain numbers\n", stationName));
                }
            }

            HttpClient HttpClient = new HttpClient();
            string response = await HttpClient.GetStringAsync(apiUrl);

            // Create BikeRentalStationList object from the JSON response
            BikeRentalStationList list = JsonConvert.DeserializeObject<BikeRentalStationList>(response);

            int result = 0;
            bool found = false;

            // Loop through all the stations in the "list" object
            foreach (Station station in list.stations)
            {
                if (station.name == stationName)
                {
                    result = station.bikesAvailable;
                    found = true;
                    break;
                }
            }

            // If the corresponding station wasnt found throw an NotFoundException
            if(!found)
            {
                throw new NotFoundException("Station not found: ", stationName);
            }
            else
            {
                return result;
            }
        }
    }
}
