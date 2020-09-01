using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1
{
    class OfflineCityBikeDataFetcher : ICityBikeDataFetcher
    {
        string numbers = "0123456789";
        public async Task<int> GetBikeCountInStation(string stationName)
        {

            for (int i = 0; i < stationName.Length; i++)
            {

                //if (Char.IsDigit(stationName[i]))
                //{
                //    throw new ArgumentException();
                //}

                if (numbers.Contains(stationName[i]))
                {
                    throw new ArgumentException(String.Format("\"{0}\"\nArgument cannot contain numbers\n", stationName));
                }
            }
            //Console.WriteLine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent + "\\bikedata.txt");


            // Visual studio code version for getting the bikedata.txt file
            string text = System.IO.File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "\\bikedata.txt");


            // Visual studio version for getting the bikedata.txt file
            //string text = System.IO.File.ReadAllText(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent + "\\bikedata.txt");

            //Console.WriteLine(text);

            string name = "";
            string number = "";
            bool found = false;
            int bikeCount = 0;

            // Parser
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '\n' && text[i - 1] == '\r')
                {
                    if (number.Length > 0)
                    {
                        if (!int.TryParse(number, out bikeCount))
                        {
                            Console.WriteLine("Couldnt parse number from string");
                        }
                        break;
                    }
                    name = "";
                }

                if (name.Length > 1)
                {
                    if (name[name.Length - 2] == ' ' && name[name.Length - 1] == ':')
                    {
                        name = name.Substring(0, name.Length - 2);
                        //Console.WriteLine(name);
                        //for (int j = 0; j < name.Length; j++)
                        //{
                        //    Console.Write(j + ":" + name[j] + ",");
                        //}
                        //Console.WriteLine();
                        if (name == stationName)
                        {
                            found = true;
                        }
                    }
                }

                if (!found && text[i] != '\n' && text[i] != '\r')
                {
                    name += text[i];
                }

                if (found && Char.IsDigit(text[i]))
                {
                    number += text[i];
                }
            }


            // For the last element 
            if (number.Length > 0)
            {
                if (!int.TryParse(number, out bikeCount))
                {
                    Console.WriteLine("Couldnt parse number from string");
                }

            }

            // If the corresponding station wasnt found throw an NotFoundException
            if (!found)
            {
                throw new NotFoundException("Station not found: ", stationName);
            }
            else
            {
                return bikeCount;
            }
        }
    }
}
