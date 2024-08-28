namespace ExtensionMethods;

public static class StringExtension
{
    // extension metode som teller antall ord i en streng.
    public static int WordCount(this string s) => s.Split(' ').Length;
    
    // extension metode som snur rekkefølgen på bokstavene i strengen.
    public static string ReverseString(this string s)
    {
        char[] charArray = s.ToCharArray();
        string reversedString = charArray.Reverse().Aggregate(string.Empty, (current, c) => current + c);
        return new string(reversedString.ToCharArray());
    }
}