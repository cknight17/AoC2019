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
fun getOp index inputData = ((safeGetItem index inputData), (safeGetItem (index+1) inputData), (safeGetItem (index+2) inputData), (safeGetItem (index+3) inputData));
fun processOp (1, x1, x2, addr) inputData = 
    let 
        val updated = Array.update(inputData, addr, ((Array.sub(inputData,x1))+(Array.sub(inputData,x2))))
    in
        (0, 4)
    end
| processOp (2, x1, x2, addr) inputData = 
    let 
        val updated = Array.update(inputData, addr, ((Array.sub(inputData,x1))*(Array.sub(inputData,x2))))
    in
        (0, 4)
    end
| processOp (_, x1, x2, addr) inputData = (1,1);
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




val x1 = [1,9,10,3,2,3,11,0,99,30,40,50];
val x2 = Array.fromList(x1);
val day2Sample1 = getOp 0 x2;
val day2Sample2 = getOp 4 x2;
val day2Sample3 = getOp 8 x2;
val day2Sample4 = getOp 12 x2;
val (stop1, offset1) = processOp day2Sample1 x2;
val (stop1, offset1) = processOp day2Sample2 x2;
val (stop1, offset1) = processOp day2Sample3 x2;
val (stop1, offset1) = processOp day2Sample4 x2;


val output1 = processProgram "inputSample1.txt";
val output2 = processProgram "inputSample2.txt";
val output3 = processProgram "inputSample3.txt";
val output4 = processProgram "inputSample4.txt";
val output5 = processProgram "inputSample5.txt";
val output = processProgramWithInput "input.txt" 12 2;
val outputPart2 = seekValue "input.txt" 0 0 19690720;