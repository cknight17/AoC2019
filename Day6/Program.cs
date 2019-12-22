using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Day6
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] input = File.ReadAllLines("Day6/input1.txt");
            Day1(input);
            Day2(input);
        }

        static void Day1(string[] input) {
            Dictionary<string,string> orbits = new Dictionary<string, string>();
            //Dictionary<string,string> orbitedBy = new Dictionary<string, string>();
            List<string> allPlanets = new List<string>();
            foreach (var line in input) {
                string[] orbitPair = line.Split(")");
                orbits.Add(orbitPair[1],orbitPair[0]);
                //orbitedBy.Add(orbitPair[0],orbitPair[1]);
                allPlanets.Add(orbitPair[0]);
                allPlanets.Add(orbitPair[1]);
            }
            allPlanets = allPlanets.Distinct().ToList();
            int orbitCount = 0;
            foreach (var planet in allPlanets) {
                string current = planet;
                while (orbits.Keys.Contains(current)) {
                    orbitCount++;
                    current = orbits[current];
                }
            }
            Console.WriteLine(orbitCount);
        }
        // 136 too low  
        static void Day2(string[] input) {
            Dictionary<string,string> orbits = new Dictionary<string, string>();
            //Dictionary<string,string> orbitedBy = new Dictionary<string, string>();
            List<string> allPlanets = new List<string>();
            List<string> path = new List<string>();
            foreach (var line in input) {
                string[] orbitPair = line.Split(")");
                orbits.Add(orbitPair[1],orbitPair[0]);
                //orbitedBy.Add(orbitPair[0],orbitPair[1]);
                allPlanets.Add(orbitPair[0]);
                allPlanets.Add(orbitPair[1]);
            }
            allPlanets = allPlanets.Distinct().ToList();
            List<string> santaPath = new List<string>();
            string current = "SAN";
            while (orbits.Keys.Contains(current)) {
                santaPath.Add(orbits[current]);
                current = orbits[current];
            }

            List<string> youPath = new List<string>();
            current = "YOU";
            while (orbits.Keys.Contains(current)) {
                youPath.Add(orbits[current]);
                current = orbits[current];
            }

            Console.WriteLine("SAN");
            foreach (var item in santaPath) {
                Console.WriteLine("\t" + item);
            }
            Console.WriteLine();
            Console.WriteLine("YOU");
            foreach (var item in youPath) {
                Console.WriteLine("\t" + item);
            }
            List<string> finalPath = new List<string>();
            List<string> possiblePath = new List<string>();
            bool found = false;
            for (int i = 0; i < santaPath.Count; i++) {
                finalPath.Add(santaPath[i]);
                possiblePath = new List<string>();
                for (int j = 0; j < youPath.Count; j++) {
                    possiblePath.Add(youPath[j]);
                    if (youPath[j] == santaPath[i]) {
                        //finalPath = finalPath.Concat(youPath).ToList();
                        found = true;
                        break;
                    }
                }
                if (found) {
                    break;
                }
            }
            Console.WriteLine(found);
            foreach (var item in finalPath) {
                Console.WriteLine(item);
            }
            Console.WriteLine(found);
            foreach (var item in possiblePath) {
                Console.WriteLine(item);
            }
            Console.WriteLine(finalPath.Count + possiblePath.Count - 2);
            // int orbitCount = 0;
            // string currentSanta = orbits["SAN"];
            // int currentSantaCount = 0;
            // string currentYou = orbits["YOU"];
            // int currentYouCount = 0;
            // string current = currentYou;
            // Boolean found = false;
            // List<string> youPath = new List<string>();
            // while (orbits.Keys.Contains(currentSanta)) {
            //     currentYouCount = 0;
            //     youPath = new List<string>();
            //     while (orbits.Keys.Contains(current)) {
            //         if (current == currentSanta) {
            //             found = true;
            //             break;
            //         }
            //         currentYouCount++;
            //         youPath.Add(string.Format("{0} - {1}",current, orbits[current]));
            //         current = orbits[current];
            //     }
            //     if (found) {
            //         path = path.Concat(youPath).ToList();
            //         break;
            //     }
            //     path.Add(string.Format("{0} - {1}",currentSanta, orbits[currentSanta]));
            //     currentSanta = orbits[currentSanta];
            //     currentSantaCount++;
            // }
            // orbitCount = currentYouCount + currentSantaCount;
            // Console.WriteLine(found);
            // foreach (var pathItem in path) {
            //     Console.WriteLine(pathItem);
            // }
            // Console.WriteLine(orbitCount);
        }
    }
}
