module TestKeysEngine

open Xunit 

open DoKey.Core
open DoKey.Core.Domain
open DoKey.Core.DomainOperations
open DoKey.Core.ModificatorsOperations
open DoKey
 
let GetStr(keysEngineResult : KeysEngineResult) =  
     let str = keysEngineResult.send 
     let modif = ModificatorsToStr keysEngineResult.appState.modificators
     let str' = if keysEngineResult.preventKeyProcess then str else str + "{GO}" 
     let str'' = match modif with | "" -> str' | _ -> str' + "(" + modif + ")"
     str''


let ProcessKey(key:string, modif:string, state:State, firstStep:string) = 
    let modificators = CreateModificatorsByStr modif 
    let session = CreateSession
    let appState = { state = state; modificators = modificators; firstStep = firstStep; preventEscOnCapsUp = false}
    let inputKey = CreateInputKey key
    let keysEngine = new KeysEngine()
    keysEngine.config <- session.config 
    keysEngine.AppStateX <- appState 
    keysEngine.inputKey <- inputKey 
    keysEngine.isUp <- false
    let output = keysEngine.ProcessKey()
    output
  
[<Theory>]  
//[<InlineData(State.Off, "", "", State.Off)>]
//[<InlineData(State.Normal, "", "", State.Normal)>]
//[<InlineData(State.Normal, "f", "c", State.Off)>]
//[<InlineData(State.Insert, "f", "c", State.Normal)>]
[<InlineData(State.Off, "q", "c", State.Normal)>]
//[<InlineData(State.Off, "q", "c", State.Insert)>]
//[<InlineData(State.Normal, "escape", "", State.Insert)>]
let TestChageState(expected:State, key:string, modif:string, state:State)=
    let output = ProcessKey(key, modif, state, "")
    Assert.Equal(expected, output.appState.state)

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
[<InlineData("^x", "x")>]
[<InlineData("^c", "c")>]
[<InlineData("^v", "v")>]
[<InlineData("^z", "z")>]
//first step 
[<InlineData("", "q")>]
[<InlineData("", "w")>]
[<InlineData("", "e")>]
[<InlineData("", "r")>]
[<InlineData("", "t")>] 
[<InlineData("", "a")>]
[<InlineData("", "s")>]
[<InlineData("", "d")>]
[<InlineData("", "f")>]
[<InlineData("", "g")>]
[<InlineData("", "b")>]
[<InlineData("", "u")>]
[<InlineData("", "i")>]
let TestStateNormal(expected:string, key:string) =      
    let str = ProcessKey(key, "", State.Normal, "") |> GetStr
    Assert.Equal(expected, str)

 
 //normal with modif
[<Theory>]  
[<InlineData("(c)", "f", "c")>]
[<InlineData("{HOME}(%)", "n", "%")>]
[<InlineData("{LEFT}(%)", "h", "%")>]
[<InlineData("{DOWN}(%)", "j", "%")>]
[<InlineData("{UP}(%)", "k", "%")>]
[<InlineData("{RIGHT}(%)", "l", "%")>] 
[<InlineData("{LEFT}(+)", "h", "+")>]
[<InlineData("{DOWN}(+)", "j", "+")>]
[<InlineData("{UP}(+)", "k", "+")>]
[<InlineData("{RIGHT}(+)", "l", "+")>]
[<InlineData("{LEFT}(^)", "h", "^")>]
[<InlineData("{DOWN}(^)", "j", "^")>]
[<InlineData("{UP}(^)", "k", "^")>]
[<InlineData("{RIGHT}(^)", "l", "^")>]
[<InlineData("{LEFT}(%+)", "h", "%+")>]
[<InlineData("{DOWN}(%+)", "j", "%+")>]
[<InlineData("{UP}(%+)", "k", "%+")>]
[<InlineData("{RIGHT}(%+)", "l", "%+")>] 
let TestStateNormalModif(expected:string, key:string, modif:string) =      
    let str = ProcessKey(key, modif, State.Normal, "") |> GetStr
    Assert.Equal(expected, str)

 
//insert 
[<Theory>]  
[<InlineData("{GO}", "f")>]
let TestStateInsert(expected:string, key:string) =      
    let str = ProcessKey(key, "", State.Insert, "") |> GetStr
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
    let str = ProcessKey(key, modif, State.Insert, "") |> GetStr
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
    let str = ProcessKey(key, modif, State.Insert, "") |> GetStr
    Assert.Equal(expected, str)
   
[<Theory>]   
[<InlineData("{F1}", "f", "d1")>] 
[<InlineData("{F2}", "f", "d2")>] 
[<InlineData("{F3}", "f", "d3")>] 
[<InlineData("{F4}", "f", "d4")>] 
[<InlineData("{F5}", "f", "d5")>] 
[<InlineData("{F6}", "f", "d6")>] 
[<InlineData("{F7}", "f", "q")>] 
[<InlineData("{F8}", "f", "w")>] 
[<InlineData("{F9}", "f", "e")>] 
[<InlineData("{F10}", "f", "r")>] 
[<InlineData("{F11}", "f", "t")>] 
[<InlineData("{F12}", "f", "f")>] 
[<InlineData("^q", "e", "q")>] 
[<InlineData("^s", "e", "s")>] 
[<InlineData("^,", "e", "oemcomma")>] 
[<InlineData("^.", "e", "oemperiod")>] 
[<InlineData("^/", "e", "oem2")>] 
[<InlineData("^-", "e", "oemminus")>] 
[<InlineData("^=", "e", "oemplus")>] 
[<InlineData("^]", "e", "oem6")>] 
[<InlineData("^\\", "e", "oem5")>] 
[<InlineData("^;", "e", "oem1")>] 
[<InlineData("^'", "e", "oem7")>] 
[<InlineData("^{F4}", "e", "d4")>] 
[<InlineData("^[", "e", "oemopenbrackets")>] 
let TestSecondStep(expected:string, firstStep:string, key:string)=
    let str = ProcessKey(key, "", State.Normal, firstStep) |> GetStr
    Assert.Equal(expected, str)
    