using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace IndraCorp.ChekFiles.Method
{

    public class FolderMonitor
    {
        CheckFileUtilityDictionary previousFolderCondition = new CheckFileUtilityDictionary();
        CheckFileUtilityDictionary currentFolderCondition = new CheckFileUtilityDictionary();
        List<string> folderMonitorDirectoriesList = new List<string>();
        string currentMonitoringFolder="";

        public void AddNewFolderToList()
        {
            Console.WriteLine("Input path:");
            var newPath = Console.ReadLine().Trim('/');
            if (Directory.Exists(newPath))
            {
                if (folderMonitorDirectoriesList.Contains(newPath))
                {
                    Console.WriteLine("This folder allready in list.");
                }
                else
                {
                    folderMonitorDirectoriesList.Add(newPath);
                    if (currentMonitoringFolder == "") currentMonitoringFolder = newPath;
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
                currentMonitoringFolder = folderMonitorDirectoriesList[folderNumber];
                GetFolderCondition();
            }
            PressEnter();
        }

        public void DeleteChoosenFolder()
        {
            int folderNumber = ChoosenFolderId();
            if (folderNumber > -1)
            {
                folderMonitorDirectoriesList.RemoveAt(folderNumber);
                currentMonitoringFolder = "";
            }
            PressEnter();
        }

        public void SaveCurrentFolderStatus()
        {
            GetFolderCondition();
            BinaryFormatter binaryFormatter= new BinaryFormatter();
            using (FileStream fileStream = 
                new FileStream($@"{currentMonitoringFolder}\foldercheckutility.dat", FileMode.Create))
            {
                binaryFormatter.Serialize(fileStream, currentFolderCondition);
            }
            Console.WriteLine("Save complite.");
            PressEnter();
        }

        public void ListCurrentDirectory()
        {
            GetFolderCondition();
            ListDictionary(currentFolderCondition);
        }

        public void ChangesList()
        {
            GetFolderCondition();
            LoadCurrentDirectoryPreviousStatus();
            Console.WriteLine("New Folders:");
            var prevDirList = previousFolderCondition.dict.Keys;
            var currentDirList = currentFolderCondition.dict.Keys;
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

        void ListDictionary(CheckFileUtilityDictionary dictionary)
        {
            foreach (var i in dictionary.dict.Keys)
                foreach (var ii in dictionary.dict[i]) Console.WriteLine(ii);
            PressEnter();
        }

        void LoadCurrentDirectoryPreviousStatus()
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (FileStream fileStream =
                new FileStream($@"{currentMonitoringFolder}\foldercheckutility.dat", FileMode.Open))
            {
                previousFolderCondition = (CheckFileUtilityDictionary)binaryFormatter.Deserialize(fileStream);
            }
        }

        int ChoosenFolderId()
        {
            int folderNumber = - 1;
            if (folderMonitorDirectoriesList.Count > 0)
            {
                for (int i = 0; i < folderMonitorDirectoriesList.Count; i++)
                {
                    Console.WriteLine(i + 1 + " " + folderMonitorDirectoriesList[i]);
                }
                Console.WriteLine("\nInput the number of the folder.");
                try
                {
                    folderNumber = Convert.ToInt32(Console.ReadLine());
                    if (folderNumber < 1 || folderNumber > folderMonitorDirectoriesList.Count)
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
            currentFolderCondition.dict =
                Directory.EnumerateDirectories(currentMonitoringFolder, "*.*", SearchOption.AllDirectories)
                .ToDictionary(i => i, i => Directory.EnumerateFiles(i)
                    .ToList());
            currentFolderCondition.dict.Add(currentMonitoringFolder,
                Directory.EnumerateFiles(currentMonitoringFolder).ToList());
        }

        void PressEnter()
        {
            Console.WriteLine("Press Enter.");
            Console.ReadLine();
        }

        List<string> createListOfAllPreviousFiles()
        {
            List<string> retString = new List<string>();
            foreach (var i in previousFolderCondition.dict.Keys)
                foreach (var ii in previousFolderCondition.dict[i])
                    retString.Add(ii);
            return retString;
        }

        List<string> createListOfAllCurrentFiles()
        {
            List<string> retString = new List<string>();
            foreach (var i in currentFolderCondition.dict.Keys)
                foreach (var ii in currentFolderCondition.dict[i])
                    retString.Add(ii);
            return retString;
        }

        //List<string> ListUnion (List<string> str1, List<string> str2)
        //{
        //    return str1.Union(str2).ToList();
        //}
    }
}
