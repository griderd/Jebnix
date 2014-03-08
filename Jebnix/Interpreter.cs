using System;
using UnityEngine;
using Jebnix.FileSystem;

namespace Jebnix
{
    public abstract class Interpreter
    {
        public Vessel vessel;

        public Interpreter(System.IO.DirectoryInfo archiveFolder)
        {
            Archive = new Folder("Archive", null, archiveFolder);
            RootFolder = new Folder("Root");
            CurrentFolder = Archive;
            Functions.RegisterStandardLibrary();
        }

        public Interpreter(Vessel vessel, System.IO.DirectoryInfo archiveFolder)
            : this(archiveFolder)
        {
            this.vessel = vessel;
        }

        public abstract int CreateProcess(string file);

        public abstract int CreateProcess(string[] lines, string filename);
        public abstract void ExecuteProcess();
        public abstract bool HasVariable(string name);
        public abstract void MemoryDump();
        public abstract string GetInterpreterVersion();

        public Folder Archive;
        public Folder RootFolder;
        public Folder CurrentFolder;
    }
}