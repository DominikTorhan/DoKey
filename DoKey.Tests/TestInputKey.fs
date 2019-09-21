module TestInputKey

open Xunit 
open DoKey.Core.Domain
open DoKey.Core.DomainOperations
 
let InputKeyToStr (i:InputKey) = 
        let x = if i.isModif then "m" else "" 
              + if i.isCaps then "c" else "" 
              + if i.isEsc then "e" else "" 
        x

[<Theory>]  
[<InlineData("", "")>]
[<InlineData("e", "escape")>]
[<InlineData("mce", "capital")>]
[<InlineData("", "a")>]
[<InlineData("m", "lmenu")>]
let Test(expected:string, key:string)=   
    let actual = InputKeyToStr(CreateInputKey key)
    Assert.Equal (expected, actual)