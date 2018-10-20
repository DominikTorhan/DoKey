using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrayApp2 {
  public class cSettings {

    private Dictionary<string, string> mDic;
    public Keys ModeChangeKey = Keys.F;

    public cSettings(string path) {

      mDic = Load(path);

    }

    public Dictionary<string,string> Load(string path) {

      var lines = File.ReadLines(path);

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

    internal bool IsTwoStep(string key) {

      if (!mDic.ContainsKey("2step")) return false;

      return mDic["2step"].Contains(key.ToLower());

    }

    private string SendKey(string prefix, string key) {

      var keyDic = prefix + "." + key.ToLower();

      if (!mDic.ContainsKey(keyDic)) return "";

      return mDic[keyDic];


    }
  }
}
