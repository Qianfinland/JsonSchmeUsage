using Newtonsoft.Json.Schema;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;


namespace GenerateJsonSchema
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Write schema for creating schema and write poco to create poco:");
            string input = Console.ReadLine();

            if (input == "schema")
            {
                FromPocoToSchema();
            }
            if (input == "poco")
            {
                FromSchemaToPoco();
            }
            
        }

        // Step 1 generate json-schema from poco
        static void FromPocoToSchema()
        {
            var jsonSchemaGenerator = new JsonSchemaGenerator();
            var myType = typeof(Car);
            var schema = jsonSchemaGenerator.Generate(myType);
            //Console.WriteLine(schema); // get the json schema

            schema.Title = myType.Name;
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

        //step 2: generate poco from json-schema
        private const string Nullable = "?";
        static void FromSchemaToPoco()
        {
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
                TextWriter poco = File.CreateText(@"poco.txt");
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

                //case JsonSchemaType.Array:
                //    foreach (var item in schema.Items.Where(x => x.Type.HasValue && x.Type == JsonSchemaType.Object))
                //    {

                //        sb.Append(ConvertJsonSchemaObjectToPoco(item));

                //    }
                //    break;


            }

            return sb;
        }

        private static StringBuilder ConvertJsonSchemaObjectToPoco(JsonSchema schema)
        {
            string className;
            return ConvertJsonSchemaObjectToPoco(schema, out className);
        }

        private static StringBuilder ConvertJsonSchemaObjectToPoco(JsonSchema schema, out string className)
        {
            var sb = new StringBuilder();
            sb.Append("class ");
            if (schema.Title != null)
                className = schema.Title;

            else
                className = String.Format("Poco_{0}", Guid.NewGuid().ToString().Replace("-", string.Empty));
                // Poco_3f692ebee83e4e278903b234173c5974


            sb.Append(className);
            sb.AppendLine(" {");

            foreach (var item in schema.Properties)
            {
                //System.Collections.Generic.Dictionary'2[System.String.Newtonsoft.Json.Schema.JsonSchema]
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
            // how to iterate all the items
            //Console.WriteLine(jsonSchema.Items);
            //Console.ReadLine();
            //System.Collections.Generic.List'1[Newtonsoft.Json.Schema.JsonSchema]

            // new code to get property type
            var propertyTypeInString = jsonSchema.Type.Value.ToString().ToLower();
            var comma = propertyTypeInString.IndexOf(',');
            var typeResult = " ";
            var arrayType = " ";

            if (comma != -1)
                typeResult = propertyTypeInString.Substring(0, propertyTypeInString.IndexOf(','));
            if (typeResult == "array")
                arrayType = jsonSchema.Items[0].ToString();
            if (arrayType.Contains("integer"))
                return "int[]";
            if (arrayType.Contains("string") && arrayType.Contains("null"))
                return "string[]";
            if (arrayType.Contains("string") && (arrayType.Contains("null")== false))
                return "char[]";
            if (arrayType.Contains("number"))
                return "float[]";
            if (comma == -1)
                typeResult = propertyTypeInString;

            //debug type
            //Console.WriteLine(propertyTypeInString);
            //Console.WriteLine(typeResult);
            //Console.ReadLine();
            return typeResult;

            
            // old code to get property type
            //switch (jsonSchema.Type)
            //{

            //    case JsonSchemaType.Array:
            //        if (jsonSchema.Items.Count == 0)
            //            return "IEnumerable<object>";
            //        if (jsonSchema.Items.Count == 1)
            //            //return String.Format("IEnumerable<{0}>", PropertyType(jsonSchema.Items[0], sb));
            //            return "array";

            //        throw new Exception("Not sure what type this will be.");

            //    case JsonSchemaType.Boolean:
            //        return String.Format("bool{0}", jsonSchema.Required.HasValue && jsonSchema.Required.Value ? string.Empty : Nullable);

            //    case JsonSchemaType.Float:
            //        return String.Format("float{0}", jsonSchema.Required.HasValue && jsonSchema.Required.Value ? string.Empty : Nullable);

            //    case JsonSchemaType.Integer:
            //        return String.Format("int{0}", jsonSchema.Required.HasValue && jsonSchema.Required.Value ? string.Empty : Nullable);

            //    case JsonSchemaType.String:
            //        return "string";

            //    case JsonSchemaType.Object:
            //        //string className;
            //        //sb.Insert(0, ConvertJsonSchemaObjectToPoco(jsonSchema, out className));
            //        //return className;
            //        return "object";

            //    case JsonSchemaType.None:
            //        return "none";
            //    case JsonSchemaType.Null:
            //        return "null";
            //    case JsonSchemaType.Any:
            //        return "any";
            //    default:
            //        return "string";
            //}
        }
    }
}
