namespace DoKey.FS

type State = Off | Normal | Insert

type AppState(state:State, modificators:Modificators, firstStep:string, preventAltUp:bool, preventEscOnCapsUp:bool) = 
    member this.State = state  
    member this.Modificators = modificators 
    member this.FirstStep = firstStep 
    member this.PreventAltUp = preventAltUp
    member this.PreventEscOnCapsUp = preventEscOnCapsUp

    member this.ToLog() = state.ToString() + " " + modificators.ToLog + " " + firstStep