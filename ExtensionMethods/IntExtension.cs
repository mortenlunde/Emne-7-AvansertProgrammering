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
    
    // Metode som finner summen av siffrene i et tall
    public static int SumOfDigits(this int number)
    {
        return number.ToString().Sum(digit => int.Parse(digit.ToString()));
    }
    
    // extension metode som returnerer tallet som en lesbar streng med tusenskillere (f.eks. 10000 -> "10,000")
    public static string ToFormattedString(this int number) => number.ToString("n2");
    
    // extension metode som tar et heltall som argument og returnerer antall sifre i tallet.
    public static int LenghtOfDigits(this int number) => number.ToString().Length;
    
    // extension metode som genererer et tilfeldig heltall innenfor et gitt område
    public static int RandomNumber(this int num1, int num2)
    {
        Random randNum = new Random();
        return randNum.Next(num1, num2);
    }
    
    // extension metode som konverterer et heltall til en binær streng.
    public static string IntToBinary(this int number)
    {
        return Convert.ToString(number, 2);
    }
    
    // extension metode som tar inn en liste med heltall som argument og returnerer det største tallet.
    public static int ReturnMaxNumber(this List<int> numberList) => numberList.Max();
    
    // extension metode som tar inn en liste med heltall som argument og returnerer gjennomsnittet.
    public static int CalcAverage(this List<int> numberList) => numberList.Sum() / numberList.Count;
}