namespace DoKey.Core

module KeysEngine = 

    open Domain

    let GetSendKey (keysList:seq<MappedKey>, predicate:(MappedKey -> bool)) =
        let result = Seq.tryFind predicate keysList 
        match result with
            | Some i -> i
            | None -> { send = ""; trigger = ""; isCaps = false } 
   
    let GetPredicate (isCaps:bool, trigger:string) =
        let predicate (sendKey:MappedKey) = sendKey.trigger = trigger.ToLower() && sendKey.isCaps = isCaps
        predicate
  
    //let GetSendKeyNormal (keysList:seq<MappedKey>, key:string) = 
    //    GetSendKey (keysList, GetPredicate(false, key))
  
    //let GetSendKeyCaps (keysList:seq<MappedKey>, key:string) = 
    //    GetSendKey (keysList, GetPredicate(true, key))

    let GetMappedKeyNormal keys trigger = GetSendKey (keys, GetPredicate(false, trigger))
    let GetMappedKeyCaps keys trigger = GetSendKey (keys, GetPredicate(true, trigger))

    let GetMappedKey isDownFirstStep keys =
        match isDownFirstStep with
            | false -> { send = ""; trigger = ""; isCaps = false } 
            | true -> GetMappedKeyNormal keys ""
        

    
    //let ProcessModificators =
    //    ModificatorsOperations.GetNextModificators

    //private KeysEngineResult ProcessModificators()
    //{

    //  //var modificators = this.AppState.Modificators.GetNextModificators(inputKey, isUp);
    //  var modificators = ModificatorsOperations.GetNextModificators(this.AppState.modificators, inputKey, isUp);

    //  return new KeysEngineResult(new AppState(this.AppState.state, modificators, this.AppState.firstStep, this.AppState.preventEscOnCapsUp), "", false);

    //}


//let Perform(appState:AppState, inputKey: InputKey, configuration: Configuration) =
//    appState


//let PerformModificators(appState:AppState) =
//    let modif = appState.Modificators.GetNextModificators(null, "")
//    let appState' = {appState with modificators = modif }
//    appState'

    //private cOutput ProcessModificators()
    //{

    //  var modificators = this.modificators.GetNextModificators(inputKey, isUp);

    //  return new cOutput
    //  {
    //    AppState = new AppState(this.AppState.State, modificators, this.AppState.FirstStep, this.AppState.PreventEscOnCapsUp)
    //  };

    //}

    //private MappedKey GetSendDoKey(bool isDownFirstStep)
    //{

    //  if (isDownFirstStep) return new MappedKey("", "", false);

    //  var trigger = AppState.firstStep + keys.ToString();

    //  var doKey = DomainOperations.GetMappedKeyNormal(config.mappedKeys, trigger);

    //  return doKey;

    //}
