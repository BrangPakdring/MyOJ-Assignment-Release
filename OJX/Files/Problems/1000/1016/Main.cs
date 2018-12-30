using System;
using System.Linq;
public class Program
{
    public static void Main(string[]args)
        => Console.WriteLine(Array.ConvertAll(Console.ReadLine().Split(" \t\n\r".ToCharArray(), StringSplitOptions.RemoveEmptyEntries), int.Parse).Sum());
}