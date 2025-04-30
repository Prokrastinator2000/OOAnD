using System.Reflection;
using Scriban;

namespace SpaceBattle;

public class AdapterBuilder
{
    private readonly string _adapterName;
    private readonly string _adapterTypeName;
    private readonly string _targetTypeName;
    private readonly List<PropertyTemplateModel> _properties = new();

    public AdapterBuilder(Type adapterType, Type targetType)
    {
        _adapterName = adapterType.Name;
        _adapterTypeName = adapterType.Name;
        _targetTypeName = FormatGenericType(targetType);
    }

    public void CreateProperty(PropertyInfo p)
    {
        string? get = null, set = null;

        var propertyTypeName = FormatGenericType(p.PropertyType);

        if (p.CanRead)
        {
            get = $"return IoC.Resolve<{propertyTypeName}>(\"Game.{p.Name}.Get\", target);";
            //get = $"Helow";
        }

        if (p.CanWrite)
        {
            set = $"IoC.Resolve<_ICommand.ICommand>(\"Game.{p.Name}.Set\", target, value).Execute();";
        }

        _properties.Add(new PropertyTemplateModel
        {
            Name = p.Name,
            Type = propertyTypeName,
            Getter = get,
            Setter = set
        });
    }

    private static string FormatGenericType(Type type)
    {
        var typeName = type.Name;

        if (type.IsGenericType)
        {
            typeName = typeName[..typeName.IndexOf('`')];
            var typeArgs = type.GetGenericArguments().Select(FormatGenericType).ToArray();
            typeName = $"{typeName}<{string.Join(", ", typeArgs)}>";
        }

        return typeName;
    }

    public string Build()
    {
        const string templateText = @"class {{ adapter_name }}Adapter : {{ adapter_type_name }} {
            {{ target_type_name }} target;
            public {{ adapter_name }}Adapter({{ target_type_name }} target) => this.target = target;{{ for prop in properties }}
            public {{ prop.type }} {{ prop.name }} {
                {{ if prop.setter}}set { {{ prop.setter }} }{{ end }}
                {{ if prop.getter}}get { {{ prop.getter }} }{{ end }}
            }{{ end }}
        }";

        var template = Template.Parse(templateText);
        var result = template.Render(new
        {
            adapter_name = _adapterName,
            adapter_type_name = _adapterTypeName,
            target_type_name = _targetTypeName,
            properties = _properties
        });

        return result;
    }

    private class PropertyTemplateModel
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string? Getter { get; set; } = string.Empty;
        public string? Setter { get; set; } = string.Empty;
    }
}
