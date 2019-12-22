using System;
using System.IO;

// 190643 too low.

namespace Day2
{
    class Program
    {
        private  const int EXIT = 99;
        private  const int MULTIPLY = 2;
        private  const int ADD = 1;
        private const int TARGET = 19690720;
        static void Main(string[] args)
        {
            var input = File.ReadAllText(Directory.GetCurrentDirectory() + "/Day2/input1.txt");
            Part1(input);
            Part2(input);
        }

        static void Part1(string input) {
            var program = input.Split(",");
            int position = 0;
            while (Process(ref program, ref position)) {

            }
            foreach (var element in program) {
                Console.Write(element + ",");
            }
            Console.WriteLine("");
        }

        static void Part2(string input) {
            for (int noun = 0; noun < 100; noun++) {
                for (int verb = 0; verb < 100; verb++) {
                    var program = input.Split(",");
                    program[1] = noun.ToString();
                    program[2] = verb.ToString();
                    int position = 0;
                    while (Process(ref program, ref position)) {

                    }
                    int result = Int32.Parse(program[0]);
                    if (result == TARGET) {
                        Console.WriteLine(100 * noun + verb);
                        foreach (var element in program) {
                            Console.Write(element + ",");
                        }
                    }
                    
                }
            }
        }

        static bool Process(ref string[] program, ref int position) {
            var op = GetCurrentOperation(program,position);
            if (op.opcode == EXIT) {
                return false;
            }
            else {
                ProcessOperation(op,ref program,position);
                position++;
                return true;
            }
        }

        static Operation GetCurrentOperation(string[] program, int position) {
            int baseCode = position * 4;
            return new Operation() { 
                opcode = (baseCode >= program.Length) ? 0 : Int32.Parse(program[baseCode]), 
                var1register = (baseCode+1 >= program.Length) ? 0 : Int32.Parse(program[baseCode+1]), 
                var2register = (baseCode+2 >= program.Length) ? 0 : Int32.Parse(program[baseCode+2]),
                resultregister = (baseCode+3 >= program.Length) ? 0 : Int32.Parse(program[baseCode+3])};
        }

        static void ProcessOperation(Operation op, ref string[] program, int position) {
            int a = (op.var1register >= program.Length) ? 0 : Int32.Parse(program[op.var1register]);
            int b = (op.var2register >= program.Length) ? 0 : Int32.Parse(program[op.var2register]);
            switch (op.opcode) {
                case ADD:
                    program[op.resultregister] = (a+b).ToString();
                break;
                case MULTIPLY:
                    program[op.resultregister] = (a*b).ToString();
                break;
                default:
                    throw new InvalidOperationException(string.Format("Opcode {0} not found", op.opcode));
                break;
            }
        }
    }

    struct Operation  {
        public int opcode;
        public int var1register;
        public int var2register;
        public int resultregister;
    };
}
