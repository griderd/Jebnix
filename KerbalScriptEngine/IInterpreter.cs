using System;
namespace KerboScriptEngine
{
    interface IInterpreter
    {
        bool CreateProcess(string file);
        bool CreateProcess(string[] lines, string filename);
        void ExecuteProcess();
        Value GetVariable(string name);
        bool HasVariable(string name);
        void MemoryDump();
        void SetVariable(string name, Value val);
        bool TryGetVariable(string name, out Value value);
    }
}
