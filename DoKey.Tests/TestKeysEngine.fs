module TestKeysEngine

open Xunit 

open DoKey.FS
open TrayApp2  


let ProcessKey(key:string, modif:string, state:State) = 
    let modificators = new Modificators(modif)
    let appState = AppState(state, modificators, "", false)
    let configuration = new Configuration()
    let eventData = new KeyEventData(key, KeyEventType.Down)
    let keysEngine = new cKeysEngine()
    keysEngine.Configuration <- configuration 
    keysEngine.AppState <- appState 
    keysEngine.KeyEventData <- eventData 
    let output = keysEngine.ProcessKey()
    output
  
[<Theory>]  
[<InlineData(State.Off, "", "", State.Off)>]
[<InlineData(State.Normal, "", "", State.Normal)>]
[<InlineData(State.Normal, "f", "c", State.Off)>]
[<InlineData(State.Insert, "f", "c", State.Normal)>]
[<InlineData(State.Off, "q", "c", State.Normal)>]
[<InlineData(State.Off, "q", "c", State.Insert)>]
let TestChageState(expected:State, key:string, modif:string, state:State)=
    let output = ProcessKey(key, modif, state)
    Assert.Equal(expected, output.AppState.State)

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
    let str = ProcessKey(key, "", State.Normal).GetStr()
    Assert.Equal(expected, str)

 
 //normal with modif
[<Theory>]  
[<InlineData("(c)", "f", "c")>]
let TestStateNormalModif(expected:string, key:string, modif:string) =      
    let str = ProcessKey(key, modif, State.Normal).GetStr()
    Assert.Equal(expected, str)

 
//insert 
[<Theory>]  
[<InlineData("{GO}", "f")>]
let TestStateInsert(expected:string, key:string) =      
    let str = ProcessKey(key, "", State.Insert).GetStr()
    Assert.Equal(expected, str)
    
//insert with modif
[<Theory>]   
//caps - independent from state
[<InlineData("(c)", "f", "c")>]
[<InlineData("{DEL}(c)", "d", "c")>] 
[<InlineData("{ESC}(c)", "e", "c")>]
[<InlineData("{BKSP}(c)", "h", "c")>]
[<InlineData("{DEL}(c)", "l", "c")>]
[<InlineData("{DEL}(c)", "d", "c")>]
[<InlineData("{ENTER}(c)", "j", "c")>]
[<InlineData("{ENTER}(c)", "r", "c")>]
[<InlineData("{TAB}(c)", "t", "c")>]
[<InlineData("+{F10}(c)", "c", "c")>]
[<InlineData("^+v(c)", "v", "c")>]


let TestStateInsertModif(expected:string, key:string, modif:string) =      
    let str = ProcessKey(key, modif, State.Insert).GetStr()
    Assert.Equal(expected, str)

[<Theory>]   
[<InlineData("{GO}(%c)", "lmenu", "c")>] 
[<InlineData("{GO}(%)", "lmenu", "")>]
[<InlineData("{GO}(%^)", "rmenu", "^")>]
[<InlineData("{GO}(%^+wc)", "lmenu", "^+wc")>]
[<InlineData("{GO}(%)", "lmenu", "%")>]
[<InlineData("{GO}(%^)", "controlkey", "%")>]
[<InlineData("{GO}(+)", "lshiftkey", "")>]
[<InlineData("{GO}(w)", "lwin", "")>]
[<InlineData("(c)", "capital", "")>]
[<InlineData("(%c)", "capital", "%")>]
let TestModif(expected:string, key:string, modif:string)=
    let str = ProcessKey(key, modif, State.Insert).GetStr()
    Assert.Equal(expected, str)
  