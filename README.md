# Object2Json
Adding TypeNameHandling to System.Text.Json

At this moment .NET core 8 and .NET core 9 lacks the TypeNameHandling which can be found in the Newtonsoft project.

This simple class is aiming to fill this gap to make the System.Text.Json namespace more usefull.

When serializing to json an object consisting of lets say Dictionaries of objects (defined at runtime), can be
serialized where the AssemblyQualifiedName is saved as the first property of the object in json.

When deserializing, this property is searched for and the object is created according to its saved AssemblyQualifiedName.

Nested serializing and deserializing is used with theoretically 'infinite' depth.

```c#
using Object2Json;

var json = ObjectJsonSerializer.Serialize(someobject, new JsonSerializerOptions()
{
	WriteIndented = true
});

var sameobject = ObjectJsonSerializer.DeSerialize(json);
```
