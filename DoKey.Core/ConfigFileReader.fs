namespace DoKey.Core

module ConfigFileReader =
    
    open System  
    open Domain 
    open System.IO
     
    let RemoveComment(line: string) =
        let idx = line.IndexOf("//")
        match idx with  
            | -1 -> line
            | _ -> line.Replace(line.Substring(idx), "") 
    
    let SplitToLines(str: string) =
        let seq = str.Split([|"\n"|], StringSplitOptions.RemoveEmptyEntries) |> Array.toSeq   
        seq  
      
    let LineToMappedKey(line : string) =
        let strs = line.Split([|" "|], StringSplitOptions.RemoveEmptyEntries)     
        let trigger = match strs.Length with
            | 2 -> strs.[0]
            | _ -> "" 
        let send = match strs.Length with
            | 2 -> strs.[1]
            | _ -> "" 
        let strCaps = "_CAPS_" 
        let trigger' = trigger.Replace(strCaps, "") 
        let isCaps = trigger.Contains strCaps
        let send' = send.Replace("\n", "").Replace("\r", "")
        match trigger with
            | "" -> None
            | _ -> Some {trigger = trigger'; send = send'; isCaps = isCaps } 
     
    let TextFileToMappedKeys(text: string) =
        let seq = SplitToLines text
        let seq' = seq |> Seq.map RemoveComment |> Seq.map LineToMappedKey |> Seq.choose id 
        seq'  
 
    let CreateConfigByFile(path: string) =  
        let text = File.ReadAllText path 
        {mappedKeys= TextFileToMappedKeys text}
