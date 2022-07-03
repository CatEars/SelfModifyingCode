# SelfModifyingCode

Easy self-updating programs with .NET Core

## Glossary

* SelfModifyingCode (SMC) - The project you are currently viewing. 
* Base Application - The application that will be self-updating with the help of SMC.
* Host Application - The SMC-provided wrapper for your application.
* SMC Manifest - Manifest declaring a specific SMC Program with application name, version, etc.
* SMC Program - A zip-archive with a manifest and corresponding Base Application.
* SMC Registry / SMC Server - A web registry / web server that serves SMC programs.

## I Just Want To Use It! - Show me how

1. Package your executable in some way. A folder with all necessary files is enough. Lets call this the exe-folder.
2. Create a manifest project with `SMCManifest` in the name. The Visual Studio/Rider IDE Class Library template is a good option.
3. Install the NuGet `SelfModifyingCode` into your manifest project.
4. Create a single class `Manifest` that implements `ISelfModifyingCodeManifest`.
   * Tip: Use `SingleExeLocator<Manifest>.FromRelativePath` and point to your exe-folder to easily package your code.  
5. Copy your exe-folder into the manifest project, right-click > properties and make sure it is always copied on build.
6. Publish your manifest project.
7. Pack the application into an SMC program using the Host Application with the `--pack` flag.
8. Publish your SMC program to the SMC Registry you specified in the manifest.
9. Run your SMC program with the SMC host application and watch the magic happen when you upload the next version of your app to the registry.

## How it works

The SMC Host application will unzip the SMC Program, find an SMC Manifest by looking for a dll 
with the name "SMCManifest" in it. It will check the manifest for an executable, and relevant
information to run the executable. Then it will start the executable, while also periodically
checking for new versions. The manifest points out the SMC Registry that the SMC Program was 
published to and the Host Application will search for updates found on the specified SMC Registry.
