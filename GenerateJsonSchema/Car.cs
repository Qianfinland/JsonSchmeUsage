using System.Collections.Generic;

namespace GenerateJsonSchema
{
    class Car
    {
        public string Model { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }
        public int[] Price { get; set; }
        public string[] TravelCities { get; set; }
        //public List<string> Registrations { get; set; }
    }
}
