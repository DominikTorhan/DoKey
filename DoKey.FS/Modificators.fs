namespace DoKey.FS

type Modificators(alt:bool, control:bool, shift:bool, win:bool, caps:bool) = 
    member this.Alt = alt
    member this.Control = control 
    member this.Shift = shift
    member this.Win = win 
    member this.Caps = caps 
    member this.ToLog = "mod: " + if alt then " alt " else "" 
                                + if control then " ctrl " else "" 
                                + if shift then " shift " else "" 
                                + if win then " win " else "" 
                                + if caps then " caps " else ""