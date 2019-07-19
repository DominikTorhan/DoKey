module AppStatesFactory

open DoKey.FS

let AppStateOff =
    new AppState()  

let AppStateNormal =
     new AppState(State.Normal, new Modificators(), "", false, false)