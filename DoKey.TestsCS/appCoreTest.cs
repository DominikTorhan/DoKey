using DoKey.CoreCS;
using Xunit;

namespace DoKey.TestsCS
{
  public class appCoreTest
  {
    private AppConfig CreateAppConfig()
    {
      return new AppConfig
      {
        funcGetConfigText = TestHelpers.GetFuncGetConfig()
      };
    }
    private AppProcessor CreateAppCore()
    {
      return null;
      //return new AppCore(CreateAppConfig());
    }
    [Theory]
    [InlineData(false, "", false)]
    [InlineData(true, "capital", false)]
    public void Test(bool expected, string key, bool isUp)
    {
      //var appCore = CreateAppCore();
      //var keyEvent = new KeyEvent
      //{
      //  key = key,
      //  isUp = isUp,
      //};
      //var actual = appCore.PerformKeyPress(keyEvent);
      //Assert.Equal(expected, actual);
    }
  }
}
