using System.Collections.Generic;
using System.Linq;

namespace DoKey.CoreCS.KeysProcessor
{
  public class MappedKeysProcessor
  {
    private readonly InputKey _inputKey;
    private readonly AppState _appState;
    private readonly IEnumerable<MappedKey> _mappedKeys;

    public MappedKeysProcessor(InputKey inputKey, AppState appState, IEnumerable<MappedKey> mappedKeys)
    {
      _inputKey = inputKey;
      _appState = appState;
      _mappedKeys = mappedKeys;
    }
    public KeysEngineResult Process()
    {
      if (CanProcessNormalMode())
      {
        return GetMappedKeyNormal();
      }
      //return GetMappedKeyNormalAndInsertWithCapital();
    }

    private MappedKey GetMappedKeyNormalAndInsertWithCapital()
    {
      if (!_appState.modificators.caps) return null;
      return GetMappedKey();
      //match appState.state with
      //      | State.Off -> EmptyMappedKey
      //      | _ -> match appState.modificators.caps with
      //             | false -> EmptyMappedKey
      //             | _ -> GetMappedKey true (appState.firstStep + key) keys

    }

    private string GetTrigger() => _appState.firstStep + _inputKey.key;

    private MappedKey GetMappedKey()
    {
      if (_appState.state == State.Off) return null;
      var trigger = GetTrigger();
      return _mappedKeys.FirstOrDefault(key => key.isCaps == _appState.modificators.caps && key.trigger == trigger);
    }

    private bool CanProcessNormalMode()
    {
      if (_appState.state != State.Normal) return false;
      if (_appState.modificators.win) return false;
      return true;
    }

    private MappedKey GetMappedKeyNormal()
    {
      if (IsDownFirstStep()) return null;
      return GetMappedKey();
    }

    private bool IsDownFirstStep() => _appState.firstStep == "" && DomainUtils.IsTwoStep(_inputKey.key);


    private KeysEngineResult ProcessNormalMode()
    {
      var isDownFirstStep = _appState.firstStep == "" && DomainUtils.IsTwoStep(_inputKey.key);
      var nextFirstStep = isDownFirstStep ? _inputKey.key : "";
      var mappedKey = GetMappedKeyNormal();
      var preventKeyProcess = _inputKey.isLetterOrDigit || mappedKey.send != "";
      var nextAppState = new AppState
      {
        state = _appState.state,
        modificators = _appState.modificators,
        firstStep = nextFirstStep,
        preventEscOnCapsUp = _appState.preventEscOnCapsUp
      };
      return new KeysEngineResult
      {
        appState = nextAppState,
        send = mappedKey.send,
        preventKeyProcess = preventKeyProcess
      };
    }

  //  let ProcessNormalMode appState inputKey keys =
  //      let isDownFirstStep = appState.firstStep = "" && IsTwoStep inputKey.key
  //      let firstStepNext = if isDownFirstStep then inputKey.key else ""
  //      let sendDoKey = GetMappedKeyNormal appState.firstStep inputKey.key keys
  //      let preventKeyProcess = inputKey.isLetterOrDigit || sendDoKey.send <> ""
  //      let nextAppState = { state = appState.state; modificators = appState.modificators; firstStep = firstStepNext; preventEscOnCapsUp = appState.preventEscOnCapsUp}
  //      {appState = nextAppState ; send = sendDoKey.send ; preventKeyProcess = preventKeyProcess
  //}

  //let GetMappedKey isCaps(trigger:string) keys =
  //    let predicate(mappedKey:MappedKey) = mappedKey.trigger = trigger.ToLower() && mappedKey.isCaps = isCaps
  //   let result = Seq.tryFind predicate keys
  //   match result with
  //        | Some i -> i
  //        | None -> EmptyMappedKey



  //let GetMappedKeyNormalAndInsertWithCapital appState key keys =
  //      match appState.state with
  //          | State.Off -> EmptyMappedKey
  //          | _ -> match appState.modificators.caps with
  //                 | false -> EmptyMappedKey
  //                 | _ -> GetMappedKey true (appState.firstStep + key) keys

  //let GetNextAppStateByStateChange key appState =
  //    let nextState = Domain.GetNextStateByKey key appState.state
  //    match nextState with
  //        | None -> None
  //        | _-> Some { state = nextState.Value; modificators = appState.modificators; firstStep = ""; preventEscOnCapsUp = true}

  //let GetNextAppStateByESC inputKey appState =
  //    match inputKey.isEsc with
  //        | false -> None
  //        | _ -> match appState.state with
  //                | State.Insert -> Some {
  //  state = GetPrevState appState.state; modificators = appState.modificators;
  //  firstStep = ""; preventEscOnCapsUp = appState.preventEscOnCapsUp}
  //                | _ -> None


  //let CanProcessNormalMode appState =
  //    match appState.state with
  //        | State.Normal -> match appState.modificators.win with
  //                            | true -> false 
  //                            | _ -> true
  //        | _ -> false



  //let ProcessKey appState inputKey keys =
  //      if CanProcessNormalMode appState
  //      then ProcessNormalMode appState inputKey keys
  //      else {appState = appState; send = ""; preventKeyProcess = false}


  }
}
