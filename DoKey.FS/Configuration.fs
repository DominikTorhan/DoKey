namespace DoKey.FS
  
open DoKeyModule 
open System.IO

type Configuration() =  
  
    let ReadKeysFromFile(path) =  
        let lines = File.ReadAllLines(path)  
        let keys = lines |> Array.map DoKeyModule.CreateSendDoKeyByStr 
        Array.toList keys 
 
    let CreateKeys(path) = 
        let keys = DoKeyModule.GetList
        let keysCustom = ReadKeysFromFile(path) 
        List.append keys keysCustom

    member this.x = ""  
    member this.FilePath = "Settings.txt"
    member this.Keys = CreateKeys(this.FilePath)
    member this.test = ReadKeysFromFile(this.FilePath)
    member this.ModeChangeKey = "f"
    member this.ModeOffKey = "q"
 
    member this.GetSendKeyNormal (key) = DoKeyModule.GetSendKeyNormal(this.Keys, key)
    member this.GetSendKeyCaps (key) = DoKeyModule.GetSendKeyCaps(this.Keys, key)
    member this.IsTwoStep (key) = "uifed".Contains(key)