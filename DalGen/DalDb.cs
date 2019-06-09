using System;
using System.Collections.Generic;

namespace DALGen
{
    public class DalGenerator
    {

        private string workingPath { get; set; }

        private string executionPath { get; set; }

        public DalGenerator(string connectionStr = "")
        {
           
            this.executionPath = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            this.workingPath = System.IO.Directory.GetCurrentDirectory();
            var conStr = "";
            if (System.IO.File.Exists(this.workingPath+@"\FrameworkDb.json"))
            {
                conStr = this.workingPath + @"\FrameworkDb.json";
            }
            else
            {
                System.IO.File.WriteAllBytes(this.workingPath + @"\FrameworkDb.json", DalGen.Properties.Resources.FrameworkDb);
                conStr = this.workingPath + @"\FrameworkDb.json";
            }

            if (!string.IsNullOrWhiteSpace(connectionStr))
            {
                conStr = connectionStr;
            }
            FrameworkTemplates =  Newtonsoft.Json.JsonConvert.DeserializeObject<List<FrameworkTemplate>>(System.IO.File.ReadAllText(conStr));
            
        }

       
        public virtual List<FrameworkTemplate> FrameworkTemplates { get; set; }


        public void SaveChanges()
        {
            GetFromFileSystem();
            System.IO.File.WriteAllText(this.workingPath + @"\FrameworkDb.json", Newtonsoft.Json.JsonConvert.SerializeObject(FrameworkTemplates));
        }


        public void InitFiles()
        {
            foreach (var item in FrameworkTemplates)
            {

                var dirName =this.workingPath+ @"\framework_files" + item.RelativePath;
                if (!System.IO.Directory.Exists(dirName))
                {
                    System.IO.Directory.CreateDirectory(dirName);
                }
                System.IO.File.WriteAllBytes(dirName + item.Id.ToString() + "_" + item.CallPriority.ToString() + "_" + item.PerModel.ToString() + "_" + item.Ovveride.ToString() + "_" + item.Mode.ToString() + "_" + item.StepNo.ToString() + "_" + item.TemplateCodeFile, item.TemplateContent);
            }
        }

        private void GetFromFileSystem()
        {
            var allFiles = System.IO.Directory.GetFiles(this.workingPath+ @"\framework_files\", "*.*", System.IO.SearchOption.AllDirectories);
             var frameworkList =new List<FrameworkTemplate>();
            foreach (var item in allFiles)
            {
                var fileEntries =(System.IO.Path.GetFileName(item)).Split('_');
                var fileContent = System.IO.File.ReadAllBytes(item);
                var basePath = System.IO.Path.GetDirectoryName(item).Replace(this.workingPath + @"\framework_files", string.Empty);
                frameworkList.Add(new FrameworkTemplate
                {
                    Id = Convert.ToInt32(fileEntries[0]),
                    TemplateCodeFile = fileEntries[6],
                    CallPriority = Convert.ToInt32(fileEntries[1]),
                    PerModel = Convert.ToBoolean(fileEntries[2]),
                    Ovveride = Convert.ToBoolean(fileEntries[3]),
                    Mode = Convert.ToInt32(fileEntries[4]),
                    StepNo = Convert.ToInt32(fileEntries[5]),
                    RelativePath = basePath,
                    TemplateContent = fileContent
                });

            }
            FrameworkTemplates = frameworkList;
        }

        


    }


    public class FrameworkPackages
    {

        public int Id { get; set; }

        public string PackageName { get; set; }
    }

  


    public partial class FrameworkTemplate
    {

        public int Id { get; set; }
      

        public string TemplateCodeFile { get; set; }

    
        public byte[] TemplateContent { get; set; }

   
        public string RelativePath { get; set; }

        public int? CallPriority { get; set; }

        public bool? PerModel { get; set; }

        public bool? Ovveride { get; set; }

        public int? Mode { get; set; }

        public int? StepNo { get; set; }
    }
}
