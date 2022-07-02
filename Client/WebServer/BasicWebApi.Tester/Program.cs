// See https://aka.ms/new-console-template for more information

using BasicWebApi.SMCManifest;

Console.WriteLine("Hello, World!");

var manifest = new Manifest();
Console.WriteLine("Exe found at: " + manifest.GetExeLocator().GetExeFileLocation());