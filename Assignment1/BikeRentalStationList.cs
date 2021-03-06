﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment1
{
    class Station
    {
        public int id { get; set; }
        public string name { get; set; }
        public float x { get; set; }
        public float y { get; set; }
        public int bikesAvailable { get; set; }
        public int spacesAvailable { get; set; }
        public bool allowDropoff { get; set; }
        public bool isFloatingBike { get; set; }
        public bool isCarStation { get; set; }
        public string state { get; set; }
        public string[] networks { get; set; }
        public bool realTimeData { get; set; }
    }

    

    class BikeRentalStationList
    {
        public Station[] stations { get; set; }
    }
}
