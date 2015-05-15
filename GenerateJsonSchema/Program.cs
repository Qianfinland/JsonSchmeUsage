using System;
using Newtonsoft.Json.Schema;
using System.IO;

namespace GenerateJsonSchema
{
    class Program
    {
        static void Main(string[] args)
        {
            // Step 1 generate json-schema from poco
            var jsonSchemaGenerator = new JsonSchemaGenerator();
            var myType = typeof(Car);
            var schema = jsonSchemaGenerator.Generate(myType);
            //Console.WriteLine(schema); // get the json schema

            //schema.Title = myType.Name;
            //Console.WriteLine(jsonSchemaGenerator); //Newtonsoft.Json.Schema.JsonSchemaGenerator
            //Console.WriteLine(myType);//GenerateJsonSchema.Car
            //Console.WriteLine(schema.Title); // Car
            //Console.WriteLine(schema.Type); //Object
            //Console.ReadLine();

            // save the generated json-schema to the text file
            TextWriter tw = File.CreateText(@"schema.txt");
            tw.WriteLine(schema);
            Console.WriteLine("Text file on schema created!");
            tw.Close();
            Console.WriteLine(Console.Read());
        }

        static void FromPocoToSchema()
        {
            // Do something
        }

        static void FromSchemaToPoco()
        {
            // Do something
        }
    }

    /*class Poco
    {
        private const string Nullable = "?";
        static void Main(string[] args)
        {
            //step 2: generate poco from json-schema
            string schemaText;
            using (var r = new StreamReader("schema.txt"))
            {
                schemaText = r.ReadToEnd();
            }
            var jsonSchema = JsonSchema.Parse(schemaText);

            
            //Console.WriteLine(jsonSchema);
            //Console.ReadLine();

            if (jsonSchema != null)
            {
                var sb = ConvertJsonSchemaToPocos(jsonSchema);
                var code = sb.ToString();
                TextWriter poco = File.CreateText(@"poconew.txt");
                poco.WriteLine(code);
                Console.WriteLine("Text file on poco created!");
                poco.Close();
                Console.WriteLine(Console.Read());
            }
        }

        private static StringBuilder ConvertJsonSchemaToPocos(JsonSchema schema)
        {
            
            if (schema.Type == null)
                throw new Exception("Schema does not specify a type.");

            var sb = new StringBuilder();
            switch (schema.Type)
            {
                case JsonSchemaType.Object:
                    sb.Append(ConvertJsonSchemaObjectToPoco(schema));
                    break;
                
                // purpose not clear ????????-
                //case JsonSchemaType.Array:
                //    foreach (var item in schema.Items.Where(x => x.Type.HasValue && x.Type == JsonSchemaType.Object))
                //    {
                //        sb.Append(ConvertJsonSchemaObjectToPoco(item));
                //        Console.WriteLine(ConvertJsonSchemaObjectToPoco(item));
                //        Console.ReadLine();
                //    }
                //    break;
            }
            
            return sb;
        }

        private static StringBuilder ConvertJsonSchemaObjectToPoco(JsonSchema schema)
        {
            string className;
            //Console.WriteLine(ConvertJsonSchemaObjectToPoco(schema, out className));
            //Console.ReadLine();
            return ConvertJsonSchemaObjectToPoco(schema, out className);
        }

        private static StringBuilder ConvertJsonSchemaObjectToPoco(JsonSchema schema, out string className)
        {
            var sb = new StringBuilder();
            sb.Append("class ");
            //if(schema.Title != null)

            //    className = GenerateSlug(schema.Title);

            //else

            className = String.Format("Poco_{0}", Guid.NewGuid().ToString().Replace("-", string.Empty));
            // Poco_3f692ebee83e4e278903b234173c5974
            

            sb.Append(className);
            sb.AppendLine(" {");

            foreach (var item in schema.Properties)
            {
                sb.Append("  public ");
                // here the type for string is wrong ??????
                //Console.WriteLine(PropertyType(item.Value, sb));
                //Console.ReadLine();
                sb.Append(PropertyType(item.Value, sb));
                sb.Append(" ");
                sb.Append(item.Key.Trim());
                sb.AppendLine(" { get; set; }");
            }

            sb.AppendLine("}");
            return sb;
        }

        private static string PropertyType(JsonSchema jsonSchema, StringBuilder sb)
        {

            //Console.WriteLine(jsonSchema.Type);
            //Console.ReadLine();
            switch (jsonSchema.Type)
            {
                case JsonSchemaType.Array:
                    if (jsonSchema.Items.Count == 0)
                        return "IEnumerable<object>";
                    if (jsonSchema.Items.Count == 1)
                        return String.Format("IEnumerable<{0}>", PropertyType(jsonSchema.Items.First(), sb));
                    
                    throw new Exception("Not sure what type this will be.");

                case JsonSchemaType.Boolean:
                    return String.Format("bool{0}", jsonSchema.Required.HasValue && jsonSchema.Required.Value ? string.Empty : Nullable);

                case JsonSchemaType.Float:
                    return String.Format("float{0}", jsonSchema.Required.HasValue && jsonSchema.Required.Value ? string.Empty : Nullable);

                case JsonSchemaType.Integer:
                    return String.Format("int{0}", jsonSchema.Required.HasValue && jsonSchema.Required.Value ? string.Empty : Nullable);

                case JsonSchemaType.String:
                    return "string";

                case JsonSchemaType.Object:
                    string className;
                    sb.Insert(0, ConvertJsonSchemaObjectToPoco(jsonSchema, out className));
                    return className;

                case JsonSchemaType.None:
                case JsonSchemaType.Null:
                default:
                    return "object";
            }
        }
    }*/
}
