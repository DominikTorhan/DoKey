namespace DoKey.Core

open System
open System.Runtime.InteropServices
open System.ComponentModel;

[<type:StructLayout(LayoutKind.Sequential)>]
type LowLevelKeyboardInputEvent =
    struct
        val VirtualCode: int
        val HardwareScanCode: int
        val Flags: int
        val TimeStamp: int
        val AdditionalInformation: IntPtr
    end


type KeyboardState =
      | KeyDown = 0x0100
      | KeyUp = 0x0101
      | SysKeyDown = 0x0104
      | SysKeyUp = 0x0105

type KeyboardHookEvent(data:LowLevelKeyboardInputEvent, state:KeyboardState) = 
    inherit HandledEventArgs()
    member this.KeyboardData = data
    member this.KeyboardState = state
    member this.IsUp = match state with
                        | KeyboardState.KeyUp -> true
                        | KeyboardState.SysKeyUp -> true
                        | _ -> false

  //      class cKeyboardHookEvent : HandledEventArgs {
  //  public KeyboardState KeyboardState { get; private set; }
  //  public LowLevelKeyboardInputEvent KeyboardData { get; private set; }

  //  public cKeyboardHookEvent(
  //      LowLevelKeyboardInputEvent keyboardData,
  //      KeyboardState keyboardState) {
  //    KeyboardData = keyboardData;
  //    KeyboardState = keyboardState;
  //  }
  //}