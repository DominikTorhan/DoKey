namespace DoKey.Core

module DomainOperations =
      
    open Domain
    open KeysOperations
  
    let CreateInputKey (stra:string)=
        let str = stra.ToLower()
        {key = str; isAlt = isAlt str; isCaps = isCaps str; isControl = isControl str; isShift = isShift str; isWin = isWin str; 
        isLetterOrDigit = isLetterOrDigit str; isModif = isModifier str; isEsc = isEsc str}

    let CreateMoficators = 
        {alt = false; control = false;shift=false;win=false;caps=false }

    let CreateAppState =
        {state=State.Off; modificators = CreateMoficators; firstStep = ""; preventEscOnCapsUp = false }

    let CreateSession =   
        let config = ConfigFileReader.CreateConfigByFile filePathNew
        {config = config; appState = CreateAppState }


    let IsTwoStep (key) = twoStep.Contains(key)
  

    let CreateEngineResultChangeAppState(appState:AppState)=
        {appState = appState; send =""; preventKeyProcess= true}