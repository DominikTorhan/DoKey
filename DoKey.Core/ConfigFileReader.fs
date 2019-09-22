namespace DoKey.Core

module ConfigFileReader =
    
    open System  
    open Domain 
    open System.IO
    
    let capsSymbol = "_CAPS_"
    let commadSymbol = "_COMMAND_"

    let RemoveComment(line: string) =
        let idx = line.IndexOf("//")
        match idx with  
            | -1 -> line
            | _ -> line.Replace(line.Substring(idx), "") 
    
    let SplitToLines(str: string) =
        str.Split([|"\n"|], StringSplitOptions.RemoveEmptyEntries) |> Array.toSeq   
    
    let SplitLine (line:string) = 
        let line' = line.Replace("\n", "").Replace("\r", "")
        line' .Split([|" "|], StringSplitOptions.RemoveEmptyEntries)     

    let GetStringByIdx (strs: string[]) idx= 
        match strs.Length with
            | 2 -> strs.[idx]
            | _ -> "" 

    let NormalizeString (str:string) =
        str.Replace("\n", "").Replace("\r", "").Replace(" ", "")

    let LineToMappedKey(line : string) =
        let strs = SplitLine line
        let trigger = GetStringByIdx strs 0
        let send = GetStringByIdx strs 1 |> NormalizeString
        let trigger' = trigger.Replace(capsSymbol, "") 
        let isCaps = trigger.Contains capsSymbol
        match trigger with
            | "" -> None
            | _ -> Some {trigger = trigger'; send = send; isCaps = isCaps } 
     
    let LineToCommandKey (line:string) =
        let strs = SplitLine line
        let trigger = GetStringByIdx strs 0
        let run = GetStringByIdx strs 1
        let trigger' = trigger.Replace(commadSymbol, "").Replace(capsSymbol, "")  
        match trigger.Contains commadSymbol with
            | false -> None
            | _ -> match trigger with
                    | "" -> None
                    | _ -> Some {trigger = trigger'; run = run } 

    let TextFileToMappedKeys text =
        SplitToLines text |> Seq.map RemoveComment |> Seq.map LineToMappedKey |> Seq.choose id 

    let TextFileToCommandKeys text =
        SplitToLines text |> Seq.map RemoveComment |> Seq.map LineToCommandKey |> Seq.choose id 
 
    let CreateConfigByFile(path: string) =  
        let text = File.ReadAllText path 
        {mappedKeys= TextFileToMappedKeys text; commandKeys = TextFileToCommandKeys text}
