using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BevMan.Web.OpenApi;

public class RequiredMemberFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        NullabilityInfoContext nullabilityContext = new();
        PropertyInfo[] properties = context.Type.GetProperties();

        foreach (PropertyInfo property in properties)
        {
            if (property.HasAttribute<JsonIgnoreAttribute>() || !property.HasAttribute<RequiredMemberAttribute>())
            {
                continue;
            }

            string jsonName = property.Name;
            if (property.HasAttribute<JsonPropertyNameAttribute>())
            {
                jsonName = property.GetCustomAttribute<JsonPropertyNameAttribute>()!.Name;
            }

            string? jsonKey = schema.Properties.Keys.SingleOrDefault(key =>
                string.Equals(key, jsonName, StringComparison.OrdinalIgnoreCase));

            if (string.IsNullOrWhiteSpace(jsonKey))
            {
                continue;
            }

            bool primitive = schema.Properties[jsonKey].Type != null;
            if (!primitive)
            {
                NullabilityInfo nullabilityInfo = nullabilityContext.Create(property);
                if (nullabilityInfo.ReadState == NullabilityState.Nullable)
                {
                    continue;
                }
            }

            schema.Properties[jsonKey].Nullable = false;
            schema.Required.Add(jsonKey);
        }
    }
}
