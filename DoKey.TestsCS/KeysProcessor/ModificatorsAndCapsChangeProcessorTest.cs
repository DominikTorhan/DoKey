using DoKey.CoreCS;
using DoKey.CoreCS.KeysProcessor;
using Xunit;

namespace DoKey.TestsCS
{
  public class ModificatorsAndCapsChangeProcessorTest
  {
    [Theory]
    [InlineData("", "0 null m=", "f", false)]
    [InlineData("0 %  False  False", "0 null m=", "lmenu", false)]
    [InlineData("0 %c  False  True", "0 null m=%", "capital", false)]
    [InlineData("0 %  False {ESC} True", "0 null m=%", "capital", true)]
    [InlineData("0 %wc  False  False", "0 null m=wc", "rmenu", false)]
    [InlineData("0 +%wc  False  False", "0 null m=^%+wc", "controlkey", true)]
    [InlineData("0 ^+%c  False  False", "0 null m=^%+wc", "lwin", true)]
    public void Test(string expected, string strAppState, string key, bool isUp)
    {
      var appState = TestHelpers.CreateAppState(strAppState);
      var inputKey = DomainUtils.CreateInputKey(key);
      var processor = new ModificatorsAndCapsChangeProcessor(appState, inputKey, isUp);
      var result = processor.ProcessKey();
      var actual = result != null ? result.ToString() : "";
      Assert.Equal(expected, actual);
    }
  }
}
