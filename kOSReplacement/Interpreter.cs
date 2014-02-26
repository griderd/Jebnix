using System;
namespace Jebnix
{
    public abstract class Interpreter
    {
        protected Computer ComputerPart;

        public void SetComputer(Computer c)
        {
            ComputerPart = c;
        }

        public bool CreateProcess(string file);
        
        bool CreateProcess(string[] lines, string filename);
        void ExecuteProcess();
        bool HasVariable(string name);
        void MemoryDump();
        string GetInterpreterVersion();
    }
}
