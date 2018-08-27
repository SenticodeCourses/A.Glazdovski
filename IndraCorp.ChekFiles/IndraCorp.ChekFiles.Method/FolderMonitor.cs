using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace IndraCorp.ChekFiles.Method
{
    public class FolderMonitor
    {
        private string _path;
        bool _isMonitoring;
        private Dictionary<string, bool> _latestFolderState = new Dictionary<string, bool>();
        private Dictionary<string, bool> _previouseFolderState = new Dictionary<string, bool>();

        FolderMonitor (string path)
        {
            if (path == null) throw new ArgumentNullException();
            if (!Directory.Exists(path)) throw new DirectoryNotFoundException();
            _path = path;
        }

        public void Monitoring()
        {
            if (!_isMonitoring) _isMonitoring = true;
        }
        public void FindChanges()
        {
            
        }

        public Dictionary<string, bool> SaveFolderState()
        {
            return Directory.EnumerateDirectories(_path).ToDictionary(i => i, i=> false);
        }

        public void CheckAddFile()
        {
            foreach (var s in _previouseFolderState)
            {
                if (_previouseFolderState)
            }
        }

        public void CheckDeleteFile()
        {

        }


    }
}
