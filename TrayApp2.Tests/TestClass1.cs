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

    [TestCase(Keys.Escape, false, false, StateEnum.Off)]
    public void TestMain(Keys keys, bool isUp, bool isESC, StateEnum state) {

      var input = new cInput {
        eventData = new cEventData {
          keys = keys,
          isUp = isUp,
        },
        stateData = new cStateData {
          isControl = isESC,
          state = state,
        }
      };

      new cKeysEngine { input = input}.ProcessKey();

    }

  }
}
