using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BIOS.FileSystem
{
    public class File
    {
        /// <summary>
        /// Instantiates a file with no real backing.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parent"></param>
        protected File(string name, Folder parent)
        {
        }

        /// <summary>
        /// Instantiates a file
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="realFile"></param>
        public File(Folder parent, FileInfo realFile)
            : this(realFile.Name, parent, realFile)
        {
        }

        /// <summary>
        /// Instantiates a file
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parent"></param>
        /// <param name="realFile"></param>
        public File(string name, Folder parent, FileInfo realFile)
        {
            if (name == null)
                throw new ArgumentNullException();
            if (parent == null)
                throw new ArgumentNullException();
            if (!realFile.Exists)
                throw new FileNotFoundException();
            if (name == "")
                throw new ArgumentOutOfRangeException("File name cannot be empty string.");

            this.name = name;
            Parent = parent;
            RealFile = realFile;
        }

        /// <summary>
        /// Creates a file with the given extension and content and stores it in the provided folder.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fileExtension"></param>
        /// <param name="contents"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static File CreateFile(string name, string fileExtension, string[] contents, Folder parent)
        {
            if ((name == null) | (fileExtension == null) | (contents == null) | (parent == null))
                throw new ArgumentNullException();

            FileInfo file = new FileInfo(parent.RealPath + "\\" + name + "." + fileExtension);
            file.Create();

            System.IO.File.WriteAllLines(file.FullName, contents);
            return new File(parent, file);
        }

        /// <summary>
        /// Folder that contains this file
        /// </summary>
        public Folder Parent { get; private set; }

        /// <summary>
        /// The real file representing this file
        /// </summary>
        public FileInfo RealFile { get; private set; }

        /// <summary>
        /// The path to the real file
        /// </summary>
        public string RealPath
        {
            get
            {
                return RealFile.FullName;
            }
        }

        /// <summary>
        /// The path to this file in the virtual file system
        /// </summary>
        public string VirtualPath
        {
            get
            {
                return Parent.VirtualPath + "/" + Name;
            }
        }

        private string name;
        /// <summary>
        /// Name of the file
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (value == null) throw new ArgumentNullException();
                if (value == "") throw new ArgumentOutOfRangeException("Filename cannot be empty string.");

                name = value;
            }
        }

        /// <summary>
        /// Gets or sets the lines contained in the file. Reads or writes directly from the backing file.
        /// </summary>
        public virtual string[] Lines
        {
            get
            {
                return System.IO.File.ReadAllLines(RealPath);
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();

                System.IO.File.WriteAllLines(RealPath, value);
            }
        }

        /// <summary>
        /// Gets or sets the text contained in the file. Reads or writes directly from the backing file.
        /// </summary>
        public virtual string Text
        {
            get
            {
                return System.IO.File.ReadAllText(RealPath);
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();

                System.IO.File.WriteAllText(RealPath, value);
            }
        }

        /// <summary>
        /// Gets or sets the bytes contained in the file. Reads or writes directly from the backing file.
        /// </summary>
        public virtual byte[] Bytes
        {
            get
            {
                return System.IO.File.ReadAllBytes(RealPath);
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();

                System.IO.File.WriteAllBytes(RealPath, value);
            }
        }

        /// <summary>
        /// Deletes both the real version of the file, and the virtual version.
        /// </summary>
        public virtual void Delete()
        {
            RealFile.Delete();
            Parent.DeleteFile(Name);
        }

        /// <summary>
        /// Changes the current parent.
        /// </summary>
        /// <param name="newParent"></param>
        public void Move(Folder newParent)
        {
            if (newParent == null)
                throw new ArgumentNullException();

            if (newParent.ContainsFile(Name))
                throw new InvalidOperationException("New parent already contains file with the same name.");

            Parent.DeleteFile(Name);
            Parent = newParent;
        }

        /// <summary>
        /// The number of bytes the file takes up
        /// </summary>
        public virtual uint Size
        {
            get
            {
                return (uint)RealFile.Length;
            }
        }
    }
}
