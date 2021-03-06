﻿namespace DoKey.Core 
 
open DoKey.Core.Domain
    
module ModificatorsOperations = 
 
    let CreateModificatorsByStr(str: string) =
        let func = str.Contains 
        { alt = func("%"); control = func("^"); shift = func("+"); win = func("w"); caps = func("c")}
        

    let GetNextModificators(modif: Modificators, input: InputKey, isUp:bool)= 
        let alt = match input.isAlt with | true -> not isUp | false -> modif.alt
        let con = match input.isControl with | true -> not isUp | false -> modif.control
        let sht = match input.isShift with | true -> not isUp | false -> modif.shift
        let win = match input.isWin with | true -> not isUp | false -> modif.win
        let cap = match input.isCaps with | true -> not isUp | false -> modif.caps
        { alt = alt; control = con; shift = sht; win = win; caps = cap}
         
    let ModificatorsToStr(modif:Modificators) = 
        let str    =            if modif.alt then "%" else "" 
                              + if modif.control then "^" else "" 
                              + if modif.shift then "+" else "" 
                              + if modif.win then "w" else "" 
                              + if modif.caps then "c" else "" 
        str 

    let ModificatorsToLog(modif:Modificators) = 
        "mod: " + if modif.alt then " alt " else "" 
                                     + if modif.control then " ctrl " else "" 
                                     + if modif.shift then " shift " else "" 
                                     + if modif.win then " win " else "" 
                                     + if modif.caps then " caps " else ""
