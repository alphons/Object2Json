
using System.Diagnostics;
using System.Text.Json;

namespace Object2Json.Test;

public class Tester
{

	public static void Main()
	{
		var dict = new Dictionary<string, object>();

		var t1 = new Test1()
		{
			Name = "alphons",
			Age = 50,
			Changed = DateTime.Now,
			Gender = TypeEnum.Male,
			Buf1 = [1, 2, 3, 4, 5]
		};

		t1.Buf2[3] = 0xff;

		t1.Doubles2.Add([1.12, 2.34, 3.45]);
		t1.Doubles2.Add([4.32, 3.21, 2.10]);

		//var json1 = BetterSerializer.Serialize(t1);
		//File.WriteAllText("test.json", json1);
		//return;

		var t2 = new Test1()
		{
			Name = "annet",
			Age = 51,
			Changed = DateTime.Now.AddDays(-1),
			Gender = TypeEnum.Female
		};

		t2.Dict.Add("par 1", 777.888M);
		t2.Dict.Add("par 2", 999.111M);

		t1.Partner = t2;

		dict.Add("man", t1);
		dict.Add("vrouw", t2);

		var COUNT = 10000;

		//var sw2 = Stopwatch.StartNew();
		//for (int i = 0; i < COUNT; i++)
		//{
		//	var json2 = JsonSerializer.Serialize(dict);
		//}
		//Console.WriteLine(sw2.ElapsedMilliseconds + "mS");

		//var sw1 = Stopwatch.StartNew();
		//for (int i = 0; i < COUNT; i++)
		//{
		//	var json = BetterSerializer.Serialize(dict);
		//}
		//Console.WriteLine(sw1.ElapsedMilliseconds + "mS");

		//var jsonA = JsonSerializer.Serialize(dict, new JsonSerializerOptions()
		//{
		//	WriteIndented = true
		//});

		//for (int i = 0; i < 10; i++)
		//{

		//	var jsonT = ObjectJsonSerializer.Serialize(dict, new JsonSerializerOptions()
		//	{
		//		WriteIndented = true
		//	});

		//	var dictcopyT = ObjectJsonSerializer.DeSerialize(jsonT);
		//}
		//// warm start ended
		//var swA = Stopwatch.StartNew();
		//for (int i = 0; i < COUNT; i++)
		//{

		//	var jsonT = ObjectJsonSerializer.Serialize(dict, new JsonSerializerOptions()
		//	{
		//		WriteIndented = true
		//	});

		//	var dictcopyT = ObjectJsonSerializer.DeSerialize(jsonT);
		//}
		//Console.WriteLine($"ended {swA.ElapsedMilliseconds}mS");
		//return;


		var json = ObjectJsonSerializer.Serialize(dict, new JsonSerializerOptions()
		{
			WriteIndented = true
		});
		File.WriteAllText("test.json", json);

		var dictcopy = ObjectJsonSerializer.DeSerialize(json);
		var jsoncopy = ObjectJsonSerializer.Serialize(dictcopy, new JsonSerializerOptions() 
		{ 
			WriteIndented = true 
		});

		if (json == jsoncopy)
			Console.WriteLine("same");
	}

}