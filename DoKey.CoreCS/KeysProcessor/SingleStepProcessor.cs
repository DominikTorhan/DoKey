using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoKey.CoreCS.KeysProcessor
{
  public class SingleStepProcessor
  {
    private readonly InputKey inputKey;
    private readonly AppState appState;
    private readonly IEnumerable<MappedKey> mappedKeys;

    public SingleStepProcessor(InputKey inputKey, AppState appState, IEnumerable<MappedKey> mappedKeys)
    {
      this.inputKey = inputKey;
      this.appState = appState;
      this.mappedKeys = mappedKeys;
    }

    public KeysEngineResult TryGetSingleStep()
    {
      var mappedKey = TryGetMappedKey();
      if (mappedKey == null) return null;
      return new KeysEngineResult
      {
        appState = appState,
        send = mappedKey.send,
        preventKeyProcess = true
      };
    }

    private MappedKey TryGetMappedKey()
    {
      if (appState.state != State.Normal) return null;
      if (appState.modificators.caps) return null;
      if (appState.modificators.win) return null;
      if (appState.firstStep != "") return null;
      if (inputKey.isFirstStep) return null;
      var mappedKey = mappedKeys.FirstOrDefault(key => key.isCaps == false && key.trigger == inputKey.key);
      return mappedKey;
    }

  }
}
