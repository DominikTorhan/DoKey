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

    member this.Key = key.ToLower() 
    member this.IsCapital = GetIsCaps  this.Key
    member this.IsAlt = GetIsAlt this.Key
    member this.IsControl = GetIsControl this.Key
    member this.IsShift = GetIsShift this.Key
    member this.IsWin = GetIsWin this.Key
    member this.IsModifier = GetIsModifier this.Key
    member this.IsEsc = GetIsEsc this.Key 
