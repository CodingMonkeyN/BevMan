namespace BevMan.Application.Common.Mappings;

public class MappingProfile : Profile
{
  public MappingProfile()
  {
    var assemblies = AppDomain.CurrentDomain.GetAssemblies()
      .Where(assembly => !string.IsNullOrEmpty(assembly.FullName));
    var types = assemblies.SelectMany(
        assembly => assembly
          .GetExportedTypes()
          .Where(
            type => type.GetInterfaces()
              .Any(interfaceType => interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IMapFrom<>))
          )
      )
      .ToList();

    foreach (var type in types)
    {
      var method = type.GetMethod("Mapping") ?? type.GetInterface("IMapFrom`1")?.GetMethod("Mapping");
      var instance = Activator.CreateInstance(type);
      method?.Invoke(instance, new object[] { this });
    }
  }
}
