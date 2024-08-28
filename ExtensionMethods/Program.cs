using ExtensionMethods;

int x = 55;
int y = 10000;

Console.WriteLine($"{x} er partall: {x.IsEven()}");
string test = "Hello World";
Console.WriteLine($"Antall ord i test- streng: {test.WordCount()}");
Console.WriteLine($"Streng i reversert rekkefølge: {test.ReverseString()}");
Console.WriteLine(y.ToFormattedString());