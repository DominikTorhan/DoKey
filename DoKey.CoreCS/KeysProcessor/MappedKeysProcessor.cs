using System.Collections.Generic;
using System.Linq;

namespace DoKey.CoreCS.KeysProcessor
{
  public class MappedKeysProcessor
  {
    private readonly InputKey inputKey;
    private readonly AppState appState;
    private readonly IEnumerable<MappedKey> mappedKeys;

    public MappedKeysProcessor(InputKey inputKey, AppState appState, IEnumerable<MappedKey> mappedKeys)
    {
      this.inputKey = inputKey;
      this.appState = appState;
      this.mappedKeys = mappedKeys;
    }
    public KeysEngineResult Process()
    {
      return ProcessNormalAndInsertWithCapital();

    }

    private KeysEngineResult ProcessNormalAndInsertWithCapital()
    {
      if (!appState.modificators.caps) return null;
      if (appState.state == State.Off) return null;

      var mappedKey = GetMappedKeyNormalAndInsertWithCapital();
      if (mappedKey == null) return null;
      if (mappedKey.send == "") return null;
        var nextAppState = new AppState
      {
        state = appState.state,
        modificators = appState.modificators,
        firstStep = "",
        preventEscOnCapsUp = true
      };
      return new KeysEngineResult
      {
        appState = nextAppState,
        send = mappedKey.send,
        preventKeyProcess = true
      };
    }


    private MappedKey GetMappedKeyNormalAndInsertWithCapital()
    {
      return mappedKeys.FirstOrDefault(key => key.isCaps == true && key.trigger == inputKey.key);
    }

  }
}
