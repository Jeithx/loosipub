using System.Text;
using System.Text.RegularExpressions;

namespace Generator
{
    public static class Helpers
    {
        public static string ExtractClassName(string fileContent)
        {
            var classNameMatch = Regex.Match(fileContent, @"class\s+(\w+)");
            return classNameMatch.Success ? classNameMatch.Groups[1].Value : throw new InvalidOperationException("Sınıf adı bulunamadı.");
        }

        public static (string Type, string Name)[] ExtractProperties(string fileContent)
        {
            var propertyMatches = Regex.Matches(fileContent, @"public\s+(\w+)\s+(\w+)\s*{");
            return propertyMatches
                .Select(m => (Type: m.Groups[1].Value, Name: m.Groups[2].Value))
                .ToArray();
        }

        public static CSharpDataType MapType(string typeName)
        {
            return typeName switch
            {
                "int" => CSharpDataType.Int,
                "short" => CSharpDataType.Short,
                "long" => CSharpDataType.Long,
                "byte" => CSharpDataType.Byte,
                "sbyte" => CSharpDataType.SByte,
                "uint" => CSharpDataType.UInt,
                "ushort" => CSharpDataType.UShort,
                "ulong" => CSharpDataType.ULong,
                "float" => CSharpDataType.Float,
                "double" => CSharpDataType.Double,
                "decimal" => CSharpDataType.Decimal,
                "char" => CSharpDataType.Char,
                "string" => CSharpDataType.String,
                "bool" => CSharpDataType.Bool,
                "object" => CSharpDataType.Object,
                "dynamic" => CSharpDataType.Dynamic,
                "DateTime" => CSharpDataType.DateTime,
                "void" => CSharpDataType.Void,
                _ => CSharpDataType.Class // Standart olmayan diğer türler için (örneğin custom class'lar)
            };
        }

        public static string GenerateDtoProperty(string typeName, string propertyName, string dtoSuffix, bool? isQuery = false)
        {
            var dataType = MapType(typeName);

            if ((dataType == CSharpDataType.Class || dataType == CSharpDataType.Struct) && isQuery == false)
            {
                return $"        public {typeName}{dtoSuffix} {propertyName} {{ get; set; }}";
            }
            else if (dataType == CSharpDataType.Class || dataType == CSharpDataType.Struct && isQuery == true)
            {
                return $"        public bool with{propertyName} {{ get; set; }}";
            }
            else
            {
                return $"        public {typeName} {propertyName} {{ get; set; }}";
            }
        }

        public static string GenerateContent(string className, (string Type, string Name)[] properties, string dtoSuffix, string baseClass, string @namespace, bool? isQuery = false)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"using Core.Models;");
            sb.AppendLine($"namespace {@namespace}");
            sb.AppendLine("{");
            sb.AppendLine($"    public class {className}{dtoSuffix}");
            sb.AppendLine("    {");

            if (dtoSuffix.Equals("Query")) //Sadece query içinde id yer almasını sağlar
                sb.AppendLine("        public long Id { get; set; }");

            foreach (var prop in properties)
            {
                sb.AppendLine(Helpers.GenerateDtoProperty(prop.Type, prop.Name, dtoSuffix, isQuery));
            }

            sb.AppendLine("    }");
            sb.AppendLine("}");

            return sb.ToString();
        }
    }
}
