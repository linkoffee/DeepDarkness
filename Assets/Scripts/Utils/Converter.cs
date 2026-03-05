using System;
using System.Text;


public class Converter
{
    public static string ToRoman(int number)
    {
        if (number < 1 || number > 3999)
            return "Too Big Number to Convert. Must be in scope (1-3999)";

        int[] numberValues = { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };
        string[] romanSymbols = { "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I"};

        System.Text.StringBuilder result = new System.Text.StringBuilder();

        for (int i = 0; i < numberValues.Length; i++)
        {
            while (number >= numberValues[i])
            {
                number -= numberValues[i];
                result.Append(romanSymbols[i]);
            }
        }
        return result.ToString();
    }
}