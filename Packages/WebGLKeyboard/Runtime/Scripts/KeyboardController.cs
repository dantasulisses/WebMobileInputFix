using AOT;
using System.Runtime.InteropServices;
using UnityEngine;

namespace WebGLKeyboard
{
    /// <summary>
    /// Controls the flow of opening the keyboard and adding the necessary components to input fields as scenes load
    /// </summary>
    public class KeyboardController : MonoBehaviour
    {
        private static bool quitting;

        private static KeyboardController instance;

        public static KeyboardController Instance
        {
            get
            {
                if (instance == null && !quitting)
                {
                    instance = FindObjectOfType<KeyboardController>();
                    if (!instance)
                    {
                        instance = new GameObject("_WebGLKeyboard").AddComponent<KeyboardController>();
                    }
                }
                return instance;
            }
        }

        private void OnApplicationQuit()
        {
            quitting = true;
        }

        private BaseWebGLInputField current_handler;

        private int known_keyboard_caret_position;

        private int selection_start, selection_end;

        private const int HTML_INPUT_DEFAULT_MAXLENGTH = 524288;

        internal delegate void DelegateVoidString(string s1);

        internal delegate void DelegateVoidStringInt(string s1, int num1);

        [DllImport("__Internal")]
        private static extern void SetWebGLKeyboardCallbacks(
                                    DelegateVoidString submitInput,
                                    DelegateVoidStringInt updateInput
                                    );

        [DllImport("__Internal")]
        private static extern void OpenInputKeyboard(string currentValue, string type, int characterLimit);

        [DllImport("__Internal")]
        private static extern void CloseInputKeyboard();

        [DllImport("__Internal")]
        private static extern void SetCaretPosition(int pos);

        [DllImport("__Internal")]
        private static extern void SetSelectionInKeyboard(int start, int end);

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(this);
            }
        }

        private void Start()
        {
            gameObject.name = "_WebGLKeyboard";
            gameObject.transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
#if UNITY_WEBGL && !UNITY_EDITOR
            SetWebGLKeyboardCallbacks(SubmitInput, UpdateInput);
#endif
        }

        public void RequestKeyboard(BaseWebGLInputField handler, string initial_text, int characterLimit, KeyboardType type = KeyboardType.text)
        {
            if (current_handler == null || current_handler == handler)
            {
                if (current_handler == null)
                {
                    current_handler = handler;
                    known_keyboard_caret_position = current_handler.CaretPosition;
                    selection_start = Mathf.Min(current_handler.SelectionStart, current_handler.SelectionEnd);
                    selection_end = Mathf.Max(current_handler.SelectionStart, current_handler.SelectionEnd);
                }

#if UNITY_WEBGL && !UNITY_EDITOR
                OpenInputKeyboard(initial_text, type.ToString().ToLower(),
                    characterLimit > 0 ? characterLimit : HTML_INPUT_DEFAULT_MAXLENGTH);
                UnityEngine.WebGLInput.captureAllKeyboardInput = false;
                StartCoroutine(MonitorCaretAndSelection());
#endif
            }
        }

        public void ReleaseKeyboard(BaseWebGLInputField handler)
        {
            if (current_handler == handler)
            {
                current_handler = null;
            }
#if UNITY_WEBGL && !UNITY_EDITOR
            CloseKeyboard();
#endif
        }


#if UNITY_WEBGL && !UNITY_EDITOR
        private System.Collections.IEnumerator MonitorCaretAndSelection()
        {
            yield return new WaitForEndOfFrame();
            while(current_handler!=null)
            {
                var ui_caret_position = current_handler.CaretPosition;
                if (ui_caret_position != known_keyboard_caret_position)
                {
                known_keyboard_caret_position = ui_caret_position;
                SetCaretPosition(ui_caret_position);
                }
                var sel_start = Mathf.Min(current_handler.SelectionStart, current_handler.SelectionEnd);
                var sel_end = Mathf.Max(current_handler.SelectionStart, current_handler.SelectionEnd);
                if (sel_start != selection_start || sel_end != selection_end)
                {
                selection_start = sel_start;
                selection_end = sel_end;
                SetSelectionInKeyboard(selection_start, selection_end);
                }
                yield return new WaitForEndOfFrame();
            }
        }
#endif

        private void CloseKeyboard()
        {
            current_handler = null;
#if UNITY_WEBGL && !UNITY_EDITOR
            CloseInputKeyboard();
            UnityEngine.WebGLInput.captureAllKeyboardInput = true;
#endif
        }

        [MonoPInvokeCallback(typeof(DelegateVoidStringInt))]
        private static void UpdateInput(string new_text, int new_caret_position)
        {
            if (instance.current_handler != null)
            {
                new_text = "" + new_text;
                instance.selection_start = new_caret_position;
                instance.selection_end = new_caret_position;
                instance.known_keyboard_caret_position = new_caret_position;
                instance.current_handler.OnTextChange(new_text, new_caret_position);
            }
        }

        [MonoPInvokeCallback(typeof(DelegateVoidString))]
        private static void SubmitInput(string final_text)
        {
            if (instance.current_handler != null)
            {
                instance.current_handler.OnKeyboardSubmit("" + final_text);
            }
            instance.CloseKeyboard();
        }
    }
}