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
            File.WriteAllText(Path.Combine(Dir, fileName), content);
        }

        public void Dispose()
        {
            Directory.Delete(Dir, true);
        }
    }
}
