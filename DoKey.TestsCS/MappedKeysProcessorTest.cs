using DoKey.CoreCS;
using DoKey.CoreCS.KeysProcessor;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DoKey.TestsCS
{
  public class MappedKeysProcessorTest
  {
    //Normal
    //simple key 
    //two step
    //Normal||Insert
    //caps
    private string path = @"C:\Users\dominik\Source\Repos\DoKey\DoKey.App\config.txt";
    [Theory]
    //[InlineData("", "0 null m=", "f")]
    //[InlineData("", "0 null m=", "capital")]
    //[InlineData("1   False  False", "1 null m=", "capital")]
    //simple
    [InlineData("1   False {LEFT} True", "1 null m=", "h")]
    [InlineData("1   False {DOWN} True", "1 null m=", "j")]
    [InlineData("1   False {UP} True", "1 null m=", "k")]
    [InlineData("1   False {RIGHT} True", "1 null m=", "l")]
    [InlineData("1   False {HOME} True", "1 null m=", "n")]
    [InlineData("1   False {PGDN} True", "1 null m=", "m")]
    [InlineData("1   False {PGUP} True", "1 null m=", "oemcomma")]//,
    [InlineData("1   False {END} True", "1 null m=", "oemperiod")]//.
    [InlineData("1   False ^{LEFT} True", "1 null m=", "y")]
    [InlineData("1   False ^{RIGHT} True", "1 null m=", "o")]
    [InlineData("1   False ^x True", "1 null m=", "x")]
    [InlineData("1   False ^c True", "1 null m=", "c")]
    [InlineData("1   False ^v True", "1 null m=", "v")]
    [InlineData("1   False ^z True", "1 null m=", "z")]
    //two step
    //[InlineData("1  s False  True", "1 null m=", "s")]
    //[InlineData("1   False {F12} True", "1 f m=", "f")]
    public void Test(string expected, string strAppState, string key)
    {
      var appState = TestHelpers.CreateAppState(strAppState);
      var inputKey = DomainUtils.CreateInputKey(key);
      var config = new ConfigFileReader(() => System.IO.File.ReadAllText(path)).CreateConfigByFile();
      var processor = new MappedKeysProcessor(inputKey, appState, config.mappedKeys);
      var result = processor.Process();
      var actual = result != null ? result.ToString() : "";
      Assert.Equal(expected, actual);
    }
  }
}
