using System;
namespace Jebnix
{
    public interface IInterpreter
    {
        bool CreateProcess(string file);
        bool CreateProcess(string[] lines, string filename);
        void ExecuteProcess();
        bool HasVariable(string name);
        void MemoryDump();
        string GetInterpreterVersion();
    }
}
