using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DoKey.CoreCS
{
  class ConfigFileReader
  {

    private readonly string _path;

    string capsSymbol = "_CAPS_";
    string commadSymbol = "_COMMAND_";

    public ConfigFileReader(string path)
    {
      _path = path;
    }

    private string RemoveComment(string line)
    {
      var idx = line.IndexOf("//");
      if (idx == -1) return line;
      return line.Replace(line.Substring(idx), "");
    }

    private string[] SplitToLines(string str)
    {
      return str.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
    }

    private string[] SplitLine(string line)
    {
      var line_ = line.Replace("\n", "").Replace("\r", "");
      return line_.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
    }

    private string GetStringByIdx(string[] strs, int idx)
    {
      if (strs.Length == 2) return strs[idx];
      return "";
    }

    private string NormalizeString(string str)
    {
      return str.Replace("\n", "").Replace("\r", "").Replace(" ", "");
    }

    private MappedKey LineToMappedKey(string line)
    {
      var strs = SplitLine(line);
      var trigger = GetStringByIdx(strs, 0);
      var send = NormalizeString(GetStringByIdx(strs, 1));
      var trigger_ = trigger.Replace(capsSymbol, "");
      var isCaps = trigger.Contains(capsSymbol);
      if (trigger == "") return null;
      return new MappedKey { trigger = trigger_, send = send, isCaps = isCaps };
    }


    //private CommandKey LineToCommandKey(line:string) =
    //    let strs = SplitLine line
    //    let trigger = GetStringByIdx strs 0
    //    let run = GetStringByIdx strs 1
    //    let trigger' = trigger.Replace(commadSymbol, "").Replace(capsSymbol, "")  
    //    match trigger.Contains commadSymbol with
    //        | false -> None
    //        | _ -> match trigger with
    //                | "" -> None
    //                | _ -> Some {
    //  trigger = trigger'; run = run } 

    private IEnumerable<MappedKey> TextFileToMappedKeys(string text)
    {
      return SplitToLines(text).Select(RemoveComment).Select(LineToMappedKey);
    }

    //let TextFileToCommandKeys text =
    //    SplitToLines text |> Seq.map RemoveComment |> Seq.map LineToCommandKey |> Seq.choose id

    public Config CreateConfigByFile()
    {
      var text = File.ReadAllText(_path);
      return new Config
      {
        mappedKeys = TextFileToMappedKeys(text),
        commandKeys = new List<CommandKey>()
      };
    }

  }
}
