using System;
using System.IO;

namespace ClashRoyalCli
{
    public class FileWriteable : IWriteable, IDisposable
    {
        private StreamWriter _file;

        public FileWriteable(string pathFile) => _file = File.CreateText(pathFile);

        public void Dispose() => _file.Close();

        public void Write(string str = null) => _file.Write(str);

        public void WriteLine(string line) => _file.WriteLine(line);
    }
}
