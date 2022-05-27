using System;
using System.IO;
using System.Linq;
using System.Text;

namespace CodenameThomas_WordListHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //make sure the files are saved in UTF-8
                string filepathOne = $@"C:\Users\Charlie\Downloads\German\autocomplete.txt";
                string filepathTwo = $@"C:\Users\Charlie\Downloads\German\austriazismen.txt";
                string filepathThree = $@"C:\Users\Charlie\Downloads\German\helvetismen.txt";

                string filecontent = string.Empty;


                if (File.Exists(filepathOne) == false)
                    Console.WriteLine($"File not found:{Environment.NewLine}{filepathOne}");
                else
                    filecontent += File.ReadAllText(filepathOne, Encoding.UTF8);

                if (File.Exists(filepathTwo) == false)
                    Console.WriteLine($"File not found:{Environment.NewLine}{filepathTwo}");
                else
                    filecontent += File.ReadAllText(filepathTwo, Encoding.UTF8);

                if (File.Exists(filepathThree) == false)
                    Console.WriteLine($"File not found:{Environment.NewLine}{filepathThree}");
                else
                    filecontent += File.ReadAllText(filepathThree, Encoding.UTF8);

                var words = filecontent.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                //remove all words who are too long

                var fiveLetterWords = words.Where(f => f.Length == 5).ToList();

                //check if the first letter is a captial, so we use only nouns
                var fiveLetterWordsWithFirstCaptiatlLetter = fiveLetterWords.Where(f => char.IsUpper(f[0])).ToList();

                //shuffle the list
                var random = new Random();
                var shuffledFiveLetterWordsWithFirstCaptiatlLetter = fiveLetterWordsWithFirstCaptiatlLetter.OrderBy(f => random.Next()).ToList();

                string filepathNew = $@"C:\Users\Charlie\Downloads\German\5LettersWord.txt";

                StringBuilder bob = new StringBuilder();

                foreach (var word in shuffledFiveLetterWordsWithFirstCaptiatlLetter)
                {
                    bob.AppendLine("\"" + word +"\",");
                }

                File.WriteAllText(filepathNew, bob.ToString(), Encoding.UTF8);
                Console.WriteLine("Hello World!");
                Console.WriteLine("I am finished");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
            }
        }
    }
}
