using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using InControl;

public class JoystickInput : PlayerActionSet {

    public PlayerAction MoveUp;
    public PlayerAction MoveDown;
    public PlayerAction MoveLeft;
    public PlayerAction MoveRight;
    public PlayerOneAxisAction MoveVertical;
    public PlayerOneAxisAction MoveHorizontal;
    public PlayerTwoAxisAction Movement;

    public PlayerAction LookUp;
    public PlayerAction LookDown;
    public PlayerAction LookLeft;
    public PlayerAction LookRight;
    public PlayerOneAxisAction LookVertical;
    public PlayerOneAxisAction LookHorizontal;
    public PlayerTwoAxisAction Look;

    public PlayerAction ActionButton;
    public PlayerAction Start;

    public JoystickInput() {
        MoveUp = CreatePlayerAction("Move Up");
        MoveDown = CreatePlayerAction("Move Down");
        MoveLeft = CreatePlayerAction("Move Left");
        MoveRight = CreatePlayerAction("Move Right");

        MoveVertical = CreateOneAxisPlayerAction(MoveDown, MoveUp);
        MoveHorizontal = CreateOneAxisPlayerAction(MoveLeft, MoveRight);
        Movement = CreateTwoAxisPlayerAction(MoveLeft, MoveRight, MoveDown, MoveUp);

        LookUp = CreatePlayerAction("Look Up");
        LookDown = CreatePlayerAction("Look Down");
        LookLeft = CreatePlayerAction("Look Left");
        LookRight = CreatePlayerAction("Look Right");

        LookVertical = CreateOneAxisPlayerAction(LookDown, LookUp);
        LookHorizontal = CreateOneAxisPlayerAction(LookLeft, LookRight);
        Look = CreateTwoAxisPlayerAction(LookLeft, LookRight, LookDown, LookUp);

        ActionButton = CreatePlayerAction("Action Button");
        Start = CreatePlayerAction("Start");

    }

    public JoystickInput CreateDefaultJoystickBindings() {
        JoystickInput newJoystickInput = new JoystickInput();

        newJoystickInput.MoveUp.AddDefaultBinding(InputControlType.LeftStickUp);
        newJoystickInput.MoveDown.AddDefaultBinding(InputControlType.LeftStickDown);
        newJoystickInput.MoveLeft.AddDefaultBinding(InputControlType.LeftStickLeft);
        newJoystickInput.MoveRight.AddDefaultBinding(InputControlType.LeftStickRight);

        newJoystickInput.LookUp.AddDefaultBinding(InputControlType.RightStickUp);
        newJoystickInput.LookDown.AddDefaultBinding(InputControlType.RightStickDown);
        newJoystickInput.LookLeft.AddDefaultBinding(InputControlType.RightStickLeft);
        newJoystickInput.LookRight.AddDefaultBinding(InputControlType.RightStickRight);

        newJoystickInput.ActionButton.AddDefaultBinding(InputControlType.Action1);
        newJoystickInput.Start.AddDefaultBinding(InputControlType.Command);


        newJoystickInput.ListenOptions.OnBindingFound = (action, binding) => {
            if (binding == new KeyBindingSource(Key.Escape)) {
                action.StopListeningForBinding();
                return false;
            }
            return true;
        };

        return newJoystickInput;

    }
}