val x  = [12,14,1969,100756];
fun mass x = x div 3 - 2;
val y = map mass x;
fun summation [] = 0
| summation (x::xs) = x + summation xs;
val y1 = summation y;

fun readFile (inFile : string) = 
let
    val ins = TextIO.openIn inFile
    fun loop ins = 
        case TextIO.inputLine ins of 
            SOME line => valOf (Int.fromString line) :: loop ins
        |   NONE => []
in
    loop ins before TextIO.closeIn ins
end;
val day1input = readFile "input.txt";
val massCalc = map mass day1input;
val day1output = summation massCalc;

fun mass2 x = 
    if mass x < 0 then 0
    else mass x + mass2 (mass x);
val day2mass = map mass2 day1input;
val day2output = summation day2mass;