using System;
using System.IO;
using System.Text;

namespace TextFileTask
{
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

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            while (true)
            {
                Console.WriteLine("\n=== Меню обробки файлу ===");
                Console.WriteLine("1. Обробити текстовий файл (варіант 8)");
                Console.WriteLine("0. Вихід");
                Console.Write("Ваш вибір: ");

                string choice = Console.ReadLine();

                if (choice == "0") break;

                if (choice != "1")
                {
                    Console.WriteLine("Некоректний вибір. Спробуйте ще раз.");
                    continue;
                }

                Console.Write("Введіть шлях до вхідного файлу (наприклад, input.txt): ");
                string inputPath = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(inputPath) || !File.Exists(inputPath))
                {
                    Console.WriteLine("Помилка: файл не існує або шлях порожній.");
                    continue;
                }

                Console.Write("Введіть шлях до вихідного файлу (наприклад, output.txt): ");
                string outputPath = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(outputPath))
                {
                    Console.WriteLine("Помилка: шлях до вихідного файлу не може бути порожнім.");
                    continue;
                }

                try
                {
                    TextFileProcessor processor = new TextFileProcessor(inputPath);
                    processor.RemoveFirstLineEndingWithExclamation(outputPath);
                    Console.WriteLine("\n Успіх! Перший рядок, що закінчується на '!', видалено.");
                    Console.WriteLine($"Результат записано у файл: {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n Сталася помилка під час обробки файлу: {ex.Message}");
                }
            }
        }
    }
}