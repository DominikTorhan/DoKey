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
    //simple key  - done
    //two step
    //Normal||Insert
    //caps
    [Theory]
    //[InlineData("", "0 null m=", "f")]
    //[InlineData("", "0 null m=", "capital")]
    //[InlineData("1   False  False", "1 null m=", "capital")]
    //[InlineData("1  s False  True", "1 null m=", "s")]

    [InlineData("1 c  True {ENTER} True", "1 null m=c", "j")]
    [InlineData("1 c  True {BKSP} True", "1 null m=c", "h")]
    [InlineData("2 c  True {DEL} True", "2 null m=c", "l")]
    [InlineData("", "1 null m=c", "p")]
    [InlineData("", "2 null m=c", "p")]
    public void Test(string expected, string strAppState, string key)
    {
      var appState = TestHelpers.CreateAppState(strAppState);
      var inputKey = DomainUtils.CreateInputKey(key);
      var config = TestHelpers.CreateConfig();
      var processor = new MappedKeysProcessor(inputKey, appState, config.mappedKeys);
      var result = processor.Process();
      var actual = result != null ? result.ToString() : "";
      Assert.Equal(expected, actual);
    }
  }
}
