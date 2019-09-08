module TestConfigFileReader

open Xunit 
open System 
 
type SendKey =
    { trigger: string
      send: string 
      isCaps: bool }


let SplitToLines(str: string) =
    let seq = str.Split([|"\n"|], StringSplitOptions.RemoveEmptyEntries) |> Array.toSeq   
    seq  
  
//let CreateSendKey(trigger : string, send : string) =
//    {trigger = trigger; send = send; isCaps = false} 


let LineToSendKey(line : string) =
    let strs = line.Split([|" "|], StringSplitOptions.RemoveEmptyEntries)     
    let trigger = match strs.Length with
        | 2 -> strs.[0]
        | _ -> "" 
    let send = match strs.Length with
        | 2 -> strs.[1]
        | _ -> "" 
    let strCaps = "_CAPS_" 
    let trigger' = trigger.Replace(strCaps, "") 
    let isCaps = trigger.Contains strCaps
    match trigger with
        | "" -> None
        | _ -> Some {trigger = trigger'; send = send; isCaps = isCaps } 

[<Theory>] 
[<InlineData(1, "//x\r\n \r\nj {LEFT}")>] 
[<InlineData(3, "//x\r\n \r\n//xxxx\r\nj {LEFT}\r\nk {UP}\r\n_CAPS_j {ENTER}")>] 
let Test(expected: int, fileText: string) = 
    let seq = SplitToLines fileText  
    let seq' = seq |> Seq.map LineToSendKey |> Seq.choose id
    let len = seq' |> Seq.length
    Assert.Equal(expected, len) 
 
[<Theory>] 
[<InlineData("", "//xxxx")>] 
[<InlineData("aaa", "aaa//xxxx")>] 
[<InlineData("aa aa", "aa aa//xxxx // adsfa")>] 
let TestRemoveComment(expected: string, str: string) =     
    let idx = str.IndexOf("//")
    let comment = str.Substring(idx); 
    let str' = str.Replace(comment, "")
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
