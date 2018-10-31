namespace DoKey.FS

type InputKey(key:string) =  

    let GetIsCaps s = s = "capital"   

    let GetIsAlt s = 
        match s with
            | "lmenu" -> true 
            | "rmenu" -> true 
            | _ -> false 

    let GetIsShift s = 
        match s with
            | "lshiftkey" -> true 
            | "rshiftkey" -> true 
            | "shiftkey" -> true 
            | _ -> false
  
    let GetIsControl s = 
        match s with
            | "controlkey" -> true 
            | "lcontrolkey" -> true 
            | "rcontrolkey" -> true 
            | _ -> false
             
    let GetIsWin s = 
        match s with
            | "lwin" -> true
            | "rwin" -> true
            | _ -> false 
 
    let GetIsModifier s =
        match s with
            | x when GetIsAlt x -> true 
            | x when GetIsShift x -> true 
            | x when GetIsControl x -> true 
            | x when GetIsWin x -> true 
            | x when GetIsCaps x -> true 
            | _ -> false 

    let GetIsEsc s = s = "escape" || GetIsCaps s
     
    let GetIsLetterOrDigit s = 
        match s with
            | "q" -> true
            | "w" -> true
            | "e" -> true
            | "r" -> true
            | "t" -> true
            | "y" -> true
            | "u" -> true
            | "i" -> true
            | "o" -> true
            | "p" -> true
            | "a" -> true
            | "s" -> true
            | "d" -> true
            | "f" -> true
            | "g" -> true
            | "h" -> true
            | "j" -> true
            | "k" -> true
            | "l" -> true
            | "z" -> true
            | "x" -> true
            | "c" -> true
            | "v" -> true
            | "b" -> true
            | "n" -> true
            | "m" -> true
            | "d1" -> true
            | "d2" -> true
            | "d3" -> true
            | "d4" -> true
            | "d5" -> true
            | "d6" -> true
            | "d7" -> true
            | "d8" -> true
            | "d9" -> true
            | "d0" -> true
            | "f1" -> false
            | "f2" -> false
            | "f3" -> false
            | "f4" -> false
            | "f5" -> false
            | "f6" -> false
            | "f7" -> false
            | "f8" -> false
            | "f9" -> false
            | "f10" -> false
            | "f11" -> false
            | "f12" -> false
            | "oemcomma" -> true
            | "oemperiod" -> true
            | "oemquestion" -> true
            | _ -> false
 
    member this.Key = key.ToLower() 
    member this.IsCapital = GetIsCaps  this.Key
    member this.IsAlt = GetIsAlt this.Key
    member this.IsControl = GetIsControl this.Key
    member this.IsShift = GetIsShift this.Key
    member this.IsWin = GetIsWin this.Key
    member this.IsModifier = GetIsModifier this.Key
    member this.IsEsc = GetIsEsc this.Key  
    member this.IsLetterOrDigit = GetIsLetterOrDigit this.Key
