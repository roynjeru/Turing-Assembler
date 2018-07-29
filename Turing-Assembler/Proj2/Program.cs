using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
namespace TuringAssembler
{

    class Program
    {
        public static StreamReader sr;
        public static string line;
        public static string[] states;
        public static string start;
        public static string accept;
        public static string reject;
        public static string[] alpha;
        public static string[] tapeAlpha;
        public static Dictionary<Tuple<string, string>, Tuple<string, string, string>> transitions = new Dictionary<Tuple<string, string>, Tuple<string, string, string>>();
        static void Main(string[] args)
        {
            // Ask for path to file
            //StreamReader sr = null;
            //string line;
            Program generic = new Program();
            Console.Write("Please enter a path to the source file: ");
            string file = Console.ReadLine();
            file = "C:\\Proj2\\" + file;
            try
            {
                StreamReader sr = new StreamReader(@file);
            }
            catch (Exception e)
            {
                Console.WriteLine("Problem locating the file");
                Console.WriteLine(e.Message);
                Console.ReadKey();
            }
            sr = new StreamReader(@file);
            // Start reading the file
            try
            {
                myParser();
            }
            catch (Exception e)
            {
                Console.WriteLine("Problem reading the file");
                Console.WriteLine(e.Message);
                Console.ReadKey();
            }
            //tape 
            if(isValidAsm())
            {
                Console.Write("Please enter a word: ");
                string word = Console.ReadLine();
                word = word.Trim();
                //check that word is in alphabet
                configFunc(word);

                Console.ReadKey();
            }
            else { Console.WriteLine("Text file does not contain valid input"); }
            Console.ReadKey();
            

        }
        static bool isValidAsm()
        {
            if(accept == reject) { return false; }
            if (alpha.Contains("_")) { return false; }
            if (states.Contains(start) && states.Contains(accept)&& states.Contains(reject)) { return true; }
            else { return false; }
        }
        static void configFunc(string aWord)
        {
            List<string> configTape = new List<string>();
            string statePH;
            string alphPH;
            string TalphPh;
            Tuple<string, string, string> result;
            Tuple<string, string> inputVal; 

            configTape.Add(start);
            foreach(char i in aWord)
            {
                configTape.Add(i.ToString()); //[Q0]01#01
            }
            configTape.Add("_");
            configTape.Add("_");
            foreach(string n in configTape) { Console.Write(n); } Console.WriteLine();
            int k = 0;
            while(k >= 0 && k < configTape.Count- 1)
            {
                statePH = configTape[k];
                alphPH = configTape[k + 1];
                inputVal = new Tuple<string, string>(statePH, alphPH);

                if(transitions.TryGetValue(inputVal, out result))
                {
                    configTape[k + 1] = result.Item2;
                    if(result.Item3 == "R")
                    {
                        TalphPh = configTape[k + 1];
                        configTape[k + 1] = result.Item1;
                        configTape[k] = TalphPh;
                    }
                    else if(result.Item3 == "L" && k > 0)
                    {
                        TalphPh = configTape[k - 1];
                        configTape[k - 1] = result.Item1;
                        configTape[k] = TalphPh;
                    }
                    // result.Item1 = destState result.Item2 = writeChar result.Item3 = L||R
                }
                else // not found 
                {
                    Console.WriteLine("Word rejected");
                    break;
                }
                k = configTape.FindIndex(a => a.StartsWith("[") == true);
                if(configTape[k]== accept)
                {
                    Console.Write("Accept: ");
                    foreach (string j in configTape)
                    {
                        Console.Write(j);
                    }
                    Console.WriteLine();
                    break;
                }
                foreach (string j in configTape)
                {
                    Console.Write(j);
                }
                Console.WriteLine();
                if(configTape[configTape.Count - 2]!= "_") { configTape.Add("_"); }                                                
            }
        }
        public static string RemoveComments(string str, string del)
        {
            return Regex.Replace(str, del + ".+", string.Empty).Trim();
        }
        static void myParser()
        {
            string[] elem;
            while ((line = sr.ReadLine()) != null)
            {
                line.TrimStart();
                if (line.StartsWith("--"))
                {
                    continue;
                }
                line = RemoveComments(line, "-");
                if (line.StartsWith("{states"))
                {
                    line = line.Substring(line.LastIndexOf(":") + 1);
                    line = line.TrimStart();
                    line = line.TrimEnd();
                    line = line.TrimEnd('}');
                    states = line.Split(',');
                    for(int i = 0; i < states.Length; i++){ states[i] = "[" + states[i] + "]"; }
                }
                else if (line.StartsWith("{start"))
                {
                    line = line.Substring(line.LastIndexOf(":") + 1);
                    line = line.TrimStart();
                    line = line.TrimEnd();
                    start = "["+line.TrimEnd('}')+"]";
                }
                else if (line.StartsWith("{accept")) 
                {
                    line = line.Substring(line.LastIndexOf(":") + 1);
                    line = line.TrimStart();
                    line = line.TrimEnd();
                    accept = "["+line.TrimEnd('}')+"]";
                }
                else if (line.StartsWith("{reject"))
                {
                    line = line.Substring(line.LastIndexOf(":") + 1);
                    line = line.TrimStart();
                    line = line.TrimEnd();
                    reject = "["+line.TrimEnd('}')+"]";
                }
                else if (line.StartsWith("{alpha"))
                {
                    line = line.Substring(line.LastIndexOf(":") + 1);
                    line = line.TrimStart();
                    line = line.TrimEnd();
                    line = line.TrimEnd('}');
                    alpha = line.Split(',');
                }
                else if (line.StartsWith("{tape"))
                {
                    line = line.Substring(line.LastIndexOf(":") + 1);
                    line = line.TrimStart();
                    line = line.TrimEnd();
                    line = line.TrimEnd('}');
                    tapeAlpha = line.Split(',');
                }
                else // Commands
                {
                    line = line.TrimStart();
                    line = line.TrimEnd();
                    line = line.TrimEnd(';');
                    if (!line.StartsWith("r"))
                    {
                        continue;
                    }
                    if (line.StartsWith("rwRt"))
                    {
                        line = line.Substring(line.LastIndexOf("rwRt") + 4);
                        line = line.TrimStart();
                        line = line.TrimEnd();
                        line = line.TrimEnd(';');
                        elem = line.Split(' '); //[Q0, 0, x, Q1]
                        transitions.Add(new Tuple<string, string>("[" + elem[0] + "]", elem[1]), new Tuple<string, string, string>("["+elem[3]+"]", elem[2], "R"));
                    }
                    if (line.StartsWith("rwLt"))
                    {
                        line = line.Substring(line.LastIndexOf("rwLt") + 4);
                        line = line.TrimStart();
                        line = line.TrimEnd();
                        line = line.TrimEnd(';');
                        elem = line.Split(' '); //[Q3, 0, x, Q5]
                        transitions.Add(new Tuple<string, string>("[" + elem[0] + "]", elem[1]), new Tuple<string, string, string>("[" + elem[3] + "]", elem[2], "L"));
                    }
                    if (line.StartsWith("rRl"))
                    {
                        line = line.Substring(line.LastIndexOf("rRl") + 3);
                        line = line.TrimStart();
                        line = line.TrimEnd();
                        line = line.TrimEnd(';');
                        elem = line.Split(' '); //[Q1, 0]
                        transitions.Add(new Tuple<string, string>("[" + elem[0] + "]", elem[1]), new Tuple<string, string, string>("[" + elem[0] + "]", elem[1], "R"));
                    }
                    if (line.StartsWith("rLl"))
                    {
                        line = line.Substring(line.LastIndexOf("rLl") + 3);
                        line = line.TrimStart();
                        line = line.TrimEnd();
                        line = line.TrimEnd(';');
                        elem = line.Split(' '); //[Q6, 1]
                        transitions.Add(new Tuple<string, string>("[" + elem[0] + "]", elem[1]), new Tuple<string, string, string>("[" + elem[0] + "]", elem[1], "L"));
                    }
                    if (line.StartsWith("rRt"))
                    {
                        line = line.Substring(line.LastIndexOf("rRt") + 3);
                        line = line.TrimStart();
                        line = line.TrimEnd();
                        line = line.TrimEnd(';');
                        elem = line.Split(' '); //[Q0, #, Q7]
                        transitions.Add(new Tuple<string, string>("[" + elem[0] + "]", elem[1]), new Tuple<string, string, string>("[" + elem[2] + "]", elem[1], "R"));
                    }
                    if (line.StartsWith("rLt"))
                    {
                        line = line.Substring(line.LastIndexOf("rLt") + 3);
                        line = line.TrimStart();
                        line = line.TrimEnd();
                        line = line.TrimEnd(';');
                        elem = line.Split(' '); //[Q5, #, Q6]
                        transitions.Add(new Tuple<string, string>("[" + elem[0] + "]", elem[1]), new Tuple<string, string, string>("[" + elem[2] + "]", elem[1], "L"));
                    }
                }
            }
            // Error checking for the parser subsets and all that // check to see if it is in list of states
        }
    }
}