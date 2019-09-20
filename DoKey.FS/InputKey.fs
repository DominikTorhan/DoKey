namespace DoKey.Core
 
open DoKey.Core.KeysOperations

type InputKey(key:string) =  

    member this.Key = key.ToLower() 
    member this.IsCapital = isCaps  this.Key
    member this.IsAlt = isAlt this.Key
    member this.IsControl = isControl this.Key
    member this.IsShift = isShift this.Key
    member this.IsWin = isWin this.Key
    member this.IsModifier = isModifier this.Key
    member this.IsEsc = isEsc this.Key  
    member this.IsLetterOrDigit = isLetterOrDigit this.Key
