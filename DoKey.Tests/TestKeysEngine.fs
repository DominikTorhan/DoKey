module TestKeysEngine

open Xunit 

open DoKey.FS
open TrayApp2  


let ProcessKey(key:string, modif:string) = 
    let modificators = new Modificators(modif)
    let appState = AppState(State.Normal, modificators, "", false)
    let configuration = new Configuration()
    let eventData = new KeyEventData(key, KeyEventType.Down)
    let keysEngine = new cKeysEngine()
    keysEngine.Configuration <- configuration 
    keysEngine.AppState <- appState 
    keysEngine.KeyEventData <- eventData 
    let output = keysEngine.ProcessKey()
    output.GetStr()
 
[<Theory>]  
//[<InlineData("", "")>]
//simple keys
[<InlineData("{LEFT}", "h")>]
[<InlineData("{DOWN}", "j")>]
[<InlineData("{UP}", "k")>]
[<InlineData("{RIGHT}", "l")>]
[<InlineData("{HOME}", "n")>]
[<InlineData("{PGDN}", "m")>]
[<InlineData("{PGUP}", "oemcomma")>]//,
[<InlineData("{END}", "oemperiod")>]//.
[<InlineData("^{LEFT}", "y")>]
[<InlineData("^{RIGHT}", "o")>]
[<InlineData("^a", "a")>]
[<InlineData("^w", "w")>]
[<InlineData("^s", "s")>]
[<InlineData("^x", "x")>]
[<InlineData("^c", "c")>]
[<InlineData("^v", "v")>]
[<InlineData("^z", "z")>]
[<InlineData("^y", "r")>]
//first step 
[<InlineData("", "f")>]
[<InlineData("", "d")>]
[<InlineData("", "u")>]
[<InlineData("", "i")>]
[<InlineData("", "e")>]


let TestStateNormal(expected:string, key:string) =      
    let str = ProcessKey(key, "")
    Assert.Equal(expected, str)


[<Theory>]  
[<InlineData("", "f", "c")>]
let TestStateNormalModif(expected:string, key:string, modif:string) =      
    let str = ProcessKey(key, modif)
    Assert.Equal(expected, str)

//[<Theory>]  
//[<InlineData("", "")>]
//let Test(expected:string, key:string)=    
//    let x = new cKeysEngine();
//    let eventData = new KeyEventData("", KeyEventType.Down)
//    //let actual = InputKeyToStr(new InputKey(key))
//    //Assert.Equal (expected, actual) 
//    Assert.True(true)