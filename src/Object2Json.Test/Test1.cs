
namespace Object2Json.Test;

public enum TypeEnum
{
	None,
	Male,
	Female
}

public class Test1
{
	public TypeEnum Gender { get; set; }
	public string? ThisIsNull { get; set; } = null;
	public DateTime Changed { get; set; } = DateTime.Now;
	public string? Name { get; set; } = "alphons";

	public Guid MyGuid { get; set; } = Guid.NewGuid();

	// can not Set property
	public DateTimeOffset DateTimeOffset1 { get; set; } = new DateTimeOffset(DateTime.Now);

	public int Age { get; set; } = 51;

	public List<string> Tags { get; set; } = ["tag 1", "tag 2"];

	public Test1? Partner { get; set; }

	public ushort Shorter { get; set; } = 123;

	public bool Enabled { get; set; } = true;

	public Dictionary<string, decimal> Dict { get; set; } = new Dictionary<string, decimal>() { { "k1", 56.78M }, { "k2", 32.10M } };

	public Dictionary<Guid, bool> Dict2 { get; set; } = new Dictionary<Guid, bool>() { { Guid.NewGuid() , true }, { Guid.NewGuid() , false } };

	public int[]? Buf1 { get; set; }

	public byte[] Buf2 { get; set; } = new byte[20];

	public List<double> Doubles { get; set; } = [ 0.1234, 1.2123, 3.4123, 5.6123];

	public decimal Dec1 { get; set; } = 21.4567M;

	public List<List<double>> Doubles2 { get; set; } = [];

	public char CreditDebit { get; set; } = 'C';

}
