namespace ExtensionMethods;

public static class IntExtension
{
    // Metode som sjekker partall
    public static bool IsEven(this int number)
    {
        return number % 2 == 0;
    }
    
    // Metode som sjekker oddetall med annen syntax
    public static bool IsOdd(this int number) => number % 2 != 0;
    
    // extension metode som returnerer tallet som en lesbar streng med tusenskillere (f.eks. 10000 -> "10,000")
    public static string ToFormattedString(this int number) => number.ToString("n2");
}