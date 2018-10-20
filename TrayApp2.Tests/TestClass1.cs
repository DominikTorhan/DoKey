using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrayApp2.Tests {

  [TestFixture]
  public class TestClass1 {

    private cSettings settings = new cSettings(@"C:\Users\dominik\Source\Repos\TrayApp2\TrayApp2\json1.json");

    [TestCase("p  cap: True first: None state: Off", Keys.Capital, false, StateEnum.Off, false)]
    [TestCase("p  cap: True first: None state: Normal", Keys.F, false, StateEnum.Off, true)]
    //normal
    [TestCase("p  cap: True first: None state: Insert", Keys.F, false, StateEnum.Normal, true)]
    [TestCase("p {DOWN} cap: False first: None state: Normal", Keys.J, false, StateEnum.Normal, false)]
    [TestCase("p ^c cap: False first: None state: Normal", Keys.C, false, StateEnum.Normal, false)]
    //insert
    [TestCase("p {BKSP} cap: True first: None state: Insert", Keys.H, false, StateEnum.Insert, true)]
    [TestCase(" cap: False first: None state: Insert", Keys.A, false, StateEnum.Insert, false)]
    public void TestMain(string expected, Keys keys, bool isUp, StateEnum state, bool isCap) {

      var input = new cInput {
        eventData = new cEventData {
          keys = keys,
          isUp = isUp,
        },
        stateData = new cStateData {
          state = state,
          isCapital = isCap
        }
      };
      
      var output = new cKeysEngine { input = input, settings = settings}.ProcessKey();

      Assert.AreEqual(expected, output.ToString());

    }

  }
}
