﻿using System.Collections.Generic;

namespace GenerateJsonSchema
{
    class Car
    {
        public string Model { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }
        public List<string> Registrations { get; set; }
    }
}
