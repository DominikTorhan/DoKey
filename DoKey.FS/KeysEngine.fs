module KeysEngine

open DoKey.FS

let Perform(appState:AppState, inputKey: InputKey, configuration: Configuration) =
    appState


//let PerformModificators(appState:AppState) =
//    let modif = appState.Modificators.GetNextModificators(null, "")
//    let appState' = {appState with modificators = modif }
//    appState'

    //private cOutput ProcessModificators()
    //{

    //  var modificators = this.modificators.GetNextModificators(inputKey, isUp);

    //  return new cOutput
    //  {
    //    AppState = new AppState(this.AppState.State, modificators, this.AppState.FirstStep, this.AppState.PreventEscOnCapsUp)
    //  };

    //}