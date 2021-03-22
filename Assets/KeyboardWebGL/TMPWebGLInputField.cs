using UnityEngine;
using UnityEngine.EventSystems;
#if USE_TMPRO
using TMPro;
#endif

namespace WebGLKeyboard
{
#if USE_TMPRO
    [RequireComponent(typeof(TMP_InputField))]
    /// <summary>
    /// Attach this to a GameObject with TMP_InputField to detect input in WebGL
    /// </summary>
    public class TMPWebGLInputField : BaseWebGLInputField, IDeselectHandler
    {
        private TMP_InputField inputField;

        private TMP_InputField Input
        {
            get
            {
                return inputField ?? (inputField = GetComponent<TMP_InputField>());
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
#else
    public class TMPWebGLInputField : BaseDetectInputField, IDeselectHandler
    {
        [Header("Use this component with TextMesh Pro InputField")]

        public override string Text { get; set; }

        public override int CharacterLimit { get; set; }

        public override int CaretPosition { get; set; }

        public override int SelectionStart => 0;
        
        public override int SelectionEnd => 0;
    }
#endif
}
