using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.WriteLine("Введите ребус ");
        string rebus = Console.ReadLine();

        if (SolveRebus(rebus))
        {
            Console.WriteLine("Решение найдено!");
        }
        else
        {
            Console.WriteLine("Решение не найдено.");
        }
    }

    static bool SolveRebus(string rebus)
    {
        HashSet<char> letters = new HashSet<char>();
        foreach (char c in rebus)
        {
            if (char.IsLetter(c))
            {
                letters.Add(c);
            }
        }

        if (letters.Count > 10)
        {
            Console.WriteLine("Слишком много букв для решения.");
            return false;
        }

        List<int[]> permutations = GeneratePermutations(10, letters.Count);

        foreach (var perm in permutations)
        {
            Dictionary<char, int> mapping = new Dictionary<char, int>();//сопоствоялаем букцы и йифры
            int index = 0;
            foreach (char letter in letters)
            {
                mapping[letter] = perm[index++];
            }

            if (CheckRebus(rebus, mapping))
            {
                PrintSolution(rebus, mapping);
                return true;
            }
        }

        return false;
    }

    static bool CheckRebus(string rebus, Dictionary<char, int> mapping)
    {
        foreach (var word in rebus.Split(new[] { '+', '=', ' ' }, StringSplitOptions.RemoveEmptyEntries))
        {
            if (mapping[word[0]] == 0)
            {
                return false;
            }
        }

        string replaced = ReplaceLetters(rebus, mapping);
        string[] parts = replaced.Split('=');
        if (parts.Length != 2) return false;

        string[] leftSide = parts[0].Split('+');
        int sum = 0;

        foreach (string num in leftSide)
        {
            if (num.Trim().Length == 0) continue;
            if (num[0] == '0' && num.Length > 1) return false;
            sum += int.Parse(num);
        }

        int rightSide = int.Parse(parts[1].Trim());
        return sum == rightSide;
    }

    static string ReplaceLetters(string rebus, Dictionary<char, int> mapping)
    {
        string result = "";
        foreach (char c in rebus)
        {
            if (c == ' ') continue; // Пропускаю пробелы
            if (mapping.ContainsKey(c))
            {
                result += mapping[c].ToString();
            }
            else
            {
                result += c.ToString();
            }
        }
        return result;
    }


    static List<int[]> GeneratePermutations(int maxDigit, int length)
    {
        List<int[]> result = new List<int[]>();
        Permute(new int[maxDigit], new bool[maxDigit], 0, length, result);
        return result;
    }

    static void Permute(int[] current, bool[] used, int pos, int length, List<int[]> result)
    {
        if (pos == length)
        {
            result.Add((int[])current.Clone());
            return;
        }

        for (int i = 0; i < used.Length; i++)
        {
            if (!used[i])
            {
                current[pos] = i;
                used[i] = true;
                Permute(current, used, pos + 1, length, result);
                used[i] = false;
            }
        }
    }

    static void PrintSolution(string rebus, Dictionary<char, int> mapping)
    {
        Console.WriteLine("Решение:");

        Console.WriteLine("Ребус с цифрами: " + ReplaceLetters(rebus, mapping));
    }
}