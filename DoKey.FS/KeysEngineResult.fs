namespace DoKey.FS

type KeysEngineResult(appState:AppState, sendDoKey:SendDoKey, preventKeyProcess :bool) =   
    new (appState:AppState) = KeysEngineResult (appState, new SendDoKey(""), false)
    member this.appState= appState
    member this.sendDoKey= sendDoKey
    member this.preventKeyProcess = preventKeyProcess 
