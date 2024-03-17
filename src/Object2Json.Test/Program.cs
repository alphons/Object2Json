
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
		//File.WriteAllText("test2.json", jsoncopy);

		if (json == jsoncopy)
			Console.WriteLine("same");
	}

}