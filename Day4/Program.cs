using System;
using System.Collections.Generic;

namespace Day4
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] testInput = new int[] {111111,223450,123789};
            Console.WriteLine("Doubles?");
            foreach (var input in testInput) {
                Console.WriteLine("{0} ? {1}",input,process(input,true,false));
            }
            Console.WriteLine("Increasing?");
            foreach (var input in testInput) {
                Console.WriteLine("{0} ? {1}",input,process(input, false, true));
            }
            Console.WriteLine("All?");
            foreach (var input in testInput) {
                Console.WriteLine("{0} ? {1}",input,process(input));
            }

            testInput = new int[] {112233,123444,111122};
            Console.WriteLine("At least 2?");
            foreach (var input in testInput) {
                Console.WriteLine("{0} ? {1}",input,process(input,true,false));
            }
            Console.WriteLine("Only 2?");
            foreach (var input in testInput) {
                Console.WriteLine("{0} ? {1}",input,process(input,true,false,true));
            }
            Console.WriteLine("Increasing?");
            foreach (var input in testInput) {
                Console.WriteLine("{0} ? {1}",input,process(input, false, true));
            }
            Console.WriteLine("All?");
            foreach (var input in testInput) {
                Console.WriteLine("{0} ? {1}",input,process(input, true, true, true));
            }

            string inputRange = "168630-718098";
            Day1(inputRange);
            Day2(inputRange);
        }

        static void Day1(string input) {
            string[] range = input.Split('-');
            int start = Int32.Parse(range[0]);
            int end = Int32.Parse(range[1]);
            int count = 0;
            for (int i = start; i <= end; i++) {
                if (process(i)) {
                    Console.WriteLine(string.Format("{0} {1}",i,process(i)));
                    count++;
                }
            }
            Console.WriteLine("{0} out of {1}",count,end-start);
        }

        static void Day2(string input) {
            string[] range = input.Split('-');
            int start = Int32.Parse(range[0]);
            int end = Int32.Parse(range[1]);
            int count = 0;
            for (int i = start; i <= end; i++) {
                if (process(i, true,true,true)) {
                    Console.WriteLine(string.Format("{0} {1}",i,process(i, true,true,true)));
                    count++;
                }
            }
            Console.WriteLine("{0} out of {1}",count,end-start);
        }

        static bool process(int input, bool checkDouble = true, bool checkDecreasing = true, bool has2Repeating = false) {
            bool success = true;
            if (checkDouble)
                success = success && HasDouble(input.ToString());
            if (checkDecreasing)
                success = success && HasDecreasing(input.ToString());
            if (has2Repeating)
                success = success && HasOnly2Repeating(input.ToString());
            return success;
        }

        static bool HasDouble(string input) {
            char[] inputChars = input.ToString().ToCharArray();
            char lastInput = inputChars[0];
            for (int i = 1; i < inputChars.Length; i++) {
                if (inputChars[i] == lastInput)
                    return true;
                lastInput = inputChars[i];
            }
            return false;
        }

        static bool HasDecreasing(string input) {
            char[] inputChars = input.ToString().ToCharArray();
            char lastInput = inputChars[0];
            int lastDigit = Int32.Parse(lastInput.ToString());
            for (int i = 1; i < inputChars.Length; i++) {
                int currentDigit = Int32.Parse(inputChars[i].ToString());
                if (lastDigit > currentDigit)
                    return false;
                lastDigit = currentDigit;
            }
            return true;
        }

        static bool HasOnly2Repeating(string input) {
            char[] inputChars = input.ToString().ToCharArray();
            char lastInput = inputChars[0];
            List<int> repeatCounts = new List<int>();
            bool lastRepeat = false;
            bool currentRepeat = false;
            for (int i = 1; i < inputChars.Length; i++) {
                currentRepeat = inputChars[i] == lastInput;
                if (currentRepeat && !lastRepeat) {
                    repeatCounts.Add(2);
                    //Console.WriteLine(string.Format("Start {0}",repeatCounts[repeatCounts.Count - 1]));
                }
                if (currentRepeat && lastRepeat) {
                    repeatCounts[repeatCounts.Count - 1] = repeatCounts[repeatCounts.Count - 1]+1;
                    //Console.WriteLine(string.Format("Increment {0}",repeatCounts[repeatCounts.Count - 1]));
                }
                lastInput = inputChars[i];
                lastRepeat = currentRepeat;
            }
            //Console.Write("Repeats: ");
            foreach (var repeat in repeatCounts) {
                //Console.Write(string.Format("{0},",repeat));
            }
            //Console.WriteLine("");
            return repeatCounts.FindAll(item => item == 2).Count > 0;
        }
    }
}
