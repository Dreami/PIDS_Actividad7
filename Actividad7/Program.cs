using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileOutputs;
using System.IO;

namespace Actividad7
{
    class Program
    {
        static void Main(string[] args)
        {
            DirectoryInfo d = new DirectoryInfo(Outputs.getAllFiles());
            FileInfo[] Files = d.GetFiles("*.txt");

            string output_path7 = @"C:\Users\maple\Documents\9° Semester\CS13309_Archivos_HTML\a7_matricula.txt";
            string output;

            var watch = System.Diagnostics.Stopwatch.StartNew();
            List<string> sortedWords = new List<string>();

            int count = 0;
            Dictionary<String, Posting> postings = new Dictionary<string, Posting>();
            foreach (FileInfo file in Files)
            {

                output = "";
                var watchEach = System.Diagnostics.Stopwatch.StartNew();
                string htmlContent = File.ReadAllText(file.FullName);
                htmlContent.Trim();

                string[] eachWord = htmlContent.Split(' ');
                try
                {
                    foreach (string word in eachWord)
                    {
                        if (!string.IsNullOrEmpty(word))
                        {
                            if (!word.Equals(" "))
                            {
                                word.Replace(",", "")
                                    .Replace(".", "");
                            }

                            if (postings.ContainsKey(word))
                            {
                                postings[word].addWord(file.Name);
                            }
                            else
                            {
                                postings.Add(word, new Posting(word, file.Name, count));
                            }
                        }
                    }
                }
                catch (ArgumentNullException argExc)
                {
                    Console.WriteLine(argExc.StackTrace);
                }
                catch (KeyNotFoundException keyNotFoundExc)
                {
                    Console.WriteLine(keyNotFoundExc.StackTrace);
                }

                foreach (var value in postings.Values.ToList())
                {
                    if (Array.IndexOf(eachWord, value.getWord()) >= 0)
                    {
                        output += "\n" + value.printScore() + "\n";
                        //Console.WriteLine(postings[key].printScore());
                    }
                    else
                    {
                        postings.Remove(value.getWord());
                    }
                }

                output += "\n" + file.Name + " finished in " + watchEach.Elapsed.TotalMilliseconds.ToString() + " ms";
                Console.WriteLine(output);
                watchEach.Stop();
                Outputs.output_print(output_path7, output);
                count++;
            }

            output = "\nAll files sorted in\t" + watch.Elapsed.TotalMilliseconds.ToString() + " ms";
            Console.WriteLine(output);
            watch.Stop();
            Outputs.output_print(output_path7, output);

            Console.Read();
        }
    }
}
