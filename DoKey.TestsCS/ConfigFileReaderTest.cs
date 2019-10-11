using DoKey.CoreCS;
using System;
using System.Linq;
using Xunit;

namespace DoKey.TestsCS
{
  public class UnitTest1
  {

    private string ReadText()
    {

      return @"_CAPS_n ^+{TAB}
_COMMAND__CAPS_back #exit           //exit app


//common
h {LEFT}//left arrow
j {DOWN}
k {UP}
";

    }

    [Fact]
    public void TestCreateConfigByFile_MappedKeys()
    {
      var reader = new ConfigFileReader(ReadText);
      var config = reader.CreateConfigByFile();
      Assert.Equal(4, config.mappedKeys.Count());
    }

    [Fact]
    public void TestCreateConfigByFile_CommandKeys()
    {
      var reader = new ConfigFileReader(ReadText);
      var config = reader.CreateConfigByFile();
      Assert.Equal(1, config.commandKeys.Count());
    }

  }
}
