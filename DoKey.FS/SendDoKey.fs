namespace DoKey.FS
  
open DoKey.FS.Domain

module DoKeyModule = 
 
    let GetList = 
         [   
            { send = "{ESC}"; trigger = "e"; isCaps = true } 
            { send = "{BKSP}"; trigger = "h"; isCaps = true } 
            { send = "{DEL}"; trigger = "l"; isCaps = true } 
            { send = "{DEL}"; trigger = "d"; isCaps = true } 
            { send = "{ENTER}"; trigger = "j"; isCaps = true } 
            { send = "{ENTER}"; trigger = "r"; isCaps = true } 
            { send = "{TAB}"; trigger = "t"; isCaps = true } 
            { send = "+{TAB}"; trigger = "g"; isCaps = true } 
            { send = "+{F10}"; trigger = "c"; isCaps = true } 
            { send = "^+v"; trigger = "v"; isCaps = true } 
            { send = "^{TAB}"; trigger = "oemperiod"; isCaps = true } 
            { send = "^+{TAB}"; trigger = "n"; isCaps = true } 
            ] 
 
    let GetSendKey (keysList:list<MappedKey>, key:string, predicate:(MappedKey -> bool)) =
        let result = List.tryFind predicate keysList 
        match result with
            | Some i -> i
            | None -> { send = ""; trigger = ""; isCaps = false } 
 
    let GetPredicate (isCaps:bool, key:string) =
        let predicate (sendKey:MappedKey) = sendKey.trigger = key.ToLower() && sendKey.isCaps = isCaps
        predicate

    let GetSendKeyNormal (keysList:list<MappedKey>, key:string) = 
        GetSendKey (keysList, key, GetPredicate(false, key))

    let GetSendKeyCaps (keysList:list<MappedKey>, key:string) = 
        GetSendKey (keysList, key, GetPredicate(true, key))
 
    let CreateSendDoKeyByStr (str:string) =
        //Insert line below: ij: {End} {ENTER} 
        let split = str.Split(':')  
        match split.Length with 
            | 3 -> { send = split.[2].Trim(); trigger = split.[1].Trim(); isCaps = false } 
            | _ -> { send = ""; trigger = ""; isCaps = false } 

        

