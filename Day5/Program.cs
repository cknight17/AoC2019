using System;
using System.IO;
using System.Collections.Generic;

// 190643 too low.

namespace Day2
{
    class Program
    {
        private  const int EXIT = 99;
        private  const int MULTIPLY = 2;
        private  const int ADD = 1;
        private const int INPUT = 3;
        private const int OUTPUT = 4;
        private const int JUMP_IF_TRUE = 5;
        private const int JUMP_IF_FALSE = 6;
        private const int LESS_THAN = 7;
        private const int EQUALS = 8;
        private const int TARGET = 19690720;

        private static List<int> inputBuffer = new List<int>() { 5 };
        private static int inputBufferPosition = 0;
        public static int nextInput {
            get {
                int input = inputBuffer[inputBufferPosition];
                inputBufferPosition++;
                return input;
            }
        }
        public static List<int> outputBuffer = new List<int>();

        static void Main(string[] args)
        {
            var input = File.ReadAllText(Directory.GetCurrentDirectory() + "/Day5/input5.txt");
            Part1(input);
            //Part2(input);
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
            Console.WriteLine("Output buffer:");
            foreach (var ouputItem in outputBuffer) {
                Console.WriteLine(ouputItem);
            }
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
            if (position >= program.Length)
                return false;
            try {
                var op = GetCurrentOperation(program,position);
                if (op.opcode == EXIT) {
                    return false;
                }
                else {
                    position = ProcessOperation(op,ref program,position);
                    position += op.opSize;
                    return true;
                }
            } catch (Exception e) {
                Console.WriteLine(e.Message );
                return false;
            }
        }

        static Operation GetCurrentOperation(string[] program, int position) {
            int startPosition = position;
            int opcode = (startPosition >= program.Length) ? 0 : Int32.Parse(program[startPosition]);
            int basecode = Int32.Parse(opcode.ToString().Substring(Math.Max(opcode.ToString().Length - 2,0)));
            return new Operation() { 
                opcode = opcode, 
                var1register = (startPosition+1 >= program.Length) ? 0 : Int32.Parse(program[startPosition+1]), 
                var2register = (startPosition+2 >= program.Length) ? 0 : Int32.Parse(program[startPosition+2]),
                resultregister = (startPosition+3 >= program.Length) ? 0 : Int32.Parse(program[startPosition+3]),
                opSize = (basecode == INPUT || basecode == OUTPUT) ? 2 : 4 };
        }

        static int ProcessOperation(Operation op, ref string[] program, int position) {
            string opstr = op.opcode.ToString();
            int opcode = Int32.Parse(opstr.Substring(Math.Max(opstr.Length-2,0)));
            byte[] rtype = new byte[] {0,0,0};
            switch (opstr.Length) {
                case 5:
                    rtype[0] = byte.Parse(opstr[2].ToString());
                    rtype[1] = byte.Parse(opstr[1].ToString());
                    rtype[2] = byte.Parse(opstr[0].ToString());
                    break;
                case 4:
                    rtype[0] = byte.Parse(opstr[1].ToString());
                    rtype[1] = byte.Parse(opstr[0].ToString());
                    break;
                case 3:
                    rtype[0] = byte.Parse(opstr[0].ToString());
                    break;
            }
            int a = 0; 
            int b = 0; 
            Console.WriteLine(op);
            switch (opcode) {
                case ADD:
                    a = (op.var1register >= program.Length) ? 0 : (rtype[0] == 1) ? op.var1register : Int32.Parse(program[op.var1register]);
                    b = (op.var2register >= program.Length) ? 0 : (rtype[1] == 1) ? op.var2register : Int32.Parse(program[op.var2register]);
                    Console.WriteLine(string.Format("ADD: {0} + {1} = {4} stored to {2}({3})",a,b,op.resultregister,program[op.resultregister],a+b));
                    program[op.resultregister] = (a+b).ToString();
                break;
                case MULTIPLY:
                    a = (op.var1register >= program.Length) ? 0 : (rtype[0] == 1) ? op.var1register : Int32.Parse(program[op.var1register]);
                    b = (op.var2register >= program.Length) ? 0 : (rtype[1] == 1) ? op.var2register : Int32.Parse(program[op.var2register]);
                    Console.WriteLine(string.Format("MULTIPLY: {0} * {1} = {4} stored to {2}({3})",a,b,op.resultregister,program[op.resultregister],a*b));
                    program[op.resultregister] = (a*b).ToString();
                break;
                case INPUT:
                    string input = nextInput.ToString();
                    Console.WriteLine(string.Format("INPUT {0} from buffer stored to {1}({2})",input,op.var1register,program[op.var1register]));
                    program[op.var1register] = input;
                    
                break;
                case OUTPUT:
                    Console.WriteLine(string.Format("OUTPUT {0}",rtype[0] == 1 ? op.var1register : Int32.Parse(program[op.var1register])));
                    if (rtype[0] == 0)
                        outputBuffer.Add(Int32.Parse(program[op.var1register]));
                    else
                        outputBuffer.Add(op.var1register);
                break;
                case JUMP_IF_TRUE:
                    
                    int condition = rtype[0] == 1 ? op.var1register : Int32.Parse(program[op.var1register]);
                    int location = (rtype[1] == 1) ? op.var2register : Int32.Parse(program[op.var2register]);
                    Console.WriteLine(string.Format("JUMP IF TRUE {0} != 0 ? GOTO {1}", condition, location));
                    if (condition != 0) {
                        position = location;
                        op.opSize = 0;
                    }
                    else {
                        op.opSize = 3;
                    }
                    break;
                case JUMP_IF_FALSE:
                    
                    condition = rtype[0] == 1 ? op.var1register : Int32.Parse(program[op.var1register]);
                    location = (rtype[1] == 1) ? op.var2register : Int32.Parse(program[op.var2register]);
                    Console.WriteLine(string.Format("JUMP IF FALSE {0} == 0 ? GOTO {1}", condition, location));
                    if (condition == 0) {
                        position = location;
                        op.opSize = 0;
                    }
                    else {
                        op.opSize = 3;
                    }
                    break;
                case LESS_THAN:
                    int arg1 = rtype[0] == 1 ? op.var1register : Int32.Parse(program[op.var1register]);
                    int arg2 = (rtype[1] == 1) ? op.var2register : Int32.Parse(program[op.var2register]);
                    Console.WriteLine(string.Format("LESS THAN {0} < {1} ? {2}", arg1,arg2, op.resultregister));
                    if (arg1 < arg2) {
                        program[op.resultregister] = 1.ToString();
                    }
                    else {
                        program[op.resultregister] = 0.ToString();
                    }
                break;
                case EQUALS:
                    arg1 = rtype[0] == 1 ? op.var1register : Int32.Parse(program[op.var1register]);
                    arg2 = (rtype[1] == 1) ? op.var2register : Int32.Parse(program[op.var2register]);
                    Console.WriteLine(string.Format("EQUALS {0} == {1} ? {2}", arg1,arg2, op.resultregister));
                    if (arg1 == arg2) {
                        program[op.resultregister] = 1.ToString();
                    }
                    else {
                        program[op.resultregister] = 0.ToString();
                    }
                break;
                default:
                    throw new InvalidOperationException(string.Format("Opcode {0} not found in {1}", opcode, opstr));
            }
            return position;
        }
    }

    class Operation  {
        public int opcode;
        public int var1register;
        public int var2register;
        public int resultregister;
        public int opSize;
        public override string ToString() {
            return string.Format("opcode: {0} reg1: {1} reg2: {2} result: {3} size {4}", opcode,var1register,var2register,resultregister,opSize);
        }
    };
}
