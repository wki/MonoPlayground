using System;
using System.IO;

namespace AdobeApp
{
    public class JavaScriptDir: IDisposable
    {
        public string Dir { get; set; }

        public JavaScriptDir()
        {
            Dir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(Dir);
        }

        public void SaveJavaScript(string fileName, string content)
        {
            File.WriteAllText(JavaScript(fileName), content);
        }

        public string JavaScript(string fileName)
        {
            return Path.Combine(Dir, fileName);
        }

        public void Dispose()
        {
            Directory.Delete(Dir, true);
        }
    }
}
