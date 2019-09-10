module AppStatesFactory

open DoKey.FS
open DoKey.FS.Domain

let AppStateOff =
    new AppState()  

let AppStateNormal =
     new AppState(State.Normal, new Modificators(), "", false)