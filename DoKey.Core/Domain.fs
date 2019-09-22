namespace DoKey.Core 
 

module Domain =   
  
    type State = Off = 0 | Normal = 1 | Insert = 2
    type KeyEventType = Down|Up

    type InputKey = 
      { key :string
        isCaps: bool
        isAlt:bool
        isControl:bool
        isShift:bool
        isWin:bool
        isModif:bool
        isEsc:bool
        isLetterOrDigit:bool }

    type KeyEventData =
      { inputKey:InputKey
        keyEventType:KeyEventType }

    type MappedKey =
        { trigger: string
          send: string 
          isCaps: bool }
     
    type CommandKey =
        { trigger: string
          run: string}

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
        { mappedKeys: seq<MappedKey> 
          commandKeys: seq<CommandKey> }

    type Session =
        { config: Config 
          appState: AppState}
    

    let filePathNew = "config.txt"
    let modeChangeKey = "f"
    let modeOffKey = "q" 
    let twoStep = "qwertasdfgbui"
 
    //let IsTwoStep (key) = "qwertasdfgbui".Contains(key)