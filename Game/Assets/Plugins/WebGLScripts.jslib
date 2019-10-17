mergeInto(LibraryManager.library, {

  WebGLExit: function () {
    document.write(
	"<html>" +
	"	<head></head>" +
	'	<body style="background-color: #0b486b;">' +
	'		<p style="text-align: center;font-family: sans-serif;font-weight: bold;background-color: #e0e0e0;font-style: italic;text-transform: uppercase;color: #314a5d;width: 300px;margin: 100px auto;padding: 100px;font-size: 24px;box-shadow: 0px 4px 8px rgba(0,0,0,0.5);border-radius: 4px;">' +
	"			Thanks for playing!" +
	"		</p>" +
	"	</body>" +
	"</html>"
	);
  },

});