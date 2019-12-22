using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

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

        private Queue<int> inputBuffer;
        public List<int> outputBuffer;
        public bool halted;
        public string ID;
        public string[] program;

        public Program(string ID, string input) {
            this.inputBuffer = new Queue<int>();
            this.outputBuffer = new List<int>();
            this.halted = false;
            this.ID = ID;
            this.program = input.Split(",");
        }

        static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
        {
            if (length == 1) return list.Select(t => new T[] { t });
            return GetPermutations(list, length - 1)
                .SelectMany(t => list.Where(o => !t.Contains(o)),
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }

        static void Main(string[] args)
        {
            var input = File.ReadAllText(Directory.GetCurrentDirectory() + "/Day7/input7a.txt");
            List<int> vals= new List<int>() {4,3,2,1,0};
            int lastVal = 0;
            for (int i = 0; i < 5; i++) {
                Program d = new Program("7a",input);
                d.inputBuffer.Enqueue(vals[i]);
                d.inputBuffer.Enqueue(lastVal);
                d.Part1();
                lastVal = d.outputBuffer[d.outputBuffer.Count -1 ];
            }
            Console.WriteLine(string.Format("Answer: {0}",lastVal));
            input = File.ReadAllText(Directory.GetCurrentDirectory() + "/Day7/input7b.txt");
            vals= new List<int>() {0,1,2,3,4};
            lastVal = 0;
            for (int i = 0; i < 5; i++) {
                Program d = new Program("7b",input);
                d.inputBuffer.Enqueue(vals[i]);
                d.inputBuffer.Enqueue(lastVal);
                d.Part1();
                lastVal = d.outputBuffer[d.outputBuffer.Count -1 ];
            }
            Console.WriteLine(string.Format("Answer: {0}",lastVal));
            input = File.ReadAllText(Directory.GetCurrentDirectory() + "/Day7/input7c.txt");
            vals= new List<int>() {1,0,4,3,2};
            lastVal = 0;
            for (int i = 0; i < 5; i++) {
                Program d = new Program("7c",input);
                d.inputBuffer.Enqueue(vals[i]);
                d.inputBuffer.Enqueue(lastVal);
                d.Part1();
                lastVal = d.outputBuffer[d.outputBuffer.Count -1 ];
            }
            Console.WriteLine(string.Format("Answer: {0}",lastVal));

            var permutations = GetPermutations(new List<int> { 0,1,2,3,4 }, 5);
            List<int> allVals = new List<int>();
            foreach (var permutation in permutations) {
                input = File.ReadAllText(Directory.GetCurrentDirectory() + "/Day7/input7.txt");
                vals= permutation.ToList();
                lastVal = 0;
                for (int i = 0; i < 5; i++) {
                    Program d = new Program("Day 7 Part 1",input);
                    d.inputBuffer.Enqueue(vals[i]);
                    d.inputBuffer.Enqueue(lastVal);
                    d.Part1();
                    lastVal = d.outputBuffer[d.outputBuffer.Count -1 ];
                }
                allVals.Add(lastVal);
                Console.WriteLine(string.Format("Answer: {0}",lastVal));
            }
            Console.WriteLine(string.Format("Final answer: {0}",allVals.Max()));
            
            //permutations = GetPermutations(new List<int> { 5,6,7,8,9 }, 5);
            input = File.ReadAllText(Directory.GetCurrentDirectory() + "/Day7/input7b1.txt");
            permutations = new List<List<int>>() { new List<int>() { 9,8,7,6,5 } };
            allVals = new List<int>();
            foreach (var permutation in permutations) {
                List<Program> ds = new List<Program>();
                foreach (var item in permutation) {
                    Program p = new Program(string.Format("P{0}",item),input);
                    //p.inputBuffer.Enqueue(item);
                    ds.Add(p);
                }
                
                vals= permutation.ToList();
                lastVal = 0;
                int limit = 0;
                for (int i = 0; i < 5; i++) {
                    ds[i].inputBuffer.Enqueue(permutation.ToList()[i]);
                    ds[i].inputBuffer.Enqueue(lastVal);
                    ds[i].Part1();
                    lastVal = ds[i].outputBuffer[ds[i].outputBuffer.Count -1 ];
                    if (i ==4 && !ds[i].halted && limit < 5000000) {
                        i = 0;
                        limit++;
                    }
                }
                allVals.Add(lastVal);
                Console.WriteLine(string.Format("Answer: {0}",lastVal));
            }
            Console.WriteLine(allVals.Max());
            //Part2(input);
        }

        public void Part1() {
            //Console.WriteLine(string.Format("Processing {0}",this.ID));
            int position = 0;
            while (Process(ref program, ref position)) {

            }
            // foreach (var element in program) {
            //     Console.Write(element + ",");
            // }
            // Console.WriteLine("");
            // Console.WriteLine("Output buffer:");
            // foreach (var ouputItem in outputBuffer) {
            //     Console.WriteLine(ouputItem);
            // }
        }

        bool Process(ref string[] program, ref int position) {
            if (position >= program.Length)
                return false;
            try {
                var op = GetCurrentOperation(program,position);
                if (op.opcode == EXIT) {
                    Console.WriteLine("EXIT");
                    halted = true;
                    return false;
                }
                else {
                    position = ProcessOperation(op,ref program,position);
                    position += op.opSize;
                    return true;
                }
            } catch (Exception e) {
                //Console.WriteLine(e.Message );
                //halted = true;
                return false;
            }
        }

        Operation GetCurrentOperation(string[] program, int position) {
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

        int ProcessOperation(Operation op, ref string[] program, int position) {
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
            //Console.WriteLine(op);
            switch (opcode) {
                case ADD:
                    a = (op.var1register >= program.Length) ? 0 : (rtype[0] == 1) ? op.var1register : Int32.Parse(program[op.var1register]);
                    b = (op.var2register >= program.Length) ? 0 : (rtype[1] == 1) ? op.var2register : Int32.Parse(program[op.var2register]);
                    //Console.WriteLine(string.Format("ADD: {0} + {1} = {4} stored to {2}({3})",a,b,op.resultregister,program[op.resultregister],a+b));
                    program[op.resultregister] = (a+b).ToString();
                break;
                case MULTIPLY:
                    a = (op.var1register >= program.Length) ? 0 : (rtype[0] == 1) ? op.var1register : Int32.Parse(program[op.var1register]);
                    b = (op.var2register >= program.Length) ? 0 : (rtype[1] == 1) ? op.var2register : Int32.Parse(program[op.var2register]);
                    //Console.WriteLine(string.Format("MULTIPLY: {0} * {1} = {4} stored to {2}({3})",a,b,op.resultregister,program[op.resultregister],a*b));
                    program[op.resultregister] = (a*b).ToString();
                break;
                case INPUT:
                    string input = inputBuffer.Dequeue().ToString();
                    //Console.WriteLine(string.Format("INPUT {0} from buffer stored to {1}({2})",input,op.var1register,program[op.var1register]));
                    program[op.var1register] = input;
                    
                break;
                case OUTPUT:
                    //Console.WriteLine(string.Format("OUTPUT {0}",rtype[0] == 1 ? op.var1register : Int32.Parse(program[op.var1register])));
                    if (rtype[0] == 0)
                        outputBuffer.Add(Int32.Parse(program[op.var1register]));
                    else
                        outputBuffer.Add(op.var1register);
                break;
                case JUMP_IF_TRUE:
                    
                    int condition = rtype[0] == 1 ? op.var1register : Int32.Parse(program[op.var1register]);
                    int location = (rtype[1] == 1) ? op.var2register : Int32.Parse(program[op.var2register]);
                    //Console.WriteLine(string.Format("JUMP IF TRUE {0} != 0 ? GOTO {1}", condition, location));
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
                    //Console.WriteLine(string.Format("JUMP IF FALSE {0} == 0 ? GOTO {1}", condition, location));
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
                    //Console.WriteLine(string.Format("LESS THAN {0} < {1} ? {2}", arg1,arg2, op.resultregister));
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
                    //Console.WriteLine(string.Format("EQUALS {0} == {1} ? {2}", arg1,arg2, op.resultregister));
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
