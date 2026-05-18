using System.ComponentModel;
using System.Reflection;
using System.Text.Json.Nodes;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace RestaurantApi.Configuration;

public class DefaultValueExampleFilter : ISchemaFilter
{
    public void Apply(IOpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type is null) return;
        if (schema is not OpenApiSchema concrete || concrete.Properties is null) return;

        foreach (var prop in context.Type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var attr = prop.GetCustomAttribute<DefaultValueAttribute>();
            if (attr?.Value is null) continue;

            var name = char.ToLowerInvariant(prop.Name[0]) + prop.Name[1..];
            if (!concrete.Properties.TryGetValue(name, out var propSchema)) continue;
            if (propSchema is not OpenApiSchema concreteProp) continue;

            var node = JsonValue.Create(attr.Value);
            concreteProp.Example = node;
            concreteProp.Default = node;
        }
    }
}
