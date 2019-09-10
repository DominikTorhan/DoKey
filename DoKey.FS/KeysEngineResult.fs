namespace DoKey.FS 
 
open DoKey.FS.Domain

type KeysEngineResult(appState:AppState, sendDoKey:SendKey, preventKeyProcess :bool) =   
    new (appState:AppState) = KeysEngineResult (appState, {send=""; trigger = ""; isCaps= false}, false)
    member this.appState= appState
    member this.sendKey= sendDoKey
    member this.preventKeyProcess = preventKeyProcess  
 
    member this.GetStr() =  
        let str = this.sendKey.send 
        let modif = this.appState.Modificators.ToStr 
        let str' = if preventKeyProcess then str else str + "{GO}" 
        let str'' = match modif with | "" -> str' | _ -> str' + "(" + modif + ")"
        str''
    
 //   state.ToString() + " " + modificators.ToLog + " " + firstStep
 
 //public string GetStr()
 //{
 //  string pStr = sendDoKey?.Send ?? "";
 //  string pModif = AppState.Modificators.ToStr;
 //  if (!PreventKeyProcess) pStr += "{GO}";
 //  if (pModif != "") pStr += "(" + AppState.Modificators.ToStr + ")";
 //  return pStr;
 //}
