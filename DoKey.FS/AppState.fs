namespace DoKey.FS
 
open DoKey.FS.Domain


type AppState(state:State, modificators:Modificators, firstStep:string, preventEscOnCapsUp:bool) =  
    new () = AppState (State.Off, Modificators(), "", false)
    member this.State = state  
    member this.Modificators = modificators 
    member this.FirstStep = firstStep 
    member this.PreventEscOnCapsUp = preventEscOnCapsUp

    member this.ToLog() = state.ToString() + " " + modificators.ToLog + " " + firstStep