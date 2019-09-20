namespace DoKey.Core

module KeysOperations =
    
    let isCaps s = s = "capital"   

    let isAlt s = 
        match s with
            | "lmenu" -> true 
            | "rmenu" -> true 
            | _ -> false 

    let isShift s = 
        match s with
            | "lshiftkey" -> true 
            | "rshiftkey" -> true 
            | "shiftkey" -> true 
            | _ -> false
  
    let isControl s = 
        match s with
            | "controlkey" -> true 
            | "lcontrolkey" -> true 
            | "rcontrolkey" -> true 
            | _ -> false
             
    let isWin s = 
        match s with
            | "lwin" -> true
            | "rwin" -> true
            | _ -> false 
 
    let isModifier s =
        match s with
            | x when isAlt x -> true 
            | x when isShift x -> true 
            | x when isControl x -> true 
            | x when isWin x -> true 
            | x when isCaps x -> true 
            | _ -> false 

    let isEsc s = s = "escape" || isCaps s
     
    let isLetterOrDigit s = 
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
 


