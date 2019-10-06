using System.Collections.Generic;
using System.Linq;

namespace DoKey.CoreCS.KeysProcessor
{
  public class DoubleStepProcessor
  {
    private readonly InputKey inputKey;
    private readonly AppState appState;
    private readonly IEnumerable<MappedKey> mappedKeys;

    public DoubleStepProcessor(InputKey inputKey, AppState appState, IEnumerable<MappedKey> mappedKeys)
    {
      this.inputKey = inputKey;
      this.appState = appState;
      this.mappedKeys = mappedKeys;
    }

    public KeysEngineResult TryGetSingleStep()
    {
      if (appState.state != State.Normal) return null;
      if (appState.modificators.caps) return null;
      if (appState.modificators.win) return null;
      var result = TryProcessFirstStep();
      if (result != null) return result;
      if (appState.firstStep == "") return null;
      var send = TryGetMappedKey();
      return new KeysEngineResult
      {
        appState = CreateAppStateWithFirstStep(""),//clear first step
        send = send,
        preventKeyProcess = true
      };
    }

    private AppState CreateAppStateWithFirstStep(string firstStep)
    {
      return new AppState
      {
        firstStep = firstStep,
        modificators = appState.modificators,
        state = appState.state,
        preventEscOnCapsUp = appState.preventEscOnCapsUp,
      };
    }

    private KeysEngineResult TryProcessFirstStep()
    {
      if (appState.firstStep != "") return null;
      if (!inputKey.isFirstStep) return null;
      var nextAppState = CreateAppStateWithFirstStep(inputKey.key);
      return new KeysEngineResult
      {
        appState = nextAppState,
        send = "",
        preventKeyProcess = inputKey.isLetterOrDigit
      };
    }

    private string TryGetMappedKey()
    {
      var trigger = appState.firstStep + inputKey.key;
      var mappedKey = mappedKeys.FirstOrDefault(key => key.isCaps == false && key.trigger == trigger);
      return mappedKey?.send ?? "";
    }

  }
}
