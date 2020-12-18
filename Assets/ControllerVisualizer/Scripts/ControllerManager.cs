using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ControllerVisualizer {
    public class ControllerManager : MonoBehaviour
    {
        #region Singleton
        private static ControllerManager instance=null;
        public static ControllerManager Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion

        public Gamepad[] gamepads;
        [SerializeField] private int numberOfGamepads = 4;

        private void Start() {
            instance = this;
            gamepads = new Gamepad[numberOfGamepads];
            int i = 0;
            foreach (var gamepad in Gamepad.all)
            {
                gamepads[i] = gamepad;
                i++;
            }
            ResolveVisualizers();
            Log();
        }

        private void OnEnable() {
            InputSystem.onDeviceChange += (device, change) => ResolveControllers(device, change);
        }

        private void ResolveControllers(InputDevice device, InputDeviceChange change)
        {
            if (!(device is Gamepad)) return; // We don’t care about other devices than gamepads
            // Debug.Log($"{device.ToString()} (id:{device.deviceId.ToString()}) has been {change.ToString()}");
            switch (change)
            {
                case InputDeviceChange.Added:
                    AddController((Gamepad)device);
                    break;
                case InputDeviceChange.Removed:
                    RemoveController((Gamepad)device);
                    break;
                default:
                    break;
            }
            // Log();
            ResolveVisualizers();
        }

        private void AddController(Gamepad gamepad)
        {
            for (int i = 0; i < gamepads.Length; i++)
            {
                if (gamepads[i] == null) {
                    gamepads[i] = gamepad;
                    return;
                }
            }
        }

        private void RemoveController(Gamepad gamepad)
        {
            for (int i = 0; i < gamepads.Length; i++)
            {
                if (gamepads[i] == gamepad)
                {
                    gamepads[i] = null;
                    return;
                } 
            }
        }

        // Only for debug purposes
        private void Log()
        {
            string str = "--- CONTROLLERS :\n";
            for (int i = 0; i < gamepads.Length; i++)
            {
                var gamepad = gamepads[i];
                if (gamepad == null)
                {
                    str += $"P{i+1}: no controller assigned\n";
                }
                else
                {
                    str += $"P{i+1}: Device-{gamepad.deviceId}\n";
                }                
            }
            Debug.Log(str);
        }

        private void ResolveVisualizers()
        {
            foreach (GamepadVisualizer gamepadVisualizer in FindObjectsOfType(typeof(GamepadVisualizer)))
            {
                gamepadVisualizer.ResolveControls();
            }
        }

    }
}
