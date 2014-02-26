using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BIOS.FileSystem
{
    /// <summary>
    /// Represents a file that only exists in memory. A VirtualFile has no real backing.
    /// </summary>
    public class VirtualFile : File
    {
        /// <summary>
        /// Instantiates an empty VirtualFile
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parent"></param>
        public VirtualFile(string name, Folder parent)
            : base(name, parent)
        {
            Lines = new string[0];
        }

        /// <summary>
        /// Instantiates a VirtualFile with the given contents
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parent"></param>
        /// <param name="contents"></param>
        public VirtualFile(string name, Folder parent, string[] contents)
            : base(name, parent)
        {
            if (contents == null)
                throw new ArgumentNullException();

            Lines = contents;
        }

        /// <summary>
        /// Instantiates a VirtualFile with the given contents
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parent"></param>
        /// <param name="contents"></param>
        public VirtualFile(string name, Folder parent, string contents)
            : base(name, parent)
        {
            if (contents == null)
                throw new ArgumentNullException();

            Text = contents;
        }

        /// <summary>
        /// Instantiates a VirtualFile with the given contents
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parent"></param>
        /// <param name="contents"></param>
        public VirtualFile(string name, Folder parent, byte[] contents)
            : base(name, parent)
        {
            if (contents == null)
                throw new ArgumentNullException();

            Bytes = contents;
        }

        /// <summary>
        /// Converts the VirtualFile to a File with a backing.
        /// </summary>
        /// <param name="fileExtension"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public File ToFile(string fileExtension, Folder parent)
        {
            if (parent.RealFolder == null)
                throw new ArgumentNullException("Parent must contain a real folder for backing.");

            return File.CreateFile(Name, fileExtension, Lines, parent);
        }

        string[] lines;
        /// <summary>
        /// Lines contained in the file.
        /// </summary>
        public override string[] Lines
        {
            get
            {
                return lines;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();

                lines = value;
            }
        }

        /// <summary>
        /// Text contained in the file.
        /// </summary>
        public override string Text
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                foreach (string line in Lines)
                {
                    sb.AppendLine(line);
                }

                return sb.ToString();
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();

                Lines = value.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            }
        }

        /// <summary>
        /// Bytes contained in the file.
        /// </summary>
        public override byte[] Bytes
        {
            get
            {
                return Encoding.UTF8.GetBytes(Text);
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();

                Text = Encoding.UTF8.GetString(value);
            }
        }

        /// <summary>
        /// Deletes the file from the parent folder.
        /// </summary>
        public override void Delete()
        {
            Parent.DeleteFile(Name);
        }

        /// <summary>
        /// Number of bytes used by the file.
        /// </summary>
        public override uint Size
        {
            get
            {
                return (uint)Bytes.Length;
            }
        }
    }
}
