using DoKey.CoreCS;
using DoKey.CoreCS.KeysProcessor;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DoKey.TestsCS
{

  public class StateChangeProcessorTest
  {
    [Theory]
    [InlineData("1 c  True", "0 null m=c", "f")]
    [InlineData("2 c  True", "1 null m=c", "f")]
    [InlineData("1   False", "2 null m=", "escape")]
    [InlineData("0 c  True", "1 null m=c", "q")]
    [InlineData("0 c  True", "2 null m=c", "q")]
    [InlineData("", "2 null m=", "q")]
    public void TestProcessStateChange(string expected, string strAppState, string key)
    {
      var appState = TestHelpers.CreateAppState(strAppState);
      var inputKey = DomainUtils.CreateInputKey(key);
      var processor = new StateChangeProcessor(appState, inputKey);
      var result = processor.ProcessStateChange();
      var actual = result != null ? result.ToString() : "";
      Assert.Equal(expected, actual);
    }
  }
}
