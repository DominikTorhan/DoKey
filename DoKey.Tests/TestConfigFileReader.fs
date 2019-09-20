module TestConfigFileReader

open Xunit 
open System  
open DoKey.Core.ConfigFileReader
 
[<Theory>] 
[<InlineData(1, "//x\r\n \r\nj {LEFT}")>] 
[<InlineData(3, "//x\r\n \r\n//xxxx\r\nj {LEFT}\r\nk {UP}\r\n_CAPS_j {ENTER}")>] 
let TestTextFileToMappedKeys(expected: int, fileText: string) = 
    let seq = TextFileToMappedKeys fileText  
    let len = seq |> Seq.length
    Assert.Equal(expected, len) 
 
[<Theory>] 
[<InlineData("", "")>] 
[<InlineData(" ", " ")>] 
[<InlineData("", "//xxxx")>] 
[<InlineData("aaa", "aaa//xxxx")>] 
[<InlineData("aa aa", "aa aa//xxxx // adsfa")>]  
[<InlineData("eoem5            ^\ ", "eoem5            ^\ ")>]  
let TestRemoveComment(expected: string, str: string) =     
    let str' = RemoveComment str
    Assert.Equal(expected, str' ) 
  
[<Theory>] 
[<InlineData("", "")>] 
[<InlineData("", "x")>] 
[<InlineData("j{LEFT}False", "j {LEFT}")>] 
[<InlineData("j{ENTER}True", "_CAPS_j {ENTER}")>] 
[<InlineData("eoem5^\False", "eoem5            ^\ ")>] 
let TestLineToMappedKey(expected: string, line: string) = 
    let str = match LineToMappedKey line with
        | Some(sendKey) -> sendKey.trigger + sendKey.send + sendKey.isCaps.ToString()
        | None -> ""
    Assert.Equal(expected, str) 
