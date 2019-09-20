namespace DoKey.Core

module DomainOperations =
      
    open Domain
  
    let CreateMoficators = 
        {alt = false; control = false;shift=false;win=false;caps=false }

    let CreateAppState =
        {state=State.Off; modificators = CreateMoficators; firstStep = ""; preventEscOnCapsUp = false }

    let CreateSession =   
        let config = ConfigFileReader.CreateConfigByFile filePathNew
        {config = config; appState = CreateAppState }


    let IsTwoStep (key) = twoStep.Contains(key)
  
    let GetSendKey (keysList:seq<MappedKey>, key:string, predicate:(MappedKey -> bool)) =
        let result = Seq.tryFind predicate keysList 
        match result with
            | Some i -> i
            | None -> { send = ""; trigger = ""; isCaps = false } 
   
    let GetPredicate (isCaps:bool, key:string) =
        let predicate (sendKey:MappedKey) = sendKey.trigger = key.ToLower() && sendKey.isCaps = isCaps
        predicate
  
    let GetSendKeyNormal (keysList:seq<MappedKey>, key:string) = 
        GetSendKey (keysList, key, GetPredicate(false, key))
  
    let GetSendKeyCaps (keysList:seq<MappedKey>, key:string) = 
        GetSendKey (keysList, key, GetPredicate(true, key))

    let GetMappedKeyNormal keys key = GetSendKeyNormal(keys, key)
    let GetMappedKeyCaps keys key = GetSendKeyCaps(keys, key)
