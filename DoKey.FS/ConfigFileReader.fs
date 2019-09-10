namespace DoKey.FS

module ConfigFileReader =
    
    open System  
    open Domain
     
    let RemoveComment(line: string) =
        let idx = line.IndexOf("//")
        match idx with  
            | -1 -> line
            | _ -> line.Replace(line.Substring(idx), "") 
    
    let SplitToLines(str: string) =
        let seq = str.Split([|"\n"|], StringSplitOptions.RemoveEmptyEntries) |> Array.toSeq   
        seq  
      
    let LineToSendKey(line : string) =
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
        match trigger with
            | "" -> None
            | _ -> Some {trigger = trigger'; send = send; isCaps = isCaps } 
     
    let TextFileToKeys(text: string) =
        let seq = SplitToLines text
        let seq' = seq |> Seq.map RemoveComment |> Seq.map LineToSendKey |> Seq.choose id 
        seq' 
