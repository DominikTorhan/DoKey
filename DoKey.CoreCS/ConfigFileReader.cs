﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace DoKey.CoreCS
{
  public class ConfigFileReader
  {

    private readonly Func<string> _funcReadText;
    private readonly string capsSymbol = "_CAPS_";
    private readonly string commadSymbol = "_COMMAND_";

    public ConfigFileReader(Func<string> funcReadText)
    {
      _funcReadText = funcReadText;
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
      if (trigger.Contains(commadSymbol)) return null;
      var send = NormalizeString(GetStringByIdx(strs, 1));
      var trigger_ = trigger.Replace(capsSymbol, "");
      var isCaps = trigger.Contains(capsSymbol);
      if (trigger == "") return null;
      return new MappedKey(trigger_, send, isCaps);
    }

    private CommandKey LineToCommandKey(string line)
    {
      var strs = SplitLine(line);
      var trigger = GetStringByIdx(strs, 0);
      if (!trigger.Contains(capsSymbol)) return null;
      if (!trigger.Contains(commadSymbol)) return null;
      var run = NormalizeString(GetStringByIdx(strs, 1));
      var trigger_ = trigger.Replace(capsSymbol, "").Replace(commadSymbol, "");
      if (trigger == "") return null;
      return new CommandKey(trigger_, run);
    }

    private IEnumerable<MappedKey> TextFileToMappedKeys(string text)
    {
      return SplitToLines(text).Select(RemoveComment).Select(LineToMappedKey).Where(k => k != null);
    }

    private IEnumerable<CommandKey> TextFileToCommandKeys(string text)
    {
      return SplitToLines(text).Select(RemoveComment).Select(LineToCommandKey).Where(k => k != null);
    }

    public Config CreateConfigByFile()
    {
      var text = _funcReadText();
      var mappedKeys = TextFileToMappedKeys(text);
      var commandKeys = TextFileToCommandKeys(text);
      return new Config(mappedKeys, commandKeys);
    }

  }
}
