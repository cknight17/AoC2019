using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Day3
{
    class Coordinate {
        public int x {get;set;}
        public int y {get;set;}
        public override string ToString() {
            return string.Format("({0},{1})",x,y);
        }
    }

    class Intersect {
        public Coordinate coordinate { get;set;}
        public LineBase line1 {get;set;}
        public LineBase line2 {get;set;}

        public int distance {
            get {
                Horizontal h = (line1 is Horizontal)?(Horizontal)line1:(Horizontal)line2;
                Vertical v = (line1 is Vertical)?(Vertical)line1:(Vertical)line2;
                int verticalDistance = Math.Abs(coordinate.y - v.ry1);
                int horizontalDistance = Math.Abs(coordinate.x - h.rx1);
                return verticalDistance + horizontalDistance;
            }
        }
    }

    interface LineBase {
        int index {get;set;}
        int distance {get;}
        
    }

    class Horizontal : LineBase {
        public int y {get;set;}
        private int _x1;
        public int x1 {
            get {
                return Math.Min(_x1,_x2);
            }
            set {
                _x1 = value;
            }
        }
        public int rx1 {
            get {
                return _x1;
            }
        }
        private int _x2;
        public int x2 {
            get {
                return Math.Max(_x1,_x2);
            }
            set {
                _x2 = value;
            }
        }
        public int rx2 {
            get {
                return _x2;
            }
        }
        public override string ToString() {
            return string.Format("Horitzontal {0} from {1} to {2} distance {3}", y, x1, x2, distance);
        }

        public int index {get;set;}
        public int distance {
            get {
                return Math.Abs(x2-x1);
            }
        }

    }

    class Vertical : LineBase {
        public int x {get;set;}
        private int _y1;
        public int y1 {
            get {
                return Math.Min(_y1,_y2);
            }
            set {
                _y1 = value;
            }
        }
        public int ry1 {
            get {
                return _y1;
            }
        }
        private int _y2; 
        public int y2 {
            get {
                return Math.Max(_y1,_y2);
            }
            set {
                _y2 = value;
            }
        }
        public int ry2 {
            get {
                return _y2;
            }
        }
        public override string ToString() {
            return string.Format("Vertical {0} from {1} to {2} distance {3}", x, y1, y2, distance);
        }

        public int index {get;set;}
        public int distance {
            get {
                return Math.Abs(y2-y1);
            }
        }
        public string direction {get;set;}
    }

    class LineSet {
        public LineSet() {
            Horizontals = new List<Horizontal>();
            Verticals = new List<Vertical>();
        }
        public List<Horizontal> Horizontals {get;set;}
        public List<Vertical> Verticals {get;set;}

        public List<LineBase> Lines { get {
            List<LineBase> lines = (from item in Horizontals
                select (LineBase)item).ToList();
            lines = lines.Concat((from item in Verticals select (LineBase)item).ToList()).ToList();
            return lines.OrderBy(item => item.index).ToList();
        }}
    }

    struct Line {
        Coordinate one {get;set;}
        Coordinate two {get;set;}
    }

    struct Instruction {
        public Instruction(string inst) {
            direction = inst.Substring(0,1);
            distance = Int32.Parse(inst.Substring(1));
        }
        public int distance {get;}
        public string direction {get;set;}
    }

    class PointMath {
        static int Distance(Coordinate one, Coordinate two) {
            return Math.Abs(one.x-two.x) + Math.Abs(one.y-two.y);
        }
    }

    class LineMath {
        public static Coordinate Intersect(Horizontal h, Vertical v) {
            // h.y = 3, h.x1 = 1, h.x2 = 5
            // v.x = 3, v.y1 = 1, v.y2 = 5
            //Console.WriteLine(h.ToString());
            //Console.WriteLine(v.ToString());
            if ((v.y1 <= h.y) && (h.y <= v.y2) && (h.x1 <= v.x) && (v.x <= h.x2))
            {
                //Console.WriteLine(string.Format("{0} | {1}", h, v));
                return new Coordinate() { x = v.x, y = h.y };
            }
            else {
                return null;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines(Directory.GetCurrentDirectory() + "/Day3/input.txt");
            var input2 = File.ReadAllLines(Directory.GetCurrentDirectory() + "/Day3/input.txt");
            // 1160 too high
            Console.WriteLine("Part 1:\n");
            Part1(input);
            Console.WriteLine("Part 2:\n");
            Part2(input2);
        }

        static void Part1(string[] input) {
            string wire1 = input[1];
            var wire1instructions = wire1.Split(",").Select(x => new Instruction(x));
            LineSet wire1path = GetLines(wire1instructions);
            string wire2 = input[0];
            var wire2instructions = wire2.Split(",").Select(x => new Instruction(x));
            LineSet wire2path = GetLines(wire2instructions);
            var intersects1 = GetIntersections(wire1path.Horizontals.Cast<LineBase>().ToList(),wire2path.Verticals.Cast<LineBase>().ToList());
            var intersects2 = GetIntersections(wire1path.Verticals.Cast<LineBase>().ToList(),wire2path.Horizontals.Cast<LineBase>().ToList());
            int minDistance1 = (from item in intersects1
                                select Math.Abs(item.coordinate.x) + Math.Abs(item.coordinate.y)).Min();
            int minDistance2 = (from item in intersects2
                                select Math.Abs(item.coordinate.x) + Math.Abs(item.coordinate.y)).Min();
            Console.WriteLine(minDistance1);
            Console.WriteLine(minDistance2);
        }
        static void Part2(string[] input) {
            string wire1 = input[1];
            var wire1instructions = wire1.Split(",").Select(x => new Instruction(x));
            LineSet wire1path = GetLines(wire1instructions);
            string wire2 = input[0];
            var wire2instructions = wire2.Split(",").Select(x => new Instruction(x));
            LineSet wire2path = GetLines(wire2instructions);
            var intersects1 = GetIntersections(wire1path.Horizontals.Cast<LineBase>().ToList(),wire2path.Verticals.Cast<LineBase>().ToList());
            var intersects2 = GetIntersections(wire1path.Verticals.Cast<LineBase>().ToList(),wire2path.Horizontals.Cast<LineBase>().ToList());
            var allIntersects = intersects1.Concat(intersects2);
            List<int> wireDistances = new List<int>();
            foreach (Intersect intersect in allIntersects) {
                wireDistances.Add(getTotalDistance(intersect,wire1path.Lines,wire2path.Lines));
            }
            Console.WriteLine(wireDistances.Min());
        }
        static LineSet GetLines(IEnumerable<Instruction> instructions) {
            LineSet results = new LineSet();
            Coordinate start = new Coordinate() { x = 0, y = 0 };
            Coordinate end = new Coordinate();
            int index = 0;
            foreach (var instruction in instructions) {
                switch (instruction.direction) {
                    case "U": 
                    end = new Coordinate() { x = start.x, y = start.y + instruction.distance };
                    results.Verticals.Add(new Vertical() { x = start.x, y1 = start.y, y2 = end.y, index = index });
                    
                    break;
                    case "D": 
                    end = new Coordinate() { x = start.x, y = start.y - instruction.distance };
                    results.Verticals.Add(new Vertical() { x = start.x, y1 = start.y, y2 = end.y, index = index });
                    
                    break;
                    case "L": 
                    end = new Coordinate() { x = start.x - instruction.distance, y = start.y };
                    results.Horizontals.Add(new Horizontal() { y = start.y, x1 = start.x, x2 = end.x, index = index });
                    
                    break;
                    case "R": 
                    end = new Coordinate() { x = start.x + instruction.distance, y = start.y };
                    results.Horizontals.Add(new Horizontal() { y = start.y, x1 = start.x, x2 = end.x, index = index });
                    
                    break;
                    default:
                    throw new InvalidCastException("Direction not found " + instruction.direction);
                }
                //Console.WriteLine(string.Format("{0} to {1}",start.ToString(),end.ToString()));
                index++;
                start = end;
            }
            return results;
        }

        static List<Intersect> GetIntersections(List<LineBase> lineSet1, List<LineBase> lineSet2) {
            List<Intersect> intersects = new List<Intersect>();
            foreach (var p1 in lineSet1) {
                foreach (var p2 in lineSet2) {
                    Horizontal h = (p1 is Horizontal)?(Horizontal)p1:(Horizontal)p2;
                    Vertical v = (p1 is Vertical)?(Vertical)p1:(Vertical)p2;
                    Coordinate c = LineMath.Intersect(h,v);
                    if (c != null && !(c.x == 0 && c.y == 0))
                    {
                        var intersect = new Intersect() { coordinate = c, line1 = p1, line2 = p2 };
                        intersects.Add(intersect);
                        Console.WriteLine(string.Format("Found {0},{1} distance {4} {2} {3}",c.x,c.y,p1,p2, intersect.distance));
                    }
                }
            }
            return intersects;
        }

        static int getTotalDistance(Intersect intersect, List<LineBase> wire1Lines, List<LineBase> wire2Lines) {
            Console.WriteLine(string.Format("For intersect {0}", intersect.coordinate));
            Console.WriteLine(string.Format("\t{0}",intersect.distance));
            int distance = intersect.distance;
            for (int i = 0; i < intersect.line1.index; i++) {
                var line = (from item in wire1Lines where item.index == i select item).FirstOrDefault();
                Console.WriteLine(string.Format("\t{0}",line.distance));
                distance += line.distance;
            }
            for (int i = 0; i < intersect.line2.index; i++) {
                var line = (from item in wire2Lines where item.index == i select item).FirstOrDefault();
                Console.WriteLine(string.Format("\t{0}",line.distance));
                distance += line.distance;
            }
            Console.WriteLine("\t-----------");
            Console.WriteLine(string.Format("\t{0}",distance));
            return distance;
        }
    }
}
