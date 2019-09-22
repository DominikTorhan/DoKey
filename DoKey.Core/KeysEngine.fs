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
    
    let GetMappedKeyNormalAndInsertWithCapital appState key keys isUp=
        match isUp with
            | true -> EmptyMappedKey
            | _ -> match appState.state with 
                   | State.Off -> EmptyMappedKey
                   | _ -> match appState.modificators.caps with
                          | false -> EmptyMappedKey
                          | _ -> GetMappedKey true (appState.firstStep + key) keys 
        
