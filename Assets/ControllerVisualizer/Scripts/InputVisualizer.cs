using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


namespace ControllerVisualizer
{
    [SelectionBase]
    public class InputVisualizer : MonoBehaviour
    {
        [Tooltip("Specifies which color the material should become when used.")]
        [SerializeField] private Color activatedColor = Color.white;

        [Tooltip("If multiple controls match 'Control Path' at runtime, this property decides "
            + "which control to visualize from the list of candidates. It is a zero-based index.")]
        public int deviceId;

        [SerializeField] private InputAction press = null;
        [SerializeField][Range(1, 2)] private float bigger = 1;

        [SerializeField] private InputAction stick = null;
        [SerializeField][Range(0, 3)] private float range = 0;

        [SerializeField] private InputAction trigger = null;
        private Slider slider;

        private Vector2 parentPos;

        void OnEnable()
        {
            if (press != null) press.Enable();
            if (stick != null) stick.Enable();
            if (trigger != null) {
                trigger.Enable();
                slider = GetComponent<Slider>();
            }
        }

        private void OnDisable() {
            if (press != null) press.Disable();
            if (stick != null) stick.Disable();
        }

        private void Awake() {
            if (press != null)
            {
                press.performed += ctx => OnPressPerformed(ctx);
                press.canceled += ctx => OnPressCanceled();
            }
            if (stick != null)
            {
                stick.performed += ctx => OnStickPerformed(ctx.ReadValue<Vector2>());
                stick.canceled += ctx => OnStickPerformed(ctx.ReadValue<Vector2>());
            }
            if (trigger != null)
            {
                trigger.performed += ctx => OnTriggerPerformed(ctx.ReadValue<float>());
                trigger.canceled += ctx => OnTriggerPerformed(ctx.ReadValue<float>());
            }
        }

        private void OnPressPerformed(InputAction.CallbackContext ctx)
        {
            if (press.activeControl.device.deviceId != deviceId) return;
            SwapColor();
            this.transform.localScale = this.transform.localScale * bigger;
        }

        private void OnPressCanceled()
        {
            if (press.activeControl.device.deviceId != deviceId) return;
            SwapColor();
            this.transform.localScale = this.transform.localScale / bigger;   
        }

        private void OnTriggerPerformed(float value)
        {
            if (trigger.activeControl.device.deviceId != deviceId) return;
            this.slider.value = value;
        }

        private void SwapColor()
        {
            SpriteRenderer spr = GetComponent<SpriteRenderer>();
            Color color = activatedColor;
            activatedColor = spr.color;
            spr.color = color;
        }
    
        private void OnStickPerformed(Vector2 value)
        {
            if (stick.activeControl.device.deviceId != deviceId) return;
            Vector3 position = this.transform.parent.position;
            this.transform.position = new Vector3(position.x + (value.x * range * transform.parent.parent.localScale.x), position.y + (value.y * range * transform.parent.parent.localScale.x), position.z);
        }

        private void OnDrawGizmosSelected() {
            if(range == 0) return;
            Gizmos.DrawWireSphere(transform.position, range * transform.parent.parent.localScale.x);
        }
    }
}
