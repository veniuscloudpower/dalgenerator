using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DALGen
{
    public class ReflectionHelper
    {

        private string executalbleFile { get; set; }

        public ReflectionHelper(string execFile)
        {
            this.executalbleFile = execFile;
        }

        public List<string>   GetClassObjects()
        {
            List<string> results = new List<string>();
            Assembly assembly = Assembly.LoadFile(executalbleFile);

            foreach (Type item in GetLoadableTypes(assembly))
            {
                if(item.GetType() != typeof(int))
                {
                    results.Add(item.Name);
                }
                
            }

            return results;

        }


        public  IEnumerable<Type> GetLoadableTypes( Assembly assembly)
        {
            // TODO: Argument validation
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Exception exSub in ex.LoaderExceptions)
                {
                    sb.AppendLine(exSub.Message);
                    FileNotFoundException exFileNotFound = exSub as FileNotFoundException;
                    if (exFileNotFound != null)
                    {
                        if (!string.IsNullOrEmpty(exFileNotFound.FusionLog))
                        {
                            sb.AppendLine("Fusion Log:");
                            sb.AppendLine(exFileNotFound.FusionLog);
                        }
                    }
                    sb.AppendLine();
                }
                string errorMessage = sb.ToString();
                //Display or log the error based on your application.
                return null;

            }
        }

        //ToDo
        public List<string> GetInterfacesObj()
        {
            List<string> results = new List<string>();
            Assembly assembly = Assembly.LoadFile(executalbleFile);
            foreach (Type item in assembly.GetTypes())
            {
                var myTypes =  item.GetInterfaces();
                foreach  (Type iType in myTypes)
                {
                    results.Add(iType.GetMethods().ToString());
                }
              
            }

            return results;
        }


    }
}
