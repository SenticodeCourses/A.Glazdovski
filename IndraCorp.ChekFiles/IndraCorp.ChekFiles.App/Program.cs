using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IndraCorp.ChekFiles.Method;
using System.IO;

namespace IndraCorp.ChekFiles.App
{
    class Program
    {
        static void Main(string[] args)
        {
            FolderMonitor folderMonitor = new FolderMonitor();

            string controlString;
            do
            {
                Console.Clear();
                Console.WriteLine("Welcome to the folder check utility.\n\n");
                Console.WriteLine("1 - Add new folder to check\n2 - Save current status on disk\n" +
                    "3 - Look changes from last saved status\n4 - Choose(view list) monitored folder\n" +
                    "5 - Delete monitored folder from list\n0 - Exit");
                controlString = Console.ReadLine();

                if (controlString == "1") folderMonitor.AddNewFolderToList();
                if (controlString == "4") folderMonitor.ChooseMonitoredFolder();
                if (controlString == "5") folderMonitor.DeleteChoosenFolder();


            }
            while (controlString != "0");



        }
    }
}
