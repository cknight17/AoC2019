val globalBuffer = Queue.mkQueue() : int Queue.queue;
fun getOpCode input = 
let
    fun strFromCharList input = valOf (Int.fromString (String.concat(map (fn x => Char.toString x) input)))
    fun intFromChar input = valOf (Int.fromString (Char.toString input))
    fun opCodeFromChars (x1::x2::x3::x4::x5::[]) = ((intFromChar x3),(intFromChar x2),(intFromChar x1),(strFromCharList [x4,x5]))
    |   opCodeFromChars (x2::x3::x4::x5::[]) = ((intFromChar x3),(intFromChar x2),0,(strFromCharList [x4,x5]))
    |   opCodeFromChars (x3::x4::x5::[]) = ((intFromChar x3),0,0,(strFromCharList [x4,x5]))
    |   opCodeFromChars (xs) = (0,0,0,(strFromCharList xs))
in
    (opCodeFromChars (String.explode (Int.toString input)))
end;
fun readFile (inFile : string) = 
    let
        val ins = TextIO.openIn inFile
        fun loop ins = 
            case TextIO.inputLine ins of 
                SOME line => line 
            |   NONE => ""
    in
        loop ins before TextIO.closeIn ins
end;
fun safeGetItem index inputData = if index >= (Array.length inputData) then 0 else Array.sub(inputData,index);
fun getOp index inputData = ((getOpCode (safeGetItem index inputData)), (safeGetItem (index+1) inputData), (safeGetItem (index+2) inputData), (safeGetItem (index+3) inputData));
fun processOp ((p1,p2,p3,1), (x1:int), x2, addr) inputData = 
    let 
        val nx1 = if p1=0 then Array.sub(inputData,x1) else x1
        val nx2 = if p2=0 then Array.sub(inputData,x2) else x2
        val updated = Array.update(inputData, addr, (nx1+nx2))
    in
        (0, 4)
    end
| processOp ((p1,p2,p3,2), (x1:int), x2, addr) inputData = 
    let 
        val nx1 = if p1=0 then Array.sub(inputData,x1) else x1
        val nx2 = if p2=0 then Array.sub(inputData,x2) else x2
        val updated = Array.update(inputData, addr, (nx1*nx2))
    in
        (0, 4)
    end
| processOp ((p1,p2,p3,3), (x1:int), x2, addr) inputData = 
    let 
        val input = Queue.dequeue globalBuffer
        val updated = Array.update(inputData, x1, input)
        val _ = print ("Array[" ^ (Int.toString x1) ^ "] updated to " ^ (Int.toString input))
    in
        (0, 2)
    handle Queue.Dequeue => (1,0) 
    end
| processOp ((p1,p2,p3,4), (x1:int), x2, addr) inputData = 
    let 
        val nx1 = if p1=0 then Array.sub(inputData,x1) else x1
        val x = print ("Enqueue " ^ (Int.toString nx1) ^ " into buffer\n");
        val output = Queue.enqueue (globalBuffer,nx1)
    in
        (0, 2)
    end
| processOp ((p1,p2,p3,p4), (x1:int), x2, addr) inputData = 
    let
        val _ = print ("(" ^ (Int.toString p1) ^ "," ^(Int.toString p2) ^ "," ^(Int.toString p3) ^ "," ^ (Int.toString p4) ^ ") not found. Exiting\n")
    in
        (1,1)
    end;
fun processOps index xArray = 
    let
        val (stop, offset) = processOp (getOp index xArray) xArray
    in
        if (index >= (Array.length xArray)) then (index,xArray) 
        else if (stop = 1) then ((index+offset),xArray)
        else (processOps (index + offset) xArray)
    end;

fun getInput filename = 
    let
        val stringInput = readFile filename
        val tokenizedInput = map (fn x => valOf (Int.fromString x)) (String.tokens (fn c => c = #",") stringInput)
    in
        Array.fromList(tokenizedInput)
    end;
fun processArray inputArray = processOps 0 inputArray;

fun processProgram filename = 
    let
        val inputArray = (getInput filename)
    in
        processArray inputArray
    end;
fun processProgramWithInput filename x1 x2 = 
    let
        val inputArray = (getInput filename)
        val nx1 = Array.update(inputArray,1,x1)
        val nx2 = Array.update(inputArray,2,x2)
    in
        processArray inputArray
    end;
fun seekValue filename x1 x2 searchValue =
    let
        val (newOffset,output) = processProgramWithInput filename x1 x2
    in
     if ((Array.sub(output,0)) = searchValue) then ((x1*100)+x2)
     else if x1 = 99 then seekValue filename 0 (x2+1) searchValue
     else seekValue filename (x1+1) x2 searchValue 
    end;

fun processProgramWithBuffer filename = 
    let
        val inputArray = (getInput filename)
    in
        processArray inputArray
    end;

val val4 = Queue.enqueue (globalBuffer,1);

val nval = processProgramWithBuffer "input.txt";

val z = Queue.contents globalBuffer;
