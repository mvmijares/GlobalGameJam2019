using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using InControl;
/// <summary>
/// This class handles all our player input for our game.
/// It will fire off events for input. If other classes need it, just register the input event
/// </summary>

public enum JoystickDirection { Clockwise, CounterClockwise } // Add later
[Serializable]
public class JoystickCombo {
    public string name;
    List<Vector2> joystickPositions;
    public int index;
    public float lowerDeadZoneX;
    public float upperDeadZoneX;

    public float lowerDeadZoneY;
    public float upperDeadZoneY;

    public bool comboCompleted;
    public JoystickCombo(string name) {
        joystickPositions = new List<Vector2>();
        index = 0;
        switch (name) {
            case "Circle": {
                    this.name = name;
                    joystickPositions = CreateCircleInputs();
                    break;
                }
        }
        comboCompleted = false;
    }
    List<Vector2> CreateCircleInputs() {
        List<Vector2> newList = new List<Vector2>();

        newList.Add(new Vector2(0,1));
        newList.Add(new Vector2(-1, 0));
        newList.Add(new Vector2(0, -1));
        newList.Add(new Vector2(1, 0));
        newList.Add(new Vector2(0, 1));

        return newList;
    }
    public bool CheckInput(Vector2 input) {
        bool condition = false;
        if (input.x < joystickPositions[index].x + upperDeadZoneX && input.x > joystickPositions[index].x + lowerDeadZoneX) {
            if (input.y < joystickPositions[index].y + upperDeadZoneX && input.y > joystickPositions[index].y + lowerDeadZoneY) {
                condition = true;
            } else {
                condition = false;
            }
        } else {
            condition = false;
        }
        if (condition)
            return true;
        else
            return false;

    }

    public int GetComboLength() {
        return joystickPositions.Count;
    }
}
public class PlayerInput : MonoBehaviour {

    #region Data
    private GameManager _gameManager;
    private Player _player;
    private JoystickInput input;
    private InputDevice inputDevice;


    //Prototype Section
    public JoystickCombo circleCombo;

    [SerializeField] bool isComboCoroutineRunning;
    [Tooltip("Time for next combo")]
    public float waitForNextComboIteration = 0.2f;
    private bool waitForNextCombo = false;
    private bool checkCombo = false;
    public float comboTimeLength;
    [SerializeField] private float currentComboTime;
    TwoAxisInputControl rightStickInput;
    #endregion
    #region Event Data
    public event Action<Vector2> OnMovementEvent;
    public event Action<Vector2> OnLookEvent;
    public event Action OnActionKeyPressedEvent;

    #endregion
    public void InitializePlayerController(Player player, GameManager gameManager) {
        _player = player;
        _gameManager = gameManager;

        input = new JoystickInput();
        input.CreateDefaultJoystickBindings();

        circleCombo = new JoystickCombo("Circle");
        circleCombo.lowerDeadZoneX = -.2f;
        circleCombo.upperDeadZoneX = .2f;
        circleCombo.lowerDeadZoneY = -.2f;
        circleCombo.upperDeadZoneY = .2f;

        isComboCoroutineRunning = false;
    }

    public void UpdatePlayerInput() {
        rightStickInput = InputManager.ActiveDevice.RightStick;

        if (OnMovementEvent != null)
            OnMovementEvent(InputManager.ActiveDevice.LeftStick);

        if (OnLookEvent != null) {
            OnLookEvent(InputManager.ActiveDevice.RightStick);
        }
      
        if (InputManager.ActiveDevice.Action1.WasReleased) {
            if (OnActionKeyPressedEvent != null)
                OnActionKeyPressedEvent();
        }

        CheckComboState();
    }
    void CheckComboState() {
        if (rightStickInput.X == 0 && rightStickInput.Y == 1) { // Start Combo
            checkCombo = true;
        }
        if (checkCombo) {
            currentComboTime += Time.deltaTime;
            bool condition = false;

            while (!condition) {
                condition = CheckForCombo(currentComboTime);
            }

            if (condition) {
                if (circleCombo.comboCompleted) {
                    _gameManager.playerScore++;
                    checkCombo = false;
                }
            }
            if (currentComboTime >= comboTimeLength) {
                checkCombo = false;
                currentComboTime = 0f;
                Debug.Log("Took too long!");
            }
        } else {
            circleCombo.index = 0;
            circleCombo.comboCompleted = false;
            currentComboTime = 0f;
        }
    }
    bool CheckForCombo(float time) {

        if (rightStickInput.X == 0 && rightStickInput.Y == 0)
            return true;

        if (circleCombo.CheckInput(rightStickInput)) {
            if (circleCombo.index >= circleCombo.GetComboLength() - 1) {
                circleCombo.comboCompleted = true;
                Debug.Log("Combo Completed");
                return true;
            }
            Debug.Log("Index is " + circleCombo.index);
            circleCombo.index++;
            return false;
        } else {
            return true;
        }

    }
}
