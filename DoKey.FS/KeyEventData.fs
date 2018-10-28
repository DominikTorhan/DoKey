namespace DoKey.FS
 
type KeyEventType = Down|Up

type KeyEventData (key:string, keyEventType:KeyEventType) =
    member this.Key = key
    member this.KeyEventType = keyEventType
    member this.ToLog() = key + "." + keyEventType.ToString()

