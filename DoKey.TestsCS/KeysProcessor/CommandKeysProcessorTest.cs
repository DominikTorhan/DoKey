using DoKey.CoreCS;
using DoKey.CoreCS.KeysProcessor;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DoKey.TestsCS.KeysProcessor
{

  public class CommandKeysProcessorTheoryData<T> : TheoryData<T>
  {
    public CommandKeysProcessorTheoryData(IEnumerable<T> data)
    {
      foreach (var d in data)
      {
        Add(d);
      }
    }
  }

  public class CommandKeysProcessorData : TheoryData{
    public readonly InputKey inputKey;
    public readonly AppState appState;
    public readonly Config config;
  }

  public class CommandKeysProcessorTest
  {

    public static CommandKeysProcessorTheoryData<CommandKeysProcessorData> p = new CommandKeysProcessorTheoryData<CommandKeysProcessorData>(null);

    [Theory]
    [MemberData(nameof(p))]
    public void TestX(CommandKeysProcessorData data)
    {


    }

    [Theory]
    [InlineData("1 c  True #exit True", "1 null m=c", "back")]
    [InlineData("1 c  True #config True", "1 null m=c", "oem2")]
    [InlineData("", "1 null m=c", "h")]
    [InlineData("", "2 null m=c", "l")]
    [InlineData("", "1 null m=c", "p")]
    public void Test(string expected, string strAppState, string key)
    {
      var appState = TestHelpers.CreateAppState(strAppState);
      var inputKey = DomainUtils.CreateInputKey(key);
      var config = TestHelpers.CreateConfig();
      var processor = new CommandKeysProcessor(inputKey, appState, config.commandKeys);
      var result = processor.Process();
      var actual = result != null ? result.ToString() : "";
      Assert.Equal(expected, actual);
    }

  }
}
