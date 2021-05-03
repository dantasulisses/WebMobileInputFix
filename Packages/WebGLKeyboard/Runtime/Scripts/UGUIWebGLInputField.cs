using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace WebGLKeyboard
{
    [RequireComponent(typeof(InputField))]
    /// <summary>
    /// Attach this to a GameObject with InputField to detect input in WebGL
    /// </summary>
    public class UGUIWebGLInputField : BaseWebGLInputField, IDeselectHandler
    {
        private InputField inputField;

        private InputField Input
        {
            get
            {
                return inputField ?? (inputField = GetComponent<InputField>());
            }
        }

        public override string Text
        {
            get
            {
                return Input.text;
            }

            set
            {
                Input.text = value;
            }
        }

        public override void OnTextChange(string text, int caretPosition)
        {
            Input.SetTextWithoutNotify(text);
            CaretPosition = caretPosition;
            if (Input.onValueChanged != null)
            {
                Input.onValueChanged.Invoke(Input.text);
            }
        }

        public override int CharacterLimit
        {
            get
            {
                return Input.characterLimit;
            }

            set
            {
                Input.characterLimit = value;
            }
        }

        public override int CaretPosition
        {
            get
            {
                return Input.caretPosition;
            }

            set
            {
                Input.caretPosition = value;
                Input.selectionAnchorPosition = value;
                Input.selectionFocusPosition = value;
            }
        }

        public override int SelectionStart
        {
            get
            {
                return Input.selectionAnchorPosition;
            }
        }

        public override int SelectionEnd
        {
            get
            {
                return Input.selectionFocusPosition;
            }
        }
    }
}
