namespace DoKey.FS 
 

module Domain =   
  
    type State = Off = 0 | Normal = 1 | Insert = 2

    type MappedKey =
        { trigger: string
          send: string 
          isCaps: bool }
     
    type Modificators = 
        { alt:bool
          control:bool
          shift:bool
          win:bool
          caps:bool }
     
    type AppState = 
        { state : State
          modificators : Modificators
          firstStep : string
          preventEscOnCapsUp: bool}

    type KeysEngineResult =
        { appState: AppState
          send: string
          preventKeyProcess: bool }

    type Config =
        { sendKeys: List<MappedKey> }

    type Session =
        { config: Config 
          appState: AppState}
    

    let filePath = "Settings.txt"
    let modeChangeKey = "f"
    let modeOffKey = "q"
 
    //let IsTwoStep (key) = "qwertasdfgbui".Contains(key)