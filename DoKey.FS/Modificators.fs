namespace DoKey.FS

type Modificators(alt:bool, control:bool, shift:bool, win:bool, caps:bool) =  
    new () = Modificators(false,false,false,false,false) 
    new (str:string) =  
        let func = str.Contains
        Modificators(func("%"), func("^"), func("+"), func("w"), func("c"))
    member this.Alt = alt
    member this.Control = control 
    member this.Shift = shift
    member this.Win = win 
    member this.Caps = caps 
    member this.ToStr =    if alt then "%" else "" 
                         + if control then "^" else "" 
                         + if shift then "+" else "" 
                         + if win then "w" else "" 
                         + if caps then "c" else ""
    member this.ToLog = "mod: " + if alt then " alt " else "" 
                                + if control then " ctrl " else "" 
                                + if shift then " shift " else "" 
                                + if win then " win " else "" 
                                + if caps then " caps " else ""

    
    member this.GetNextModificators(input: InputKey, isUp:bool)= 
        let alt = match input.IsAlt with | true -> not isUp | false -> this.Alt
        let con = match input.IsControl with | true -> not isUp | false -> this.Control
        let sht = match input.IsShift with | true -> not isUp | false -> this.Shift
        let win = match input.IsWin with | true -> not isUp | false -> this.Win
        let cap = match input.IsCapital with | true -> not isUp | false -> this.Caps
        new Modificators(alt, con, sht, win, cap)
    