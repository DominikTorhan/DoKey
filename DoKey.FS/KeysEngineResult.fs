namespace DoKey.FS 
 
open DoKey.FS.Domain

type KeysEngineResult(appState:AppState, send:string, preventKeyProcess :bool) =   
    new (appState:AppState) = KeysEngineResult (appState, "", false)
    member this.appState= appState
    member this.send= send
    member this.preventKeyProcess = preventKeyProcess  
 
