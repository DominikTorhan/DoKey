module TestConfiguration
 
open Xunit 
open DoKey.FS

[<Theory>]   
[<InlineData("", "")>] 
[<InlineData("{BKSP}", "h")>] 
let TestGetSendKeyCaps (expected:string,key:string)=
    let conf = new Configuration ()  
    let sendDoKey = conf.GetSendKeyCaps key
    Assert.Equal(expected, sendDoKey.Send)