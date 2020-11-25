using UnityEngine;
using UnityEngine.InputSystem;

namespace ControllerVisualizer
{
    public class GamepadVisualizer : MonoBehaviour
    {
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
        }

        private void ResolveControl()
        {
            InputVisualizer[] visualizers = GetComponentsInChildren<InputVisualizer>();
            var deviceId = Gamepad.all[m_ControlIndex].deviceId;
            foreach (InputVisualizer visualizer in visualizers)
            {
                visualizer.deviceId = deviceId;
            }
        }
    }
}