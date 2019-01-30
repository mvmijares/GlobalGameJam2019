using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using InControl;

public enum JoystickDirection { None, Clockwise, CounterClockwise }

public class LechonMinigame : MonoBehaviour {
    GameManager _gameManager;
    [SerializeField] private Vector2 input;


    public JoystickCombo clockwiseCombo;
    public JoystickCombo counterClockwiseCombo;

    public JoystickDirection joystickDirection;
    public JoystickDirection pigDirection;
    public float waitForNextComboIteration = 0.2f;
    private bool waitForNextCombo = false;
    private bool checkCombo = false;
    public float comboTimeLength;
    private float currentComboTime;

    private float waitForNextChoice;
    public float nextChoiceTime = 2f;
    public Camera lechonCamera;

    public void OnLookEventCalled(Vector2 look) { input = look; }

    public Transform pig;
    bool spinPig = false;
    public float rotationSpeed = 3;
    float angle;

    private LechonUserInterface lechonUserInterface;

    public int maxCircleCombo = 10;
    [SerializeField] int currentCircleCombo = 0;

    public float waitTimeForDirection;
    private float currTimeForDirection;
    private bool isWaiting;

    public void InitializeLechonMinigame(GameManager gameManager) {
        _gameManager = gameManager;
       
        clockwiseCombo = new JoystickCombo("Clockwise");
        clockwiseCombo.lowerDeadZoneX = -.2f;
        clockwiseCombo.upperDeadZoneX = .2f;
        clockwiseCombo.lowerDeadZoneY = -.2f;
        clockwiseCombo.upperDeadZoneY = .2f;

        counterClockwiseCombo = new JoystickCombo("CounterClockwise");
        counterClockwiseCombo.lowerDeadZoneX = -.2f;
        counterClockwiseCombo.upperDeadZoneX = .2f;
        counterClockwiseCombo.lowerDeadZoneY = -.2f;
        counterClockwiseCombo.upperDeadZoneY = .2f;

        joystickDirection = JoystickDirection.None;

        checkCombo = false;

        lechonCamera.gameObject.SetActive(false);

        lechonUserInterface = FindObjectOfType<LechonUserInterface>();
        if (lechonUserInterface)
            lechonUserInterface.InitializeLechonUserInterface(gameManager);

        isWaiting = false;
        _gameManager.GetPlayer().playerInput.OnLookEvent += OnLookEventCalled;
    }
    public void SetGameplayCamera(bool condition) {
        lechonCamera.gameObject.SetActive(condition);
    }
    private void OnDestroy() {
        _gameManager.GetPlayer().playerInput.OnLookEvent -= OnLookEventCalled;
    }
    public void CloseMiniGame() {
        lechonUserInterface.directionalKey.gameObject.SetActive(false);
    }
    public void UpdateLechonMinigame() {
        if(joystickDirection == JoystickDirection.None) {
            if (!isWaiting) { 
                int choice = Random.Range(-1, 2);
                if(choice == 1) {
                    joystickDirection = JoystickDirection.Clockwise;
                    lechonUserInterface.SetImageBasedOnDirection(joystickDirection);
                } else {
                    joystickDirection = JoystickDirection.CounterClockwise;
                    lechonUserInterface.SetImageBasedOnDirection(joystickDirection);
                }
                checkCombo = true;
            } else {
                lechonUserInterface.ResetImage();
                currTimeForDirection += Time.deltaTime;
                if (currTimeForDirection >= waitTimeForDirection) {
                    currTimeForDirection = 0.0f;
                    isWaiting = false;
                }
            }
        } else {
            if (checkCombo) {
                if (!lechonUserInterface.GetPlayAnimationState()) {
                    lechonUserInterface.SetPlayCondition(true);
                    lechonUserInterface.SetDoneCondition(false);
                }
                lechonUserInterface.SetImageCondition(true);
                currentComboTime += Time.deltaTime;
                bool condition = false;

                while (!condition)
                    condition = CheckForCombo(currentComboTime);

                if (condition) {
                    if (joystickDirection == JoystickDirection.Clockwise) {
                        if (clockwiseCombo.comboCompleted) {
                            currentCircleCombo++;
                            _gameManager.playerScore++;
                            checkCombo = false;
                            spinPig = true;
                            pigDirection = JoystickDirection.Clockwise;
                            isWaiting = true;

                            lechonUserInterface.SetImageCondition(false);
                            lechonUserInterface.ResetImage();

                        }
                    } else if(joystickDirection == JoystickDirection.CounterClockwise) {
                        if (counterClockwiseCombo.comboCompleted) {
                            currentCircleCombo++;
                            _gameManager.playerScore++;
                            checkCombo = false;
                            spinPig = true;
                            lechonUserInterface.SetImageCondition(false);
                            pigDirection = JoystickDirection.CounterClockwise;
                            isWaiting = true;
                            lechonUserInterface.SetImageCondition(false);
                        }
                    }
                }
                if (currentComboTime >= comboTimeLength) {
                    checkCombo = false;
                    currentComboTime = 0f;
                    Debug.Log("Took too long!");
                }
            } else {
                clockwiseCombo.index = 0;
                clockwiseCombo.comboCompleted = false;

                counterClockwiseCombo.index = 0;
                counterClockwiseCombo.comboCompleted = false;
                
                currentComboTime = 0f;

                waitForNextChoice += Time.deltaTime;

                lechonUserInterface.SetDoneCondition(true);
                lechonUserInterface.SetPlayCondition(false);

                if (waitForNextChoice >= nextChoiceTime) {
                    joystickDirection = JoystickDirection.None;
                    waitForNextChoice = 0.0f;
                }
            }
        }
        if (spinPig) {
            int direction = 1;
            if (pigDirection == JoystickDirection.Clockwise)
                direction = 1;
            else if (pigDirection == JoystickDirection.CounterClockwise)
                direction = -1;

            angle += 20 * Time.deltaTime * rotationSpeed;

            if (angle >= 360) {
                spinPig = false;
                angle = 0;
            }
            pig.RotateAround(pig.transform.position, pig.transform.up * direction, 20 * Time.deltaTime * rotationSpeed);
        }
        if(currentCircleCombo >= maxCircleCombo) {
            _gameManager.switchScreen = true;
            _gameManager.credits = true;
            
            CloseMiniGame();
        }
    }
    bool CheckForCombo(float time) {
        if (input.x == 0 && input.y == 0)
            return true;

        if (joystickDirection == JoystickDirection.Clockwise) {
            if (clockwiseCombo.CheckInput(input)) {
                if (clockwiseCombo.index >= clockwiseCombo.GetComboLength() - 1) {
                    clockwiseCombo.comboCompleted = true;
                    Debug.Log("Combo Completed");
                    return true;
                } else {
                    Debug.Log("Combo Index is " + clockwiseCombo.index);
                    clockwiseCombo.index++;
                    return false;
                }
            } else {
                return true;
            }
        }else if(joystickDirection == JoystickDirection.CounterClockwise) {

            if (counterClockwiseCombo.CheckInput(input)) {
                if (counterClockwiseCombo.index >= counterClockwiseCombo.GetComboLength() - 1) {
                    counterClockwiseCombo.comboCompleted = true;
                    Debug.Log("Combo Completed");
                    return true;
                } else {
                    Debug.Log("Combo Index is " + counterClockwiseCombo.index);
                    counterClockwiseCombo.index++;
                    return false;
                }
            } else {
                return true;
            }
        } else {
            return true;
        }
    }


}
