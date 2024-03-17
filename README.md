# Object2Json
Adding TypeNameHandling to System.Text.Json

At this moment .NET core 8 and .NET core 9 lacks the TypeNameHandling which can be found in the Newtonsoft project.
This simple class is aiming to fill this gap to make the System.Text.Json namespace more usefull.

```c#
using Object2Json;

var json = ObjectJsonSerializer.Serialize(someobject, new JsonSerializerOptions()
{
	WriteIndented = true
});

var sameobject = ObjectJsonSerializer.DeSerialize(json);
```
