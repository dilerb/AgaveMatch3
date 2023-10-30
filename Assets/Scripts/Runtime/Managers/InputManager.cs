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
            CoreGameSignals.Instance.onGameStart += DisableInput;
            CoreGameSignals.Instance.onBoardCreated += EnableInput;
            CoreGameSignals.Instance.onReset += OnReset;
            InputSignals.Instance.onInputEnable += EnableInput;
            InputSignals.Instance.onInputDisable += DisableInput;
            InputSignals.Instance.onInputTaken += InputTaken;
        }

        private void UnSubscribeEvents()
        {
            CoreGameSignals.Instance.onGameStart -= DisableInput;
            CoreGameSignals.Instance.onBoardCreated -= EnableInput;
            CoreGameSignals.Instance.onReset -= OnReset;
            InputSignals.Instance.onInputEnable -= EnableInput;
            InputSignals.Instance.onInputDisable -= DisableInput;
            InputSignals.Instance.onInputTaken -= InputTaken;
        }

        private void InputTaken(MatchInfoParams matchInfos)
        {
            CoreGameSignals.Instance.onMatchInfosTaken?.Invoke(matchInfos);
        }
        private void EnableInput()
        {
            Debug.LogWarning("input enabled." + Time.time);
            _isAvailableForTouch = true;
            SendTouchAvailability();
        }

        private void DisableInput()
        {
            Debug.LogWarning("input disabled." + Time.time);
            _isAvailableForTouch = false;
            SendTouchAvailability();
        }

        private void OnReset()
        {
            DisableInput();
        }
    }
}