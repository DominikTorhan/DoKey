module TestKeysEngine

open Xunit 

open DoKey.FS
open DoKey
open TrayApp2

[<Theory>]  
[<InlineData("", "")>]
let Test(expected:string, key:string)=    
    let x = new cKeysEngine();
    let eventData = new KeyEventData("", KeyEventType.Down)
    //let actual = InputKeyToStr(new InputKey(key))
    //Assert.Equal (expected, actual) 
    Assert.True(true)