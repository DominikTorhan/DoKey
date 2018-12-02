namespace DoKey.FS
 
type KeyType =
    | Normal
    //| NormalTwoStep
    | Caps 

type SendDoKey(send: string, name:string, keyType:KeyType, trigger:string, modAllowed:string)=   
    new (send, name, keyType, trigger) = SendDoKey(send, name, keyType, trigger, "")
    new (send) = SendDoKey(send, "", KeyType.Normal, "", "")
    member this.Send = send  
    member this.Name = name
    member this.KeyType = keyType
    member this.Trigger = trigger 
    member this.IsAlt = send.Contains("%") 
    member this.IsControl = send.Contains("^") 
    member this.IsEmpty = send = "" 
    member this.ToLog = name + " " + send
    member this.IsAltAllowed = modAllowed.Contains("%");
    member this.IsShiftAllowed = modAllowed.Contains("+");
 
 type KeysList(keyList:list<SendDoKey>) = 
    member this.KeyList = keyList
         
 

 module DoKeyModule = 

    let GetList = 
        let DoKeyList = [ 
            SendDoKey("{LEFT}", "Left", KeyType.Normal, "h", "+%");
            SendDoKey("{DOWN}", "Down", KeyType.Normal, "j", "+%");
            SendDoKey("{UP}", "Up", KeyType.Normal, "k", "+%");
            SendDoKey("{RIGHT}", "Right", KeyType.Normal, "l", "+%");
            SendDoKey("{HOME}", "Home", KeyType.Normal, "n", "+%");
            SendDoKey("{PGDN}", "PageDown", KeyType.Normal, "m", "+");
            SendDoKey("{PGUP}", "PageUp", KeyType.Normal, "oemcomma", "+");
            SendDoKey("{END}", "End", KeyType.Normal, "oemperiod", "+%"); 
            //SendDoKey("%{LEFT}", "AltLeft", KeyType.Normal, "%h");//chyba do wywalenia
            //SendDoKey("%{DOWN}", "AltDown", KeyType.Normal, "%j");
            //SendDoKey("%{UP}", "AltUp", KeyType.Normal, "%k");
            //SendDoKey("%{RIGHT}", "AltRight", KeyType.Normal, "%l");
            //SendDoKey("%{HOME}", "AltHome", KeyType.Normal, "%n");
            //SendDoKey("%{PGDN}", "AltPageDown", KeyType.Normal, "%m");
            //SendDoKey("%{PGUP}", "AltPageUp", KeyType.Normal, "%oemcomma");
            //SendDoKey("%{END}", "AltEnd", KeyType.Normal, "%oemperiod"); 
            SendDoKey("^{LEFT}", "CtrlLeft", KeyType.Normal, "y", "+");
            SendDoKey("^{RIGHT}", "CtrlRight", KeyType.Normal, "o", "+");
            SendDoKey("^{DOWN}", "CtrlDown", KeyType.Normal, "b");
            SendDoKey("^{UP}", "CtrlUp", KeyType.Normal, "g");

            SendDoKey("^x", "CtrlX", KeyType.Normal, "x");
            SendDoKey("^c", "CtrlC", KeyType.Normal, "c");
            SendDoKey("^v", "CtrlV", KeyType.Normal, "v");
            SendDoKey("^z", "CtrlZ", KeyType.Normal, "z");
            SendDoKey("^y", "CtrlY", KeyType.Normal, "r");
 
            SendDoKey("{ESC}", "Esc", KeyType.Caps, "e");
            SendDoKey("{BKSP}", "Backspace", KeyType.Caps, "h");
            SendDoKey("{DEL}", "Delete", KeyType.Caps, "l");
            SendDoKey("{ENTER}", "Enter", KeyType.Caps, "j");
            SendDoKey("{TAB}", "Tab", KeyType.Caps, "t");

            SendDoKey("{F1}", "F1", KeyType.Normal, "fd1");
            SendDoKey("{F2}", "F2", KeyType.Normal, "fd2");
            SendDoKey("{F3}", "F3", KeyType.Normal, "fd3");
            SendDoKey("{F4}", "F4", KeyType.Normal, "fd4");
            SendDoKey("{F5}", "F5", KeyType.Normal, "fd5");
            SendDoKey("{F6}", "F6", KeyType.Normal, "fd6");
            SendDoKey("{F7}", "F7", KeyType.Normal, "fq");
            SendDoKey("{F8}", "F8", KeyType.Normal, "fw");
            SendDoKey("{F9}", "F9", KeyType.Normal, "fe");
            SendDoKey("{F10}", "F10", KeyType.Normal, "fr"); 
            SendDoKey("{F11}", "F11", KeyType.Normal, "ft");
            SendDoKey("{F12}", "F12", KeyType.Normal, "ff");

            //SendDoKey("{End} {ENTER}", "Insert line below", KeyType.Normal, "ij");
            //SendDoKey("{UP}{End}{ENTER}", "Insert line above", KeyType.Normal, "ik");
            //SendDoKey("^m^m", "VS ^m^m toggle outline", KeyType.Normal, "uu");
            //SendDoKey("^m^o", "VS ^m^o collapse to definition", KeyType.Normal, "uo");
            //SendDoKey("^+{F12}", "VS ^F12 go to next error", KeyType.Normal, "ue"); 
            //SendDoKey("^k^c", "VS ^k^c comment selection", KeyType.Normal, "uk"); 

            ] 
        DoKeyList
               //Seq.toList DoKeyList
        //ResizeArray<SendDoKey> DoKeyList
        //CSList 
 
    let CreateKeysList = 
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

        
