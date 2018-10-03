# WebMobileInputFix
A Unity WebGL workaround to enable mobile input (virtual keyboard) be triggered by Input Fields

## What this fixes
When you try to interact with any input field in a WebGL application in mobile, the virtual keyboard will never be triggered. This is a fix to that. Also, this can be used to enable copy/paste to the input fields

## How to use
Simply add the "KeyboardController" component to any object in the scene

## How it works
This workaround is a combination of getting inputs natively from a html input field and binds them to a Input Field
KeyboardController.cs is a Singleton that will manage automatically this binding of input fields and will be the bridge between the native javascript calls and the interaction with input fields.
This solution works with TMPro Input fields, just add the DEFINE #USE_TMPRO to your project

If you are using a custom index.html template, check the FixIndexFile.cs script, because it is a post build process to add some necessary entries to the index.html file
(or add this to your index.html manually)

```
<script>
		function FixInputOnSubmit() {
			document.getElementById("fixInput").blur();
			event.preventDefault();
		}
	</script>
    <div>
		<form onsubmit="FixInputOnSubmit()" autocomplete="off" style="width: 0px; height: 0px; position: absolute; top: -9999px;">
			<input type="text" id="fixInput" oninput="gameInstance.Module.ccall('FixInputUpdate', null)" onblur="gameInstance.Module.ccall('FixInputOnBlur', null)" style="font-size: 42px;">
		</form>
	</div>
```

## Known problems
The caret position and text selection will not work properly
