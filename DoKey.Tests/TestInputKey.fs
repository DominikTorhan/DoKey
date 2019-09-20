module TestInputKey

open Xunit 
open DoKey.Core
 
let InputKeyToStr (i:InputKey) = 
        let x = if i.IsModifier then "m" else "" 
              + if i.IsCapital then "c" else "" 
              + if i.IsEsc then "e" else "" 
        x

[<Theory>]  
[<InlineData("", "")>]
[<InlineData("e", "escape")>]
[<InlineData("mce", "capital")>]
[<InlineData("", "a")>]
[<InlineData("m", "lmenu")>]
let Test(expected:string, key:string)=   
    let actual = InputKeyToStr(new InputKey(key))
    Assert.Equal (expected, actual)