using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

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
    
    // extension metode som sjekker om strengen er en palindrom (leses likt baklengs og forover).
    public static bool Palindrome(this string s)
    {
        char[] charArray = s.ToCharArray();
        char[] reversedString = charArray.Reverse().ToArray();
        return charArray.SequenceEqual(reversedString);
    }
    
    // extension metode som fjerner alle mellomrom fra strengen
    public static string RemoveWhitespace(this string s) => Regex.Replace(s, @"\s+", "");
    
    // extension metode som bytter ut alle forekomster av et gitt ord med et annet ord i strengen.
    public static string ReplaceParts(this string s, string oldValue, string newValue, StringComparison comparisonType)
    {
        return s.Replace( oldValue, newValue, comparisonType);
    }
    
    // extension metode som returnerer de første N karakterene fra strengen.
    public static string ReturnNLetters(this string s, int n)
    {
        char[] charArray = s.ToCharArray();
        string returnString = string.Empty;
        for (int i = 0; i < n; i++)
            returnString += charArray[i];
        return returnString;
    }
    
    // extension metode som konverterer strengen til "Title Case" (hver første bokstav i hvert ord blir stor).
    public static string TitleCase(this string s) => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s.ToLower());
    
    // extension metode som fjerner alle spesialtegn og kun beholder bokstaver og tall.
    public static string RemoveSpecialCharacters(this string s)
    {
        StringBuilder returnString = new StringBuilder();
        foreach (char c in s)
        {
            if (char.IsLetterOrDigit(c) || char.IsWhiteSpace(c))
            {
                returnString.Append(c);
            }
        }
        return returnString.ToString();
    }
}