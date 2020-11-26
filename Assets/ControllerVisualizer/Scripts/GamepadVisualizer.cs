using UnityEngine;
using UnityEngine.InputSystem;

namespace ControllerVisualizer
{
    public class GamepadVisualizer : MonoBehaviour
    {
        [SerializeField] private GameObject unboundCache = default;

        public int controlIndex
        {
            get => m_ControlIndex;
            set
            {
                m_ControlIndex = value;
                ResolveControl();
            }
        }
        [SerializeField] private int m_ControlIndex;

        private void Awake() {
            ResolveControl();
            InputSystem.onDeviceChange += (device, change) => ResolveControl();
        }

        private void ResolveControl()
        {
            InputVisualizer[] visualizers = GetComponentsInChildren<InputVisualizer>();
            var gamepads = Gamepad.all;
            if (gamepads.Count <= this.controlIndex) {
                unboundCache.SetActive(true);
                return;
            }
            var deviceId = gamepads[m_ControlIndex].deviceId;
            unboundCache.SetActive(false);
            foreach (InputVisualizer visualizer in visualizers)
            {
                visualizer.deviceId = deviceId;
            }
        }
    }
}