using System;
namespace GenerateJsonSchema
{
    using GenerateJsonSchema.NiceCars;
    class Car
    {
        public string Model { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }
        public int[] Price { get; set; }
        public string[] Cities { get; set; }
        public char CharTest { get; set; } 
        public char[] CharArrayTest { get; set; }
        public float FloatTest { get; set; }
        public float[] FloatArrayTest { get; set; }
        public NiceCar NamespaceTest { get; set; }
        //public List<string> Registrations { get; set; }
    }


}

namespace GenerateJsonSchema.NiceCars
{
    class NiceCar
    {
        public string Description { get; set; }
    }
}

