using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;

namespace IndraCorp.ChekFiles.Method
{

    public class FolderMonitor
    {
        public Dictionary<string, List<string>> _previousFolderCondition = new Dictionary<string, List<string>>();
        public Dictionary<string, List<string>> _currentFolderCondition = new Dictionary<string, List<string>>();
        List<string> _folderMonitorDirectoriesList = new List<string>();
        string _currentMonitoringFolder="";

        public void AddNewFolderToList()
        {
            Console.WriteLine("Input path:");
            var newPath = Console.ReadLine().Trim('/');
            if (Directory.Exists(newPath))
            {
                if (_folderMonitorDirectoriesList.Contains(newPath))
                {
                    Console.WriteLine("This folder allready in list.");
                }
                else
                {
                    _folderMonitorDirectoriesList.Add(newPath);
                    if (_currentMonitoringFolder == "") _currentMonitoringFolder = newPath;
                }
            }
            else
            {
                Console.WriteLine("Incorrect path.");
            }
            PressEnter();
        }

        public void ChooseMonitoredFolder()
        {
            int folderNumber = ChoosenFolderId();
            if (folderNumber > -1)
            {
                _currentMonitoringFolder = _folderMonitorDirectoriesList[folderNumber];
                GetFolderCondition();
            }
            PressEnter();
        }

        public void DeleteChoosenFolder()
        {
            int folderNumber = ChoosenFolderId();
            if (folderNumber > -1)
            {
                _folderMonitorDirectoriesList.RemoveAt(folderNumber);
                _currentMonitoringFolder = "";
            }
            PressEnter();
        }

        public void SaveCurrentFolderStatus()
        {
            GetFolderCondition();
            //BinaryFormatter binaryFormatter= new BinaryFormatter();
            //using (FileStream fileStream = 
            //    new FileStream($@"{_currentMonitoringFolder}\foldercheckutility.dat", FileMode.Create))
            //{
            //    binaryFormatter.Serialize(fileStream, _currentFolderCondition);
            //}
            string Json_currentMonitoringFolder = JsonConvert.SerializeObject(_currentFolderCondition, Formatting.Indented);
            File.WriteAllText($@"{_currentMonitoringFolder}\foldercheckutility.dat", Json_currentMonitoringFolder);

            Console.WriteLine("Save complite.");
            PressEnter();
        }

        public void ListCurrentDirectory()
        {
            GetFolderCondition();
            ListDictionary(_currentFolderCondition);
        }

        public void ChangesList()
        {
            GetFolderCondition();
            LoadCurrentDirectoryPreviousStatus();
            Console.WriteLine("New Folders:");
            var prevDirList = _previousFolderCondition.Keys;
            var currentDirList = _currentFolderCondition.Keys;
            var createdDirList = currentDirList.Except(prevDirList).ToList();
            foreach (string str in createdDirList) Console.WriteLine(str);
            PressEnter();
            Console.WriteLine("Deleted Folders:");
            var deletedDirList = prevDirList.Except(currentDirList).ToList();
            foreach (string str in deletedDirList) Console.WriteLine(str);
            PressEnter();
            List<string> prevFileList = createListOfAllPreviousFiles();
            List<string> currentFileList = createListOfAllCurrentFiles();
            Console.WriteLine("New Files:");
            var createdFiles = currentFileList.Except(prevFileList);
            foreach (string str in createdFiles) Console.WriteLine(str);
            PressEnter();
            Console.WriteLine("Deleted Files:");
            var deletedFiles = prevFileList.Except(currentFileList);
            foreach (string str in deletedFiles) Console.WriteLine(str);
            PressEnter();
        }

        void ListDictionary(Dictionary<string, List<string>> dictionary)
        {
            foreach (var i in dictionary.Keys)
                foreach (var ii in dictionary[i]) Console.WriteLine(ii);
            PressEnter();
        }

        void LoadCurrentDirectoryPreviousStatus()
        {
            if (File.Exists($@"{_currentMonitoringFolder}\foldercheckutility.dat"))
            {
                //BinaryFormatter binaryFormatter = new BinaryFormatter();
                //using (FileStream fileStream =
                //    new FileStream($@"{_currentMonitoringFolder}\foldercheckutility.dat", FileMode.Open))
                //{
                //    _previousFolderCondition = (CheckFileUtilityDictionary)binaryFormatter.Deserialize(fileStream);
                //}
                string JsonString;
                JsonString = File.ReadAllText($@"{_currentMonitoringFolder}\foldercheckutility.dat");
                _previousFolderCondition = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(JsonString);

            }
            else
            {
                Console.WriteLine("File not exists");
                PressEnter();
            }
        }

        int ChoosenFolderId()
        {
            int folderNumber = - 1;
            if (_folderMonitorDirectoriesList.Count > 0)
            {
                for (int i = 0; i < _folderMonitorDirectoriesList.Count; i++)
                {
                    Console.WriteLine(i + 1 + " " + _folderMonitorDirectoriesList[i]);
                }
                Console.WriteLine("\nInput the number of the folder.");
                try
                {
                    folderNumber = Convert.ToInt32(Console.ReadLine());
                    if (folderNumber < 1 || folderNumber > _folderMonitorDirectoriesList.Count)
                    {
                        Console.WriteLine("Wrong number.");
                        return -1;
                    }
                    return folderNumber - 1;
                }
                catch
                {
                    Console.WriteLine("Wrong number.");
                }
            }
            else Console.WriteLine("List is empty.");
            return folderNumber - 1;
        }

        void GetFolderCondition()
        {
            _currentFolderCondition =
                Directory.EnumerateDirectories(_currentMonitoringFolder, "*.*", SearchOption.AllDirectories)
                .ToDictionary(i => i, i => Directory.EnumerateFiles(i)
                    .ToList());
            _currentFolderCondition.Add(_currentMonitoringFolder,
                Directory.EnumerateFiles(_currentMonitoringFolder).ToList());
        }

        void PressEnter()
        {
            Console.WriteLine("Press Enter.");
            Console.ReadLine();
        }

        List<string> createListOfAllPreviousFiles()
        {
            List<string> retString = new List<string>();
            foreach (var i in _previousFolderCondition.Keys)
                foreach (var ii in _previousFolderCondition[i])
                    retString.Add(ii);
            return retString;
        }

        List<string> createListOfAllCurrentFiles()
        {
            List<string> retString = new List<string>();
            foreach (var i in _currentFolderCondition.Keys)
                foreach (var ii in _currentFolderCondition[i])
                    retString.Add(ii);
            return retString;
        }
    }
}
