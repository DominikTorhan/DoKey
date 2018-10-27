using DoKey.FS;
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
    private KeysList KeysList; 

    public Keys ModeChangeKey = Keys.F;
    public Keys ModeOffKey = Keys.Q;

    public cSettings(string path) {

      //mDic = Load(path);

      KeysList = DoKeyModule.CreateKeysList;

    }

    //public Dictionary<string,string> Load(string path) {

    //  var lines = File.ReadLines(path);

    //  return lines
    //    .Select(ReadLine)
    //    .Where(t => t != null)
    //    .ToDictionary(t => t.Item1, t => t.Item2);

    //}

    //private Tuple<string, string> ReadLine(string line) {
    //  if (!line.Contains(":")) return null;
    //  var split = line.Split(':');
    //  var key = ReadKey(split[0]);
    //  var val = ReadKey(split[1]);
    //  return new Tuple<string, string>(key, val);
    //}

    //private string ReadKey(string line) {
    //  var a = line.TrimStart(TrimChars());
    //  var b = a.TrimEnd(TrimChars());
    //  return b;
    //}

    //private char[] TrimChars() => new[] { ' ', '\"', ',' };

    //public string SendKeyNormal(string key) => SendKey("n", key);
    //public string SendKeyNormal(string key) => GetSendKeyNormal(key); 

    //internal string SendKeyNormalWithControl(string key) => SendKey("nc", key);
    //internal string SendKeyInsert(string key) => SendKey("ic", key);

    internal bool IsTwoStep(string key) {

      string x = "uif";

      return x.Contains(key.ToLower());

    } 
 
    public string GetSendKeyNormal(string key) {
      return DoKeyModule.GetSendKeyNormal(KeysList, key);
    }
    public string GetSendKeyCaps(string key) {
      return DoKeyModule.GetSendKeyCaps(KeysList, key);
    }

    //private string SendKey(string prefix, string key) {

    //  var keyDic = prefix + "." + key.ToLower();

    //  if (!mDic.ContainsKey(keyDic)) return "";

    //  return mDic[keyDic];


    //}
  }
}
