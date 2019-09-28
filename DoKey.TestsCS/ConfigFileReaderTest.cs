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


//common
h {LEFT}//left arrow
j {DOWN}
k {UP}
";

    }

    [Fact]
    public void TestCreateConfigByFile()
    {
      var reader = new ConfigFileReader(ReadText);
      var config = reader.CreateConfigByFile();
      Assert.Equal(4, config.mappedKeys.Count());
    }
  }
}
