namespace DoKey.FS

type State = Off = 0 | Normal = 1 | Insert = 2

type AppState(state:State, modificators:Modificators, firstStep:string, preventEscOnCapsUp:bool) =  
    new () = AppState (State.Off, Modificators(), "", false)
    member this.State = state  
    member this.Modificators = modificators 
    member this.FirstStep = firstStep 
    member this.PreventEscOnCapsUp = preventEscOnCapsUp

    member this.ToLog() = state.ToString() + " " + modificators.ToLog + " " + firstStep