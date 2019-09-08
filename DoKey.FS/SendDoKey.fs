namespace DoKey.FS
 
type KeyType =
    | Normal
    | Caps 

type SendDoKey(send: string, name:string, keyType:KeyType, trigger:string)=   
    new (send) = SendDoKey(send, "", KeyType.Normal, "")
    member this.Send = send  
    member this.Name = name
    member this.KeyType = keyType
    member this.Trigger = trigger 
    member this.IsEmpty = send = "" 
    member this.ToLog = name + " " + send
 
 type KeysList(keyList:list<SendDoKey>) = 
    member this.KeyList = keyList
         
 

 module DoKeyModule = 
 
    let GetList = 
        let DoKeyList = [ 
            SendDoKey("{ESC}", "Esc", KeyType.Caps, "e");
            SendDoKey("{BKSP}", "Backspace", KeyType.Caps, "h");
            SendDoKey("{DEL}", "Delete", KeyType.Caps, "l");
            SendDoKey("{DEL}", "Delete LeftHand", KeyType.Caps, "d");
            SendDoKey("{ENTER}", "Enter", KeyType.Caps, "j");
            SendDoKey("{ENTER}", "Enter LeftHand", KeyType.Caps, "r");
            SendDoKey("{TAB}", "Tab", KeyType.Caps, "t");
            SendDoKey("+{TAB}", "ShiftTab", KeyType.Caps, "g");
            SendDoKey("+{F10}", "ContextMenu+F10", KeyType.Caps, "c");
            SendDoKey("^+v", "CtrlShift+V", KeyType.Caps, "v");
            SendDoKey("^{TAB}", "CtrlTab+V", KeyType.Caps, "oemperiod");
            SendDoKey("^+{TAB}", "CtrlShiftTab", KeyType.Caps, "n");
            ] 
        DoKeyList
               //Seq.toList DoKeyList
        //ResizeArray<SendDoKey> DoKeyList
        //CSList 
 
    let bCreateKeysList = 
        let x = new KeysList(GetList) 
        x 

    let GetSendKey (keysList:list<SendDoKey>, key:string, predicate:(SendDoKey -> bool)) =
        let result = List.tryFind predicate keysList 
        match result with
            | Some i -> i
            | None -> new SendDoKey("")
 
    let GetPredicate (keyType:KeyType, key:string) =
        let predicate (sendKey:SendDoKey) = sendKey.Trigger = key.ToLower() && sendKey.KeyType = keyType
        predicate

    let GetSendKeyNormal (keysList:list<SendDoKey>, key:string) = 
        GetSendKey (keysList, key, GetPredicate(KeyType.Normal, key))

    let GetSendKeyCaps (keysList:list<SendDoKey>, key:string) = 
        GetSendKey (keysList, key, GetPredicate(KeyType.Caps, key))
 
    let CreateSendDoKeyByStr (str:string) =
        //Insert line below: ij: {End} {ENTER} 
        let split = str.Split(':')  
        match split.Length with 
            | 3 -> new SendDoKey(split.[2].Trim(), split.[0], KeyType.Normal, split.[1].Trim()) 
            | _ -> new SendDoKey("")

        
