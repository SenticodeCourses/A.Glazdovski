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
        Dictionary<string, List<string>> previousFolderCondition = new Dictionary<string, List<string>>();
        Dictionary<string, List<string>> currentFolderCondition = new Dictionary<string, List<string>>();
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
            Console.WriteLine("Press Enter.");
            Console.ReadLine();
        }

        public void ChooseMonitoredFolder()
        {
            int folderNumber = ChoosenFolderId();
            if (folderNumber > -1) currentMonitoringFolder = folderMonitorDirectoriesList[folderNumber];
            Console.WriteLine("Press Enter.");
            Console.ReadLine();
        }

        public void DeleteChoosenFolder()
        {
            int folderNumber = ChoosenFolderId();
            if (folderNumber > -1) folderMonitorDirectoriesList.RemoveAt(folderNumber);
            Console.WriteLine("Press Enter.");
            Console.ReadLine();
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

        public void GetFolderCondition()
        {
            currentFolderCondition =
                Directory.EnumerateDirectories(currentMonitoringFolder, "*.*", SearchOption.AllDirectories)
                .ToDictionary(i => i, i => Directory.EnumerateFiles(i)
                    .ToList());
            currentFolderCondition.Add(currentMonitoringFolder,
                Directory.EnumerateFiles(currentMonitoringFolder).ToList());
        }
    }
}
