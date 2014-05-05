﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jebnix;

namespace KerboScriptEngine.UtilityModules
{
    public class SystemAPI
    {
        static Processor processor;

        public static void Initialize(Processor proc)
        {
            processor = proc;

            Functions.RegisterFunction("system", "run", 1, new Action<string>(Run));
            Functions.RegisterFunction("system", "copyto", 2, new Action<string, string>(CopyTo));
            Functions.RegisterFunction("system", "copyfrom", 2, new Action<string, string>(CopyFrom));
            Functions.RegisterFunction("system", "delete", 1, new Action<string>(Delete));
        }

        /// <summary>
        /// Runs the given script
        /// </summary>
        /// <param name="script"></param>
        /// <param name="caller"></param>
        public static void Run(string script)
        {

        }

        public static void CopyTo(string sourceFile, string destFolder)
        {
            if (processor.CurrentFolder.ContainsFile(sourceFile))
            {
                if ((destFolder == "root") | (destFolder == "1"))
                    processor.CurrentFolder.GetFile(sourceFile).Move(processor.RootFolder);
                else if ((destFolder == "archive") | (destFolder == "0"))
                    processor.CurrentFolder.GetFile(sourceFile).Move(processor.Archive);
                else
                    throw new KSRuntimeException("Folder does not exist.", null, processor.CurrentProcess);
            }
            else
            {
                throw new KSRuntimeException("File not found.", null, processor.CurrentProcess);
            }
        }

        public static void CopyFrom(string file, string sourceFolder)
        {
            if ((sourceFolder == "root") | (sourceFolder == "1"))
            {
                if (processor.RootFolder.ContainsFile(file))
                    processor.RootFolder.GetFile(file).Move(processor.CurrentFolder);
                else
                    throw new KSRuntimeException("Folder does not exist.", null, processor.CurrentProcess);
            }
            else if ((sourceFolder == "archive") | (sourceFolder == "2"))
            {
                if (processor.Archive.ContainsFile(file))
                    processor.Archive.GetFile(file).Move(processor.CurrentFolder);
                else
                    throw new KSRuntimeException("Folder does not exist.", null, processor.CurrentProcess);
            }
            else
                throw new KSRuntimeException("Folder does not exist.", null, processor.CurrentProcess);
        }

        public static void Delete(string file)
        {
            if (processor.CurrentFolder.ContainsFile(file))
            {
                processor.CurrentFolder.DeleteFile(file);
            }
            else
            {
                throw new KSRuntimeException("File does not exist.", null, processor.CurrentProcess);
            }
        }
    }
}