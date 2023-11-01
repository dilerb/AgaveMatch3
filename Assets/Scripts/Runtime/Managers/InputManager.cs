using Runtime.Controllers;
using Runtime.Data.UnityObjects;
using Runtime.Data.ValueObjects;
using Runtime.Keys;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Managers
{
    public class InputManager: MonoBehaviour
    {
        [SerializeField] private InputController inputController;
        
        private InputData _inputData;
        private bool _isAvailableForTouch;
        private void OnEnable() => SubscribeEvents();
        private void OnDisable() => UnSubscribeEvents();

        private void Awake()
        {
            SetInputData();
            SendDataToController();
        }
        private void SetInputData() =>  _inputData = Resources.Load<CD_Input>($"Data/CD_Input").Data;
        private void SendDataToController() => inputController.SetInputData(_inputData);
        private void SendTouchAvailability() => inputController.SetTouchAvailability(_isAvailableForTouch);
        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.OnGameStart += DisableInput;
            CoreGameSignals.Instance.OnBoardCreated += EnableInput;
            CoreGameSignals.Instance.OnMatchCompleted += EnableInput;
            CoreGameSignals.Instance.OnMatchFailed += EnableInput;
            CoreGameSignals.Instance.OnReset += OnReset;
            InputSignals.Instance.OnInputEnable += EnableInput;
            InputSignals.Instance.OnInputDisable += DisableInput;
            InputSignals.Instance.OnInputTaken += InputTaken;
        }

        private void UnSubscribeEvents()
        {
            CoreGameSignals.Instance.OnGameStart -= DisableInput;
            CoreGameSignals.Instance.OnBoardCreated -= EnableInput;
            CoreGameSignals.Instance.OnMatchCompleted -= EnableInput;
            CoreGameSignals.Instance.OnMatchFailed -= EnableInput;
            CoreGameSignals.Instance.OnReset -= OnReset;
            InputSignals.Instance.OnInputEnable -= EnableInput;
            InputSignals.Instance.OnInputDisable -= DisableInput;
            InputSignals.Instance.OnInputTaken -= InputTaken;
        }

        private void InputTaken(MatchInfoParams matchInfos)
        {
            CoreGameSignals.Instance.OnMatchInfosTaken?.Invoke(matchInfos);
        }
        private void EnableInput()
        {
            _isAvailableForTouch = true;
            SendTouchAvailability();
        }

        private void DisableInput()
        {
            _isAvailableForTouch = false;
            SendTouchAvailability();
        }

        private void OnReset()
        {
            DisableInput();
        }
    }
}