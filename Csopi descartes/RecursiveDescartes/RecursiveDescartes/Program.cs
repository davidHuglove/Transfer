using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RecursiveDescartes
{

    class DescartesGenerator
    {
        public int fileCounter;
        public DescartesGenerator(int fileCounterStart = 0)
        {
            fileCounter = fileCounterStart;
        }
        public static Dictionary<string, dynamic> ParseDictionary(Dictionary<string, object> dictionary)
        {
            Dictionary<string, dynamic> arrayDict = new Dictionary<string, dynamic>();
            foreach (var res in dictionary)
            {
                if (res.Value.GetType().Equals(typeof(List<string>)))
                {
                    Console.WriteLine("A list array");
                    arrayDict.Add(res.Key, res.Value);
                }
                else if (res.Value.GetType().Equals(typeof(string)))
                {
                    Console.WriteLine("A simple");
                    arrayDict.Add(res.Key, ParseRanges((string)res.Value));
                }
                else if (res.Value.GetType().Equals(typeof(Dictionary<string, object>)))
                {
                    Console.WriteLine("A complex");
                    Dictionary<string, dynamic> recursiveDict = ParseDictionary((Dictionary<string, object>)res.Value, res.Key + ".");
                    foreach (var pair in recursiveDict)
                    {
                        arrayDict.Add(pair.Key, pair.Value);
                    }
                }

            }
            return arrayDict;

        }

        private static Dictionary<string, dynamic> ParseDictionary(Dictionary<string, object> dictionary, string path)
        {
            Dictionary<string, dynamic> arrayDict = new Dictionary<string, dynamic>();
            foreach (var res in dictionary)
            {

                if (res.Value.GetType().Equals(typeof(List<string>)))
                {
                    Console.WriteLine("A list array");
                    arrayDict.Add(path + res.Key, res.Value);
                }
                else if (res.Value.GetType().Equals(typeof(string)))
                {
                    Console.WriteLine("A simple");
                    arrayDict.Add(path + res.Key, ParseRanges((string)res.Value));
                }
                else if (res.Value.GetType().Equals(typeof(Dictionary<string, object>)))
                {
                    Console.WriteLine("A complex");
                    Dictionary<string, dynamic> recursiveDict = ParseDictionary((Dictionary<string, object>)res.Value, path + res.Key + ".");
                    foreach (var pair in recursiveDict)
                    {
                        arrayDict.Add(pair.Key, pair.Value);
                    }
                }


            }
            return arrayDict;
        }

        private static List<float> ParseRanges(string range)
        {
            List<float> array = new List<float>();
            if (range.Contains(",") && !range.Contains(":"))
            {
                string[] helper = range.Split(',');
                foreach (string str in helper)
                {
                    string temp = str.Trim();
                    try
                    {
                        array.Add(float.Parse(temp));
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine($"Unable to parse '{temp}'");
                    }
                }
            }
            else if (!range.Contains(",") && range.Contains(":"))
            {
                List<string> helper = range.Split(':').ToList();

                if (helper.Count != 3)
                {
                    Console.WriteLine("Bad pythonic number range format");
                }

                helper.ForEach(p => p.Trim());

                try
                {
                    float start = float.Parse(helper[0]);
                    float step = float.Parse(helper[1]);
                    float end = float.Parse(helper[2]);
                    for (float i = start; i <= end; i += step)
                    {
                        array.Add(i);
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine($"Unable to parse '{helper}'");
                }
            }
            else if (range.Contains(",") && range.Contains(":"))
            {
                Console.WriteLine("Number range has both enumeration and pythonic range. Only one is allowed");
                return null;
            }
            else
            {
                string temp = range.Trim();
                try
                {
                    array.Add(float.Parse(temp));
                }
                catch (FormatException)
                {
                    Console.WriteLine($"Unable to parse '{temp}'");
                }
            }
            return array;
        }

        public static void RecurseDeserialize(Dictionary<string, object> result)
        {
            foreach (var keyValuePair in result.ToArray())
            {
                Dictionary<string, object> dictionaries;
                List<string> enumList;
                try
                {
                    dictionaries = JsonConvert.DeserializeObject<Dictionary<string, object>>(keyValuePair.Value.ToString());
                    //Set the result as the dictionary
                    result[keyValuePair.Key] = dictionaries;

                    //Recurse
                    RecurseDeserialize(dictionaries);
                    continue;
                }
                catch
                {
                    try
                    {
                        enumList = JsonConvert.DeserializeObject<List<string>>(keyValuePair.Value.ToString());
                        //Set the result as the dictionary
                        result[keyValuePair.Key] = enumList;
                        continue;
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
        }

        public void RecursiveDescartes(Dictionary<string, dynamic> arrayDictionary, Dictionary<string, object> result, int level)
        {
            foreach (var element in arrayDictionary.ElementAt(level).Value)
            {
                string[] path = arrayDictionary.ElementAt(level).Key.Split(".");
                if (path.Length > 1)
                {
                    Dictionary<string, object> temp = (Dictionary<string, object>)result[path[0]];
                    int i = 1;
                    while (i < path.Length - 1)
                    {
                        temp = (Dictionary<string, object>)temp[path[i]];
                        i++;
                    }
                    temp[path[i]] = element;
                }
                else
                {
                    result[path[0]] = element;
                }

                if (level < arrayDictionary.Count - 1)
                {
                    RecursiveDescartes(arrayDictionary, result, level + 1);
                }
                else
                {
                    using (StreamWriter file = File.CreateText(@".\output\" + fileCounter.ToString() + ".json"))
                    {
                        fileCounter++;
                        JsonSerializer serializer = new JsonSerializer();
                        //serialize object directly into file stream
                        serializer.Serialize(file, result);
                    }
                }
            }
        }
    }

    class Program
    {

        
        static void Main(string[] args)
        {
            string json = @"{
                      'Network': {
                        'Size': '1:0.4:10',
                        'Type': [
                          'Grid1D',
                          'Grid2D',
                          'Complete'
                        ]
                      },
                      'StartTime': '2',
                      'Beta': '5',
                      'SimulationLenght': '1,5,7,8',
                      'InitialPhase': [
                        'Zero',
                        'High'
                      ]
                        }";
            var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            DescartesGenerator descartes = new DescartesGenerator(0);
            DescartesGenerator.RecurseDeserialize(result);

            var arrayDictionary = DescartesGenerator.ParseDictionary(result);

            descartes.RecursiveDescartes(arrayDictionary, result, 0);

        }
    }
}
