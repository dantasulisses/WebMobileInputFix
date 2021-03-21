mergeInto(LibraryManager.library, {

	OpenInputKeyboard: function (currentValue) 
	{
    Module.WebGLKeyboard.formElement.style.visibility = 'visible';
		Module.WebGLKeyboard.inputElement.value = Pointer_stringify(currentValue);
    Module.WebGLKeyboard.AddOpenWebGLKeyboardListeners();
	},
	CloseInputKeyboard: function ()
	{
		Module.WebGLKeyboard.inputElement.blur();
    Module.WebGLKeyboard.formElement.style.visibility = 'hidden';
	}
});