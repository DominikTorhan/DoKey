using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrayApp2 {
  public class cSettings {

    Dictionary<string, string> mDic;

    public cSettings() {

      mDic = Load();

    }

    public Dictionary<string,string> Load() {

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

    public string SendKeyNormal(string key) => SendKey("n", key);
    internal string SendKeyNormalWithControl(string key) => SendKey("nc", key);
    internal string SendKeyInsert(string key) => SendKey("ic", key);


    private string SendKey(string prefix, string key) {

      var keyDic = prefix + "." + key.ToLower();

      if (!mDic.ContainsKey(keyDic)) return "";

      return mDic[keyDic];


    }
  }
}
