namespace DoKey.FS 
 

module Domain =   
  
    type State = Off = 0 | Normal = 1 | Insert = 2

    type SendKey =
        { trigger: string
          send: string 
          isCaps: bool }
     
    //type KeysEngineResult =
    //    { appState: AppState
    //      send: string
    //      preventKeyProcess: bool }

    //type AppState = 
    //    { state: State
    //      modificators: Modificators 
    //      firstStep: string
    //      preventEscOnCapsUp: bool
    //    }

    type Config =
        { sendKeys: List<SendKey> }

    type Session =
        { config: Config }
    

