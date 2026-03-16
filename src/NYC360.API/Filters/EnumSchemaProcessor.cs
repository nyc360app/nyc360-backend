using NJsonSchema.Generation;

namespace NYC360.API.Filters;

public class EnumSchemaProcessor : ISchemaProcessor
{
    public void Process(SchemaProcessorContext context)
    {
        // 1. Safety checks
        if (context.ContextualType?.Type == null || context.Schema == null) return;

        var type = context.ContextualType.Type;
        var underlyingType = Nullable.GetUnderlyingType(type);
        if (underlyingType != null) type = underlyingType;

        if (!type.IsEnum) return;

        var schema = context.Schema;
        
        // 2. Do NOT touch ExtensionData here to avoid the "duplicated mapping key" error.
        // NSwag is likely already handling the 'enum' array of integers.

        // 3. Just build a clean HTML Legend for the description
        var legendHeader = "<b>Values:</b>";
        
        // Prevent double-appending if NSwag re-uses the schema object
        if (string.IsNullOrEmpty(schema.Description) || !schema.Description.Contains(legendHeader))
        {
            var names = Enum.GetNames(type);
            var legend = $"<br/>{legendHeader}<ul>";
            
            foreach (var name in names)
            {
                try
                {
                    var value = Convert.ChangeType(Enum.Parse(type, name), typeof(long));
                    legend += $"<li><code>{value}</code> = {name}</li>";
                }
                catch
                {
                    legend += $"<li>{name}</li>";
                }
            }
            legend += "</ul>";

            schema.Description = (schema.Description ?? string.Empty) + legend;
        }
    }
}