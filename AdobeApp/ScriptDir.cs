using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace AdobeApp
{
    /// <summary>
    /// A temporary directory holding all JavaScript files
    /// </summary>
    public class ScriptDir: IDisposable
    {
        /// <summary>
        /// Directory
        /// </summary>
        /// <value>Full path to the temporary directory</value>
        public string Dir { get; set; }
        private bool isRandomDir;

        /// <summary>
        /// Initializes a new JavaScriptDir instance, creates a temporary directory and collects JavaScript files
        /// </summary>
        public ScriptDir()
        {
            Dir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(Dir);
            isRandomDir = true;
        }

        /// <summary>
        /// Initializes a new JavaScriptDir instance using an existing directory and collects JavaScript files
        /// </summary>
        /// <param name="dir">Full path to an existing directory</param>
        public ScriptDir(string dir)
        {
            Dir = dir;
            isRandomDir = false;
        }

        /// <summary>
        /// Fills the directory with all files from a given collector
        /// </summary>
        /// <param name="collection">JavaScript collection to populate from</param>
        public void Populate(JavaScriptCollection collection)
        {
            foreach (var fileName in collection.Files()) 
            {
                Save(fileName, collection[fileName]);
            }
        }

        /// <summary>
        /// Writes the given content into a file inside the directory
        /// </summary>
        /// <param name="fileName">File name to save into</param>
        /// <param name="content">Content of the JavaScript file</param>
        public void Save(string fileName, string content)
        {
            File.WriteAllText(Script(fileName), content);
        }

        /// <summary>
        /// Load the content of the specified filename.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public string Load(string fileName)
        {
            return File.ReadAllText(Script(fileName));
        }

        /// <summary>
        /// Generates a path to a file inside dir.
        /// </summary>
        /// <returns>Full path to the file inside the directory</returns>
        /// <param name="fileName">File name.</param>
        public string Script(string fileName)
        {
            return Path.Combine(Dir, fileName);
        }

        public void Dispose()
        {
            if (isRandomDir)
            {
                Directory.Delete(Dir, true);
            }
        }
    }
}
