using System;
using System.Diagnostics;

namespace DALGen
{
    /// <summary>
    /// Core Helper
    /// </summary>
    public static class CoreHelper
    {

        public static Boolean NewJson(string wDir,string sdkVer)
        {
            return Exec(" new globaljson --sdk-version "+sdkVer, wDir);
            // dotnet new globaljson --sdk-version 2.2.100
        }

        /// <summary>
        /// Build project
        /// </summary>
        /// <param name="wDir"></param>
        /// <returns></returns>
        public static Boolean BuildProject(string wDir)
        {
           return Exec(" build --configuration Release", wDir);
        }

        /// <summary>
        /// Add package to project
        /// </summary>
        /// <param name="wDir"></param>
        /// <param name="projectRelativePathName"></param>
        /// <param name="packageName"></param>
        /// <returns></returns>
        public static Boolean AddPackageToProject(string wDir, string projectRelativePathName, string packageName)
        {
            return Exec(" add " + projectRelativePathName + " package " + packageName, wDir);
        }

        /// <summary>
        /// Add refference to project
        /// </summary>
        /// <param name="wDir"></param>
        /// <param name="projectRelativePathName"></param>
        /// <param name="refRelativePathName"></param>
        /// <returns></returns>
        public static Boolean AddRefferenceToProject(string wDir,string projectRelativePathName,string refRelativePathName)
        {
            return Exec(" add "+ projectRelativePathName + " reference "+ refRelativePathName, wDir);
        }

        /// <summary>
        /// Add project to solution
        /// </summary>
        /// <param name="wDir"></param>
        /// <param name="projectName"></param>
        /// <param name="solutionName"></param>
        /// <returns></returns>
        public static Boolean AddProjectToSolution(string wDir,string projectrelativePathName,string solutionName)
        {
            return Exec(" sln "+solutionName +" add " + projectrelativePathName, wDir);
            //dotnet sln todo.sln add todo-app/todo-app.csproj
        }

        /// <summary>
        /// Create new core project
        /// </summary>
        /// <param name="wDir"></param>
        /// <param name="projectName"></param>
        /// <param name="projectType"></param>
        /// <returns></returns>
        public static Boolean CreateNewCoreProject(string wDir,string projectName,string projectType)
        {
            return Exec("new " + projectType + " -f netcoreapp2.1 -lang C# " + " " + " -n " + projectName, wDir);
            //dotnet new classlib -f netcoreapp2.0 -lang C# -n myproj.csproj
        }

        /// <summary>
        /// Create new solution
        /// </summary>
        /// <param name="wDir"></param>
        /// <param name="solutionName"></param>
        /// <returns></returns>
        public static Boolean CreateNewSolution(string wDir,string solutionName)
        {
            return Exec("new sln "+solutionName,wDir);
            //donet sln todo.sln
        }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="wdir"></param>
        /// <returns></returns>
        public static Boolean Exec(string ex,string wdir)
        {
            
            // Use ProcessStartInfo class
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            startInfo.FileName = "dotnet.exe";            
            startInfo.WorkingDirectory = wdir;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments =  ex;

            try
            {
                // Start the process with the info we specified.
                // Call WaitForExit and then the using statement will close.
                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                    return true;
                }
            }
            catch
            {
                return false;               
            }
        }


    }
}
