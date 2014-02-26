using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BIOS.FileSystem
{
    public class Folder
    {
        List<Folder> subfolders;
        List<File> files;

        /// <summary>
        /// Instantiates a folder with a real backing and the given name. Default MaxCapacity is 0 (unlimited).
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parent"></param>
        /// <param name="realFolder"></param>
        public Folder(string name, Folder parent, System.IO.DirectoryInfo realFolder)
        {
            if (name == null)
                throw new ArgumentNullException();
            if (name == "")
                throw new ArgumentOutOfRangeException("Folder name cannot be empty string.");
            if (realFolder == null)
                throw new ArgumentNullException();
            if (!realFolder.Exists)
                throw new DirectoryNotFoundException();

            this.name = name;
            Parent = parent;
            RealFolder = realFolder;
            MaxCapacity = 0;

            if (parent != null)
            {
                if (parent.ContainsFolder(name)) throw new InvalidOperationException("Folder with the given name already exsts.");

                parent.subfolders.Add(this);
            }

            subfolders = new List<Folder>();
            files = new List<File>();

            DirectoryInfo[] subdirectories = realFolder.GetDirectories();
            foreach (DirectoryInfo dir in subdirectories)
            {
                subfolders.Add(new Folder(dir, this));
            }

            FileInfo[] realfiles = realFolder.GetFiles();
            foreach (FileInfo file in realfiles)
            {
                files.Add(new File(this, file));
            }
        }

        /// <summary>
        /// Instantiates a folder with a real backing. Default MaxCapacity is 0 (unlimited).
        /// </summary>
        /// <param name="realFolder"></param>
        /// <param name="parent"></param>
        public Folder(DirectoryInfo realFolder, Folder parent)
        {
            if (realFolder == null)
                throw new ArgumentOutOfRangeException();
            if (!realFolder.Exists)
                throw new DirectoryNotFoundException();

            name = realFolder.Name;
            Parent = parent;
            RealFolder = realFolder;
            MaxCapacity = 0;

            subfolders = new List<Folder>();
            files = new List<File>();

            DirectoryInfo[] subdirectories = realFolder.GetDirectories();
            foreach (DirectoryInfo dir in subdirectories)
            {
                subfolders.Add(new Folder(dir, this));
            }

            FileInfo[] realfiles = realFolder.GetFiles();
            foreach (FileInfo file in realfiles)
            {
                files.Add(new File(this, file));
            }
        }

        /// <summary>
        /// Instantiates a virtual folder (with no real backing), with the given name and parent. Default MaxCapacity is 10,000.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parent"></param>
        public Folder(string name, Folder parent, uint capacity = 10000)
        {
            if (name == null)
                throw new ArgumentNullException();
            if (name == "")
                throw new ArgumentOutOfRangeException("Folder name cannot be empty string.");

            this.name = name;
            this.Parent = parent;
            MaxCapacity = capacity;
            RealFolder = null;
        }

        /// <summary>
        /// Instantiates a virtual folder (with no real backing), with the given name and parent. Default MaxCapacity is 10,000
        /// </summary>
        /// <param name="name"></param>
        public Folder(string name, uint capacity = 10000)
        {
            if (name == null)
                throw new ArgumentNullException();
            if (name == "")
                throw new ArgumentOutOfRangeException("Folder name cannot be empty string.");

            this.name = name;
            Parent = Parent;
            RealFolder = null;
            MaxCapacity = capacity;
        }

        /// <summary>
        /// Gets the maximum number of bytes that can be used by the folder. If zero, the folder has unlimited size.
        /// </summary>
        public uint MaxCapacity { get; private set; }

        /// <summary>
        /// Gets the number of bytes used by the folder.
        /// </summary>
        public uint BytesUsed
        {
            get
            {
                uint count = 0;

                foreach (File f in files)
                {
                    count += f.Size;
                }
                foreach (Folder f in subfolders)
                {
                    count += f.BytesUsed;
                }

                return count;
            }
        }

        public double UsageRatio
        {
            get
            {
                if (MaxCapacity != 0)
                    return ((double)BytesUsed) / ((double)MaxCapacity);
                else
                    return double.PositiveInfinity;
            }
        }

        public double UsagePercent
        {
            get
            {
                if (!double.IsInfinity(UsageRatio))
                    return UsageRatio * 100;
                else
                    return double.PositiveInfinity;
            }
        }
        
        /// <summary>
        /// Expands the storage capacity to meet the new size
        /// </summary>
        /// <param name="newSize"></param>
        public void Expand(uint newSize)
        {
            if (newSize <= MaxCapacity)
                throw new ArgumentOutOfRangeException();

            MaxCapacity = newSize;
        }

        /// <summary>
        /// Parent folder.
        /// </summary>
        public Folder Parent { get; private set; }

        private string name;
        /// <summary>
        /// Name of the folder.
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
                if (value == "") throw new ArgumentOutOfRangeException("Folder name cannot be empty string.");
            }
        }

        /// <summary>
        /// Folders contained in this folder.
        /// </summary>
        public Folder[] Subfolders
        {
            get
            {
                return subfolders.ToArray();
            }
        }

        /// <summary>
        /// Files contained in this folder
        /// </summary>
        public File[] Files
        {
            get
            {
                return files.ToArray();
            }
        }

        /// <summary>
        /// Path that exists within the Jebnix file system
        /// </summary>
        public string VirtualPath
        {
            get
            {
                StringBuilder path = new StringBuilder();
                Stack<Folder> folders = new Stack<Folder>();

                Folder f = this;
                while (f.Parent != null)
                {
                    folders.Push(f.Parent);
                    f = f.Parent;
                }

                folders.Reverse();
                while (folders.Count > 0)
                {
                    path.Append(folders.Pop().Name);
                    if (folders.Count > 0) path.Append("/");
                }

                return path.ToString();
            }
        }

        /// <summary>
        /// Path that exists within the real file system
        /// </summary>
        public string RealPath
        {
            get
            {
                return RealFolder.FullName;
            }
        }

        public DirectoryInfo RealFolder { get; private set; }

        public Folder GetSubfolder(string name)
        {
            foreach (Folder f in subfolders)
            {
                if (f.Name == name)
                    return f;
            }
            return null;
        }

        public File GetFile(string name)
        {
            foreach (File f in files)
            {
                if (f.Name == name)
                    return f;
            }
            return null;
        }

        public bool ContainsFile(string name)
        {
            foreach (File f in files)
            {
                if (f.Name == name)
                    return true;
            }
            return false;
        }

        public bool ContainsFolder(string name)
        {
            foreach (Folder f in subfolders)
            {
                if (f.Name == name)
                    return true;
            }
            return false;
        }

        public bool DeleteFile(string name)
        {
            for (int i = 0; i < files.Count; i++)
            {
                if (files[i].Name == name)
                {
                    files.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        public bool DeleteFolder(string name)
        {
            for (int i = 0; i < subfolders.Count; i++)
            {
                if (subfolders[i].Name == name)
                {
                    subfolders.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        public void MoveTo(Folder folder)
        {
            if (folder == null)
                throw new ArgumentNullException();

            if (folder.ContainsFolder(Name))
                throw new InvalidOperationException("Folder with the given name already exists.");

            Parent.DeleteFolder(Name);
            Parent = folder;
            folder.subfolders.Add(this);
        }
    }
}
