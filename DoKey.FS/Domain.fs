namespace DoKey.FS 
 

module Domain =   
  
    type State = Off = 0 | Normal = 1 | Insert = 2

    type SendKey =
        { trigger: string
          send: string 
          isCaps: bool }

 
    type KeysList(keyList:list<SendKey>) = 
        member this.KeyList = keyList

