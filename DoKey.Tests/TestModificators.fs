module TestModificators

open Xunit 
open DoKey.FS

[<Theory>]  
[<InlineData("%", "lmenu", false, "")>]
[<InlineData("%^", "rmenu", false, "^")>]
[<InlineData("%^+wc", "lmenu", false, "^+wc")>]
[<InlineData("%", "lmenu", false, "%")>]
[<InlineData("", "lmenu", true, "%")>]
[<InlineData("%^", "controlkey", false, "%")>]
[<InlineData("%", "controlkey", true, "%^")>]
[<InlineData("+", "lshiftkey", false, "")>]
[<InlineData("w", "lwin", false, "")>]
[<InlineData("c", "capital", false, "")>]
let TestNext(expected:string, key:string, isUp:bool, modifStr:string) =
    let input = new InputKey (key) 
    let modif = new Modificators(modifStr) 
    let modif' = modif.GetNextModificators(input, isUp)
    Assert.Equal(expected, modif'.ToStr)




