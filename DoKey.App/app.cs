using DoKey.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DoKey.Core.Domain;

namespace DoKey.App
{
  class App
  {
    private Session _session; 
 
    public void Initialize()
    {

      _session = DomainOperations.CreateSession;

    } 
 
    public KeysEngineResult Work(KeyEventData keyEventData)
    {

      KeysEngineResult output = ProcessKey(keyEventData);

      if (output == null) return null;

      _session = new Session(_session.config, output.appState);

      return output;

    }

    private KeysEngineResult ProcessKey(KeyEventData keyEventData)
    {

      return new KeysEngine
      {
        AppStateX = _session.appState,
        inputKey = keyEventData.inputKey,
        isUp = keyEventData.keyEventType.IsUp,
        config = _session.config,
      }.ProcessKey();

    }


  }
}
