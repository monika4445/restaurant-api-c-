using System.ComponentModel;
using System.Reflection;
using System.Text.Json.Nodes;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace RestaurantApi.Configuration;

public class DefaultValueParameterFilter : IParameterFilter
{
    public void Apply(IOpenApiParameter parameter, ParameterFilterContext context)
    {
        var attr = context.ParameterInfo?.GetCustomAttribute<DefaultValueAttribute>();
        if (attr?.Value is null) return;
        if (parameter is not OpenApiParameter concrete) return;

        var node = JsonValue.Create(attr.Value);
        concrete.Example = node;
        if (concrete.Schema is OpenApiSchema schema)
        {
            schema.Example = node;
            schema.Default = node;
        }
    }
}
