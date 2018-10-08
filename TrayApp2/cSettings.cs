﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrayApp2 {
  internal class cSettings {

    Dictionary<string, string> mDic;

    public cSettings() {

      mDic = Load();

    }

    internal Dictionary<string,string> Load() {

      var lines = File.ReadLines("json1.json");

      return lines
        .Select(ReadLine)
        .Where(t => t != null)
        .ToDictionary(t => t.Item1, t => t.Item2);

    }

    private Tuple<string, string> ReadLine(string line) {
      if (!line.Contains(":")) return null;
      var split = line.Split(':');
      var key = ReadKey(split[0]);
      var val = ReadKey(split[1]);
      return new Tuple<string, string>(key, val);
    }

    private string ReadKey(string line) {
      var a = line.TrimStart(TrimChars());
      var b = a.TrimEnd(TrimChars());
      return b;
    }

    private char[] TrimChars() => new[] { ' ', '\"', ',' };

    internal string SendKeyNormal(string key) {

      var keyDic = "n." + key.ToLower();

      if (!mDic.ContainsKey(keyDic)) return "";

      return mDic[keyDic];

    }

  }
}
