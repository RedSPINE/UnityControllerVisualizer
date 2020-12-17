using UnityEngine;
using UnityEngine.InputSystem;

namespace ControllerVisualizer
{
    public class GamepadVisualizer : MonoBehaviour
    {
        [SerializeField] private GameObject unboundCache = null;
        [SerializeField] private Animator centralLight = null;

        private InputVisualizer[] visualizers;

        public int controlIndex
        {
            get => m_ControlIndex;
            set
            {
                m_ControlIndex = value;
                ResolveControl();
            }
        }
        [Tooltip("If multiple controls match 'Control Path' at runtime, this property decides "
            + "which control to visualize from the list of candidates. It is a zero-based index.")]
        [SerializeField] private int m_ControlIndex;

        private void OnEnable() {
            visualizers = GetComponentsInChildren<InputVisualizer>();
            ResolveControl();
            InputSystem.onDeviceChange += (device, change) => ResolveControl();
        }

        private void ResolveControl()
        {
            var gamepads = Gamepad.all;

            if (gamepads.Count <= this.controlIndex) {
                if (!this.unboundCache.activeSelf) Debug.Log($"Controller {this.controlIndex+1} disconnected.");
                unboundCache?.SetActive(true);
                return;
            }

            var deviceId = gamepads[m_ControlIndex].deviceId;
            if (unboundCache.activeSelf) 
            {
                centralLight.SetTrigger("Flash");
                unboundCache?.SetActive(false);
                Debug.Log($"Controller {this.controlIndex+1} connected.");        
            }
            foreach (InputVisualizer visualizer in visualizers)
                visualizer.deviceId = deviceId;
        }
    }
}