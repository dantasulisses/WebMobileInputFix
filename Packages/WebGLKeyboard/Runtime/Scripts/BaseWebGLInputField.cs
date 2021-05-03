using UnityEngine;
using UnityEngine.EventSystems;

namespace WebGLKeyboard
{
    [System.Serializable]
    public enum KeyboardType
    {
        text,
        number,
        tel,
        url,
        email
    }

    public abstract class BaseWebGLInputField : MonoBehaviour, IDeselectHandler, IPointerDownHandler
    {
        public KeyboardType HTMLInputType;

        public abstract string Text { get; set; }

        public abstract int CharacterLimit { get; set; }

        public abstract int CaretPosition { get; set; }

        public abstract int SelectionStart { get; }

        public abstract int SelectionEnd { get; }

        protected bool quitting;
        
        private void OnApplicationQuit()
        {
            quitting = true;
        }

        public void OnDeselect(BaseEventData eventData)
        {
            if (!quitting)
            {
                KeyboardController.Instance.ReleaseKeyboard(this);
            }
        }

        private void OnDisable()
        {
            OnDeselect(null);
        }

        public virtual void OnTextChange(string text, int caretPosition)
        {
            Text = text;
        }

        public void OnKeyboardSubmit(string final_text)
        {
            Text = final_text;
            EventSystem.current.SetSelectedGameObject(null);
        }

        public void OnPointerDown(PointerEventData pointerEventData)
        {
            KeyboardController.Instance.RequestKeyboard(this, Text, CharacterLimit, HTMLInputType);
        }
    }
}
