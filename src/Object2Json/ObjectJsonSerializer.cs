
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Object2Json;

public partial class ObjectJsonSerializer
{
	private const string PropClass = "__class__"; // Newtonsoft $type
	private const string Tabs = "\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t";
	public static string Serialize(object? value, JsonSerializerOptions? jsonSerializerOptions = null)
	{
		var sb = new StringBuilder();

		jsonSerializerOptions ??= JsonSerializerOptions.Default;
		SerializeInternal(sb, 0, value, jsonSerializerOptions);
		return sb.ToString();
	}

	[GeneratedRegex(@", Version=\d+.\d+.\d+.\d+|, Culture=\w+|, PublicKeyToken=\w+")]
	private static partial Regex LesserRegex();

	private static readonly Regex lesser = LesserRegex();

	private static string AssemblyLessQualifiedName(Type? t)
	{
		var n = t?.AssemblyQualifiedName;
		if (n != null)
			return lesser.Replace(n, string.Empty);
		else
			return string.Empty;
	}

	private static void SerializeInternal(StringBuilder sb, int level, object? o, JsonSerializerOptions jsonSerializerOptions)
	{
		if (o == null)
		{
			sb.Append("null");
			return;
		}

		var S = jsonSerializerOptions.WriteIndented ? Tabs[..level] : string.Empty;
		var SS = jsonSerializerOptions.WriteIndented ? Tabs[..(level+1)] : string.Empty;
		var NL = jsonSerializerOptions.WriteIndented ? Environment.NewLine : string.Empty;

		var t = o.GetType();

		switch (o)
		{
			case sbyte i:
				sb.Append(i);
				break;
			case byte i:
				sb.Append(i);
				break;
			case short i:
				sb.Append(i);
				break;
			case ushort i:
				sb.Append(i);
				break;
			case int i:
				sb.Append(i);
				break;
			case uint i:
				sb.Append(i);
				break;
			case long i:
				sb.Append(i);
				break;
			case ulong i:
				sb.Append(i);
				break;
			case Guid g:
				sb.Append($"\"{g}\"");
				break;
			case Enum e:
				sb.Append(Convert.ToInt32(e));
				break;
			case bool i:
				sb.Append(i ? "true" : "false");
				break;
			case float i:
				sb.Append(string.Format(CultureInfo.InvariantCulture, "{0:0.##}", i));
				break;
			case double i:
				sb.Append(string.Format(CultureInfo.InvariantCulture, "{0:0.##}", i));
				break;
			case decimal i:
				sb.Append(string.Format(CultureInfo.InvariantCulture, "{0:0.##}", i));
				break;
			case char s:
				sb.Append($"\"{s}\"");
				break;
			case string s:
				sb.Append($"\"{s}\"");
				break;
			case DateTime dt:
				sb.Append($"\"{dt}\"");
				break;
			case DateTimeOffset dt:
				sb.Append($"\"{dt}\"");
				break;
			case int[] intsbuffer:
				sb.Append($"[ {string.Join(',', intsbuffer)} ]");
				break;
			case byte[] bytesbuffer:
				sb.Append($"\"{Convert.ToBase64String(bytesbuffer)}\"");
				break;
			case IList list:
				if (list.Count == 0)
				{
					sb.Append("[]");
					break;
				}
				if (level > 0)
					sb.Append(NL);
				sb.Append($"{S}[{NL}");
				for (int j = 0; j < list.Count; j++)
				{
					sb.Append($"{SS}");
					SerializeInternal(sb, level + 1, list[j], jsonSerializerOptions);
					if (j < (list.Count - 1))
						sb.Append(',');
					sb.Append(NL);
				}
				sb.Append($"{S}]");
				break;
			case IDictionary dict:
				if (level > 0)
					sb.Append($"{NL}");
				sb.Append($"{S}{{{NL}{SS}\"{PropClass}\": \"{AssemblyLessQualifiedName(t)}\"");
				if (dict.Keys.Count > 0)
					sb.Append(',');
				sb.Append($"{NL}");
				var dicti = 0;
				foreach (var key in dict.Keys)
				{
					sb.Append($"{SS}\"{key}\": ");
					SerializeInternal(sb, level + 1, dict[key], jsonSerializerOptions);
					if (dicti < (dict.Keys.Count - 1))
						sb.Append(',');
					sb.Append(NL);
					dicti++;
				}
				sb.Append($"{S}}}");
				break;
			default:
				if (level > 0)
					sb.Append($"{NL}");
				sb.Append($"{S}{{{NL}{SS}\"{PropClass}\": \"{AssemblyLessQualifiedName(t)}\"");
				var fis = t.GetProperties(BindingFlags.Instance | BindingFlags.Public);
				if (fis.Length > 0)
					sb.Append(',');
				sb.Append($"{NL}");
				for (int i = 0; i < fis.Length; i++)
				{
					var fi = fis[i];
					sb.Append($"{SS}\"{fi.Name}\": ");
					SerializeInternal(sb, level + 1, fi.GetValue(o), jsonSerializerOptions);
					if (i < (fis.Length - 1))
						sb.Append(',');
					sb.Append(NL);
				}
				sb.Append($"{S}}}");
				break;
		}
	}

	public static T? DeSerialize<T>(string json, JsonSerializerOptions? jsonSerializerOptions = null)
	{
		var node = JsonNode.Parse(json);

		if (node == null)
			return default;

		return (T?)DeSerializeInternal(node, typeof(T), jsonSerializerOptions);

	}

	public static object? DeSerialize(string json, JsonSerializerOptions? jsonSerializerOptions = null)
	{
		return DeSerialize<object>(json, jsonSerializerOptions);

	}

	private static object? DeSerializeInternal(JsonNode? node, Type targetType, JsonSerializerOptions? jsonSerializerOptions)
	{
		if (node == null)
			return null;

		switch (node.GetValueKind())
		{
			case JsonValueKind.Object:
				var o = node.AsObject();

				if (o.Remove(PropClass, out JsonNode? className))
				{
					var t = Type.GetType(className!.ToString()) ?? throw new Exception($"can not find type {className}");
					var arguments = t.GetGenericArguments();

					var value = Activator.CreateInstance(t!);

					if (arguments.Length > 0)
					{
						var keyType = arguments[0];
						var valueType = arguments[1];
						if (value is IDictionary dict)
						{
							foreach (var prop in o)
							{
								var k = TypeDescriptor.GetConverter(keyType).ConvertFromInvariantString(prop.Key);
								var v = DeSerializeInternal(prop.Value, valueType, jsonSerializerOptions);
								dict.Add(k!, v);
							}
						}
						return value;
					}

					foreach (var prop in o)
					{
						var fi = t?.GetProperty(prop.Key);
						if (fi == null)
							continue;

						var v = DeSerializeInternal(prop.Value, fi.PropertyType, jsonSerializerOptions);
						if (v == null)
							continue;

						if (fi.CanWrite)
							fi.SetValue(value, v);
						else
							throw new Exception("can not set property");
					}
					return value;
				}
				break;
			case JsonValueKind.True:
			case JsonValueKind.False:
				return (bool?)node;
			case JsonValueKind.Number:
				return node.AsValue().Deserialize(targetType);
			case JsonValueKind.String:
				if (targetType == typeof(byte[]))
				{
					return Convert.ChangeType(Convert.FromBase64String((string)node!), targetType);
				}
				else if (targetType == typeof(Guid))
				{
					_ = Guid.TryParse((string?)node, out Guid guid);
					return guid;
				}
				else if (targetType == typeof(DateTimeOffset))
				{
					_ = DateTimeOffset.TryParse((string?)node, out DateTimeOffset dto);
					return dto;
				}
				return Convert.ChangeType((string?)node, targetType); // also datetime
			case JsonValueKind.Array:
				return node.AsArray().Deserialize(targetType);

			case JsonValueKind.Null:
			case JsonValueKind.Undefined:
			default:
				break;
		}

		return null;
	}

}
