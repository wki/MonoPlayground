# AdobeApp

A simple library which allows to execute JavaScript functions inside an Adobe
Program, e.g. InDesign Server. Currently it only supports OS-X as its platform.

In order to keep things easy and maintainable, JavaScript files which are spread
among a couple of assemblies (and locatable as Resources) are copied into a
temporary directory before execution. This avoids duplication.

A typical JavaScript you write is located in a Directory named "JavaScript" in your
project, ambedded into your assembly's resources and might look like this:

    #include "adobe.js"

    function ensureFontsAreLoaded() {
        ... 
    }

    function ensureImagesAreLinked() {
        ...
    }

    function createProof() {
        ...
    }

From your program written in pure C# you can call all these functions in the 
given order:

    var indesign = new Application("Adobe InDesign CC");
    var render =
    	indesign.Invocation("render.js")
        	.Open("/path/to/x.indd")
        	.EnsureFontsAreLoaded()
        	.EnsureImagesAreLinked()
        	.CreateProof()
        	.Close();

    var result = indesign.Invoke(render);

All parameter handling to and from the JavaScript is handled by this library
and you get the illusion of being able to directly call methods on an object
located on the Adobe App. Also opening and closing documents is handled for
you.