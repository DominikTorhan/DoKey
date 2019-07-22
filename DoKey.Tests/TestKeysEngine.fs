module TestKeysEngine

open Xunit 

open DoKey.FS
open TrayApp2  
 
[<Theory>]  
[<InlineData("", "")>]
let TestStateNormal(expected:string, key:string) =     
    let appState = AppStatesFactory.AppStateNormal  
    let configuration = new Configuration()
    let eventData = new KeyEventData(key, KeyEventType.Down)
    let keysEngine = new cKeysEngine()
    keysEngine.Configuration <- configuration 
    keysEngine.AppState <- appState 
    keysEngine.KeyEventData <- eventData 
    let output = keysEngine.ProcessKey
    Assert.True(true)

//[<Theory>]  
//[<InlineData("", "")>]
//let Test(expected:string, key:string)=    
//    let x = new cKeysEngine();
//    let eventData = new KeyEventData("", KeyEventType.Down)
//    //let actual = InputKeyToStr(new InputKey(key))
//    //Assert.Equal (expected, actual) 
//    Assert.True(true)