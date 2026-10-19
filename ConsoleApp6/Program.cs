using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace LabTask
{
    public class TextProcessor
    {
        private string text;

        public string Text => text;

        public TextProcessor(string text)
        {
            this.text = text ?? throw new ArgumentNullException(nameof(text));
        }

        private bool IsVowel(char c)
        {
            char lower = char.ToLower(c);
            string vowels = "аеєиіїоуюяaeiou";
            return vowels.Contains(lower);
        }

        public string[] GetWords()
        {
            MatchCollection matches = Regex.Matches(text, @"[а-яА-Яа-яЄєІіЇїҐґa-zA-Z]+");
            string[] words = new string[matches.Count];
            for (int i = 0; i < matches.Count; i++)
            {
                words[i] = matches[i].Value;
            }
            return words;
        }

        public int GetCountOfWordsWithEqualVowelsAndConsonants()
        {
            string[] words = GetWords();
            int count = 0;

            foreach (string word in words)
            {
                int vowels = 0;
                int consonants = 0;

                foreach (char c in word)
                {
                    if (char.IsLetter(c))
                    {
                        if (IsVowel(c))
                        {
                            vowels++;
                        }
                        else
                        {
                            consonants++;
                        }
                    }
                }

                if (vowels > 0 && vowels == consonants)
                {
                    count++;
                }
            }

            return count;
        }

        public string GetLongestWord()
        {
            string[] words = GetWords();
            if (words.Length == 0)
            {
                return string.Empty;
            }

            string longest = words[0];
            foreach (string word in words)
            {
                if (word.Length > longest.Length)
                {
                    longest = word;
                }
            }

            return longest;
        }
    }

    public class TextFileProcessor
    {
        private string inputFilePath;

        public TextFileProcessor(string inputFilePath)
        {
            this.inputFilePath = inputFilePath ?? throw new ArgumentNullException(nameof(inputFilePath));
        }

        public void RemoveFirstLineEndingWithExclamation(string outputFilePath)
        {
            if (!File.Exists(inputFilePath))
            {
                throw new FileNotFoundException("Вхідний файл не знайдено.", inputFilePath);
            }

            string[] lines = File.ReadAllLines(inputFilePath, Encoding.UTF8);
            bool removed = false;

            using (StreamWriter writer = new StreamWriter(outputFilePath, false, Encoding.UTF8))
            {
                foreach (string line in lines)
                {
                    if (!removed && line.TrimEnd().EndsWith("!"))
                    {
                        removed = true;
                        continue;
                    }
                    writer.WriteLine(line);
                }
            }
        }
    }

    public class FileTextProcessor
    {
        private string inputFilePath;

        public FileTextProcessor(string inputFilePath)
        {
            this.inputFilePath = inputFilePath ?? throw new ArgumentNullException(nameof(inputFilePath));
        }

        private bool IsVowel(char c)
        {
            char lower = char.ToLower(c);
            string vowels = "аеєиіїоуюяaeiou";
            return vowels.Contains(lower);
        }

        public string[] GetWords(string text)
        {
            MatchCollection matches = Regex.Matches(text, @"[а-яА-Яа-яЄєІіЇїҐґa-zA-Z]+");
            string[] words = new string[matches.Count];
            for (int i = 0; i < matches.Count; i++)
            {
                words[i] = matches[i].Value;
            }
            return words;
        }

        public string RemoveWordsStartingWithVowel(string text)
        {
            return Regex.Replace(text, @"\b[аеєиіїоуюяaeiouАЕЄИІЇОУЮЯAEIOU][а-яА-Яа-яЄєІіЇїҐґa-zA-Z]*\b", string.Empty);
        }

        public int GetWordCount(string text)
        {
            return GetWords(text).Length;
        }

        public void ProcessFileVariant13(string outputFilePath)
        {
            if (!File.Exists(inputFilePath))
            {
                throw new FileNotFoundException("Вхідний файл не знайдено.", inputFilePath);
            }

            string text = File.ReadAllText(inputFilePath, Encoding.UTF8);

            string modifiedText = RemoveWordsStartingWithVowel(text);
            modifiedText = Regex.Replace(modifiedText, @"\s+", " ").Trim();
            int wordCount = GetWordCount(text);

            using (StreamWriter writer = new StreamWriter(outputFilePath, false, Encoding.UTF8))
            {
                writer.WriteLine($"Кількість слів у тексті: {wordCount}");
                writer.WriteLine();
                writer.WriteLine("Текст після видалення всіх слів, що починаються з голосних літер:");
                writer.WriteLine(modifiedText);
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            while (true)
            {
                Console.WriteLine("\n=== ГОЛОВНЕ МЕНЮ ЛАБОРАТОРНОЇ РОБОТИ ===");
                Console.WriteLine("1. Завдання 1: Аналіз рядка (слова, голосні/приголосні, найдовше слово)");
                Console.WriteLine("2. Завдання 2: Обробка текстового файлу (видалити перший рядок з '!')");
                Console.WriteLine("3. Завдання 3: Обробка файлу (варіант 13 - видалити слова на голосні + підрахунок слів)");
                Console.WriteLine("0. Вихід");
                Console.Write("Ваш вибір: ");

                string choice = Console.ReadLine();

                if (choice == "0") break;

                switch (choice)
                {
                    case "1":
                        RunTask1();
                        break;
                    case "2":
                        RunTask2();
                        break;
                    case "3":
                        RunTask3();
                        break;
                    default:
                        Console.WriteLine("Некоректний вибір. Спробуйте ще раз.");
                        break;
                }
            }
        }

        static void RunTask1()
        {
            Console.Write("\nВведіть текстовий рядок: ");
            string input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Помилка! Рядок не може бути порожнім.");
                return;
            }

            TextProcessor processor = new TextProcessor(input);

            Console.WriteLine("\n--- Результати аналізу тексту ---");
            int equalCount = processor.GetCountOfWordsWithEqualVowelsAndConsonants();
            Console.WriteLine($"а) Кількість слів з однаковою кількістю голосних і приголосних: {equalCount}");

            string longestWord = processor.GetLongestWord();
            if (!string.IsNullOrEmpty(longestWord))
            {
                Console.WriteLine($"б) Найдовше слово: \"{longestWord}\" (довжина: {longestWord.Length} літер)");
            }
            else
            {
                Console.WriteLine("б) У введеному тексті не знайдено жодного слова.");
            }
        }

        static void RunTask2()
        {
            Console.Write("\nВведіть шлях до вхідного файлу (наприклад, input.txt): ");
            string inputPath = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(inputPath) || !File.Exists(inputPath))
            {
                Console.WriteLine("Помилка: файл не існує або шлях порожній.");
                return;
            }

            Console.Write("Введіть шлях до вихідного файлу (наприклад, output.txt): ");
            string outputPath = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(outputPath))
            {
                Console.WriteLine("Помилка: шлях до вихідного файлу не може бути порожнім.");
                return;
            }

            try
            {
                TextFileProcessor processor = new TextFileProcessor(inputPath);
                processor.RemoveFirstLineEndingWithExclamation(outputPath);
                Console.WriteLine("\nУспіх! Перший рядок, що закінчується на '!', видалено.");
                Console.WriteLine($"Результат записано у файл: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nСталася помилка під час обробки файлу: {ex.Message}");
            }
        }

        static void RunTask3()
        {
            Console.Write("\nВведіть шлях до вхідного файлу (наприклад, input.txt): ");
            string inputPath = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(inputPath) || !File.Exists(inputPath))
            {
                Console.WriteLine("Помилка: файл не існує або шлях порожній.");
                return;
            }

            Console.Write("Введіть шлях до вихідного файлу (наприклад, output.txt): ");
            string outputPath = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(outputPath))
            {
                Console.WriteLine("Помилка: шлях до вихідного файлу не може бути порожнім.");
                return;
            }

            try
            {
                FileTextProcessor processor = new FileTextProcessor(inputPath);
                processor.ProcessFileVariant13(outputPath);
                Console.WriteLine("\nУспіх! Файл успішно оброблено (Варіант 13).");
                Console.WriteLine($"Результат записано у файл: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nСталася помилка під час обробки файлу: {ex.Message}");
            }
        }
    }
}
