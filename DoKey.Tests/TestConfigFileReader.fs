module TestConfigFileReader

open Xunit 
open System  
open DoKey.FS.ConfigFileReader
 
[<Theory>] 
[<InlineData(1, "//x\r\n \r\nj {LEFT}")>] 
[<InlineData(3, "//x\r\n \r\n//xxxx\r\nj {LEFT}\r\nk {UP}\r\n_CAPS_j {ENTER}")>] 
let TestTextFileToKeys(expected: int, fileText: string) = 
    let seq = TextFileToKeys fileText  
    let len = seq |> Seq.length
    Assert.Equal(expected, len) 
 
[<Theory>] 
[<InlineData("", "")>] 
[<InlineData(" ", " ")>] 
[<InlineData("", "//xxxx")>] 
[<InlineData("aaa", "aaa//xxxx")>] 
[<InlineData("aa aa", "aa aa//xxxx // adsfa")>] 
let TestRemoveComment(expected: string, str: string) =     
    let str' = RemoveComment str
    Assert.Equal(expected, str' ) 
  
[<Theory>] 
[<InlineData("", "")>] 
[<InlineData("", "x")>] 
[<InlineData("j{LEFT}False", "j {LEFT}")>] 
[<InlineData("j{ENTER}True", "_CAPS_j {ENTER}")>] 
let TestLineToSendKey(expected: string, line: string) = 
    let str = match LineToSendKey line with
        | Some(sendKey) -> sendKey.trigger + sendKey.send + sendKey.isCaps.ToString()
        | None -> ""
    Assert.Equal(expected, str) 
