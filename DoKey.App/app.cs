//using DoKey.Core;
using DoKey.CoreCS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using static DoKey.Core.Domain;

namespace DoKey.App
{
  //class App
  //{
  //  private Session _session;

  //  public void Initialize(Func<string> GetConfigText)
  //  {

  //    _session = DomainOperations.CreateSession;

  //  }

  //  public KeysEngineResult Work(KeyEventData keyEventData)
  //  {

  //    KeysEngineResult output = ProcessKey(keyEventData);

  //    if (output == null) return null;

  //    _session = new Session(_session.config, output.appState);

  //    return output;

  //  }

  //  private KeysEngineResult ProcessKey(KeyEventData keyEventData)
  //  {
  //    bool isUp = keyEventData.keyEventType == KeyEventType.Up;

  //    return new KeysEngine
  //    {
  //      AppStateX = _session.appState,
  //      inputKey = keyEventData.inputKey,
  //      isUp = keyEventData.keyEventType.IsUp,
  //      config = _session.config,
  //    }.ProcessKey();

  //  }


  //}

  //CS
  class App
  {
    private Session _session;

    public void Initialize(Func<string> GetConfigText)
    {

      _session = DomainUtils.CreateSession(GetConfigText);

    }

    public KeysEngineResult Work(KeyEventData keyEventData)
    {

      KeysEngineResult output = ProcessKey(keyEventData);

      if (output == null) return null;

      _session = new Session { config = _session.config, appState = output.appState };

      return output;

    }

    private KeysEngineResult ProcessKey(KeyEventData keyEventData)
    {
      bool isUp = keyEventData.keyEventType == KeyEventType.Up;

      var processor = new DoKey.CoreCS.KeysProcessor.KeysProcessor(_session.appState,
        keyEventData.inputKey,
        isUp,
        _session.config);

      return processor.ProcessKey();

    }


  }


}
