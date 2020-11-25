using UnityEngine;

namespace ControllerVisualizer
{
    public class Gamepad : MonoBehaviour
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
            foreach (InputVisualizer visualizer in visualizers)
            {
                visualizer.m_ControlIndex = this.m_ControlIndex;
            }
        }
    }
}