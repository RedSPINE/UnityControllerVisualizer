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
                ResolveControls();
            }
        }

        [Tooltip("If multiple controls match 'Control Path' at runtime, this property decides "
            + "which control to visualize from the list of candidates. It is a zero-based index.")]
        [SerializeField] private int m_ControlIndex;

        private void OnEnable() {
            visualizers = GetComponentsInChildren<InputVisualizer>();
        }

        // Activate and match controls if it matches a gamepad
        public void ResolveControls()
        {
            var gamepad = ControllerManager.Instance.gamepads[m_ControlIndex];
            // No gamepad affiliated
            if (gamepad == null)
            {
                unboundCache.SetActive(true);
                UpdateVisualizers(0);
                return;
            }
            // found matching gamepad
            var deviceId = gamepad.deviceId;
            if (unboundCache.activeSelf)
                Activate();
            UpdateVisualizers(deviceId);
        }

        // Plays a little animation when connecting
        private void Activate()
        {
            centralLight.SetTrigger("Flash");
            unboundCache?.SetActive(false);
        }

        // Goes throughout all child visualizers to set their tracked device
        private void UpdateVisualizers(int deviceId)
        {
            foreach (InputVisualizer visualizer in visualizers)
                visualizer.deviceId = deviceId;
        }
    }
}