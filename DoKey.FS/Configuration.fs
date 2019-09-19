namespace DoKey.FS
  
open DoKeyModule 
open System.IO 
open Domain

type Configuration() =  
  
    let ReadKeysFromFile(path) =  
        let lines = File.ReadAllLines(path)  
        let keys = lines |> Array.map DoKeyModule.CreateSendDoKeyByStr 
        Array.toList keys 
 
    let CreateKeys(path) = 
        let keys = DoKeyModule.GetList
        let keysCustom = ReadKeysFromFile(path) 
        List.append keys keysCustom

    member this.Keys = CreateKeys(filePath)
 
    member this.GetSendKeyNormal (key) = DoKeyModule.GetSendKeyNormal(this.Keys, key)
    member this.GetSendKeyCaps (key) = DoKeyModule.GetSendKeyCaps(this.Keys, key)
    member this.IsTwoStep (key) = "qwertasdfgbui".Contains(key)