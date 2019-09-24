namespace DoKey.Core

module KeysEngine = 

    open Domain
    open DomainOperations

    let EmptyMappedKey = { send = ""; trigger = ""; isCaps = false } 

    let GetMappedKey isCaps (trigger:string) keys =
        let predicate (mappedKey:MappedKey) = mappedKey.trigger = trigger.ToLower() && mappedKey.isCaps = isCaps
        let result = Seq.tryFind predicate keys
        match result with
            | Some i -> i
            | None -> EmptyMappedKey 

    let IsDownFirstStep firstStep key =
        firstStep = "" && IsTwoStep key

    let GetMappedKeyNormal firstStep key keys =
        let isDownFirstStep = IsDownFirstStep firstStep key
        match isDownFirstStep with
            | true-> EmptyMappedKey 
            | _ -> GetMappedKey false (firstStep + key) keys 
    
    let GetMappedKeyNormalAndInsertWithCapital appState key keys =
        match appState.state with 
            | State.Off -> EmptyMappedKey
            | _ -> match appState.modificators.caps with
                   | false -> EmptyMappedKey
                   | _ -> GetMappedKey true (appState.firstStep + key) keys 
        
    let GetNextAppStateByStateChange key appState =
        let nextState = Domain.GetNextStateByKey key appState.state
        match nextState with
            | None -> None
            | _-> Some {state = nextState.Value; modificators = appState.modificators; firstStep=""; preventEscOnCapsUp = true}

    let GetNextAppStateByESC inputKey appState = 
        match inputKey.isEsc with
            | false -> None
            | _ -> match appState.state with
                    | State.Insert -> Some {state = GetPrevState appState.state; modificators = appState.modificators; 
                        firstStep=""; preventEscOnCapsUp = appState.preventEscOnCapsUp}
                    | _ -> None


    let CanProcessNormalMode appState =
        match appState.state with
            | State.Normal -> match appState.modificators.win with
                                | true -> false 
                                | _ -> true
            | _ -> false


    let ProcessNormalMode appState inputKey keys =
        let isDownFirstStep = appState .firstStep = "" && IsTwoStep inputKey.key
        let firstStepNext = if isDownFirstStep then inputKey.key else ""
        let sendDoKey = GetMappedKeyNormal appState.firstStep inputKey.key keys
        let preventKeyProcess = inputKey.isLetterOrDigit || sendDoKey.send <> ""
        let nextAppState = {state = appState.state; modificators = appState.modificators; firstStep = firstStepNext; preventEscOnCapsUp = appState.preventEscOnCapsUp}
        {appState = nextAppState ; send = sendDoKey.send ; preventKeyProcess = preventKeyProcess }
            
    let ProcessKey appState inputKey keys=
        if CanProcessNormalMode appState 
        then ProcessNormalMode appState inputKey keys
        else {appState = appState; send = ""; preventKeyProcess = false}

    //let ProcessNormalMode appState inputKey =
    //    match CanProcessNormalMode appState with
    //        | false -> ProcessKey
    //        | _ ->

    //private KeysEngineResult ProcessNormalMode()
        //{

        //  if (AppState.state != State.Normal) return null;
        //  if (AppState.modificators.win) return null;

        //  var isDownFirstStep = AppState.firstStep == "" && DomainOperations.IsTwoStep(inputKey.key);

        //  var firstStepNext = isDownFirstStep ? inputKey.key : "";

        //  var sendDoKey = DoKey.Core.KeysEngine.GetMappedKeyNormal(AppState.firstStep, inputKey.key.ToString(), config.mappedKeys);

        //  var preventKeyProcess = inputKey.isLetterOrDigit || sendDoKey.send != "";

        //  return new KeysEngineResult(new AppState(this.AppState.state, this.AppState.modificators, firstStepNext, this.AppState.preventEscOnCapsUp), sendDoKey.send, preventKeyProcess);

        //}
