using ExtensionMethods;

int x = 55;
int y = 10699;
const string test = "Hello hello nice World @2024..! You, are most welcome to this nice extension method!";
const string testPalindrome = "ikki";
List<int> numlist = [21, 23, 344, 43, 35];

// Int Extensions
Console.WriteLine($"{x} er partall: {x.IsEven()}");
Console.WriteLine($"{x} er oddetall: {x.IsOdd()}");
Console.WriteLine($"Summen av siffrene i {y} er {y.SumOfDigits()}");
Console.WriteLine(y.ToFormattedString());
Console.WriteLine($"Antall siffre i {y} er {y.LenghtOfDigits()}");
Console.WriteLine($"Et tilfeldig tall mellom {x} og {y} er: {x.RandomNumber(y)}");
Console.WriteLine(x.IntToBinary());
Console.WriteLine($"Biggist number in list ({string.Join(",", numlist.ToArray()) }): {numlist.ReturnMaxNumber()}");
Console.WriteLine($"Average of numbers in list({string.Join(",", numlist.ToArray()) }): {numlist.CalcAverage()}");

// String Extensions
Console.WriteLine();
Console.WriteLine($"Antall ord i test- streng: {test.WordCount()}");
Console.WriteLine($"Streng i reversert rekkefølge: {test.ReverseString()}");
Console.WriteLine($"Er {test} et palindome?: {test.Palindrome()}");
Console.WriteLine($"Er {testPalindrome} et palindome?: {testPalindrome.Palindrome()}");
Console.WriteLine($"{test} uten mellomrom er: {test.RemoveWhitespace()}");
Console.WriteLine(test.ReplaceParts("t", "b", StringComparison.OrdinalIgnoreCase));
Console.WriteLine(test.ReplaceParts("Hello", "goodbye", StringComparison.OrdinalIgnoreCase));
Console.WriteLine(test.ReturnNLetters(10));
Console.WriteLine(test.TitleCase());
Console.WriteLine(test.RemoveSpecialCharacters());