Module["WebGLKeyboard"] = {};

Module.WebGLKeyboard.InjectWebGLKeyboardElements = function () {
    var formElement = document.createElement("form");
    formElement.id = "keyboard_input_form";
    formElement.autocomplete = "off";
    formElement.style = "font-size: 42px; position: fixed; top: -999px; left: -999px; visibility: hidden;";
    document.body.appendChild(formElement);
    Module.WebGLKeyboard.formElement = formElement;

    var inputElement = document.createElement("input");
    inputElement.type = "text";
    inputElement.id = "text_input_field";
    inputElement.style = "font-size: 42px;";
    inputElement.onclick = function () { Module.WebGLKeyboard.inputElement.focus(); }
    formElement.appendChild(inputElement);
    Module.WebGLKeyboard.inputElement = inputElement;

    inputElement.oninput = function () {
        var caretPos = inputElement.selectionStart;
        if (inputElement.type === "number") {
            caretPos = inputElement.value.length;
        }
        if (inputElement.maxlength > 0) {
            if (inputElement.maxlength < inputElement.value.length) {
                inputElement.value = inputElement.value.slice(0, inputElement.maxlength);
            }
            if (caretPos > inputElement.maxlength) {
                caretPos = inputElement.maxlength;
            }
        }
        Module.dynCall_vii(Module.WebGLKeyboard.updateInputPtr, Module.WebGLKeyboard.GetUTF8String(inputElement.value), caretPos);
    };

    inputElement.onblur = function() {
        formElement.style.visibility = 'hidden';
    };

    formElement.onsubmit = function (event) {
        inputElement.blur();
        event.preventDefault();
        Module.dynCall_vi(Module.WebGLKeyboard.submitInputPtr, Module.WebGLKeyboard.GetUTF8String(inputElement.value));
    };
}

Module.WebGLKeyboard.GetUTF8String = function (inputStr) {
    var bufferSize = lengthBytesUTF8(inputStr) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(inputStr, buffer, bufferSize);
    return buffer;
}

Module.WebGLKeyboard.InjectWebGLKeyboardElements();

Module.WebGLKeyboard.AddOpenWebGLKeyboardListeners = function () {
    document.addEventListener('mouseup', Module.WebGLKeyboard.OpenWebGLKeyboard, { once: true });
    document.addEventListener('touchend', Module.WebGLKeyboard.OpenWebGLKeyboard, { once: true });
}

Module.WebGLKeyboard.OpenWebGLKeyboard = function () {
    Module.WebGLKeyboard.inputElement.click();
    Module.WebGLKeyboard.inputElement.focus();
    document.removeEventListener('mouseup', Module.WebGLKeyboard.OpenWebGLKeyboard, { once: true });
    document.removeEventListener('touchend', Module.WebGLKeyboard.OpenWebGLKeyboard, { once: true });
}
