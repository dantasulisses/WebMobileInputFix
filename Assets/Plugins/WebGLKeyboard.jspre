Module["WebGLKeyboard"] = {};

Module.WebGLKeyboard.InjectWebGLKeyboardElements = function () {
    var formElement = document.createElement("form");
    formElement.id = "keyboard_input_form";
    formElement.style.width;
    formElement.autocomplete = "off";
    formElement.style = "font-size: 42px; position: fixed; top: -999px; left: -999px; visibility: hidden;";
    document.body.appendChild(formElement);
    Module.WebGLKeyboard.formElement = formElement;
    
    var inputElement = document.createElement("input");
    inputElement.type = "text";
    inputElement.id = "text_input_field";
    inputElement.onclick = function() { document.getElementById("text_input_field").focus(); }
    formElement.appendChild(inputElement);
    Module.WebGLKeyboard.inputElement = inputElement;

    inputElement.oninput = function() {
      SendMessage('_WebGLKeyboard', 'ReceiveInputChange', Module.WebGLKeyboard.inputElement.value);
    };

    inputElement.onblur = function() {
      SendMessage('_WebGLKeyboard', 'LoseFocus');
    };

    formElement.onsubmit = function(event) {
      inputElement.blur();
      event.preventDefault();
    };
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
