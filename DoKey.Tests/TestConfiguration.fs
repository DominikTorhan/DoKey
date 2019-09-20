module TestConfiguration
 
open Xunit 
open DoKey.Core

//[<Theory>]   
//[<InlineData("", "")>] 
//[<InlineData("{BKSP}", "h")>] 
//let TestGetSendKeyCaps (expected:string,key:string)=
//    let conf = new Configuration ()  
//    let sendKey = conf.GetSendKeyCaps key
//    Assert.Equal(expected, sendKey .send)dhdh