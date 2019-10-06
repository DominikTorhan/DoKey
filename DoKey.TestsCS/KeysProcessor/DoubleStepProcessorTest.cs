using DoKey.CoreCS;
using DoKey.CoreCS.KeysProcessor;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DoKey.TestsCS.KeysProcessor
{
  public class DoubleStepProcessorTest
  {
    [Theory]
    [InlineData("1  f False  True", "1 null m=", "f")]//first key
    [InlineData("1  g False  True", "1 null m=", "g")]
    [InlineData("1  t False  True", "1 null m=", "t")]
    [InlineData("1  q False  True", "1 null m=", "q")]
    [InlineData("1   False {F12} True", "1 f m=", "f")]//second key -> send
    [InlineData("1   False {End}{ENTER} True", "1 i m=", "j")]
    [InlineData("1   False  True", "1 i m=", "x")]//no send, clear first key
    [InlineData("", "1 null m=", "h")]//invalid
    [InlineData("", "0 null m=", "j")]
    [InlineData("", "2 null m=", "j")]
    [InlineData("", "1 null m=c", "j")]
    public void Test(string expected, string strAppState, string key)
    {
      var appState = TestHelpers.CreateAppState(strAppState);
      var inputKey = DomainUtils.CreateInputKey(key);
      var config = TestHelpers.CreateConfig();
      var processor = new DoubleStepProcessor(inputKey, appState, config.mappedKeys);
      var result = processor.TryGetSingleStep();
      var actual = result != null ? result.ToString() : "";
      Assert.Equal(expected, actual);
    }


  }
}
