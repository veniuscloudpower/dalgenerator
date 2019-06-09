using DALGen;
using System;
using System.Collections.Generic;
using System.Text;

namespace DalGen
{
    public class FrameStart
    {
        public FrameStart(string[] p)
        {
            ValidateArgs(p);
        }

        private void ValidateArgs(string[] p)
        {
            if (p.Length == 0)
            {
                ShowOption();
            }
            else
            {

                if (p[0] == "list")
                {
                    ShowOption();
                }

                if (p[0] == "init")
                {
                    InitDal(p);
                }

                if (p[0] == "generate")
                {
                    GenDal(p);
                }

                if (p[0] == "updateSettings")
                {
                    UpdateDal(p);
                }

                if (p[0] == "updateDALFiles")
                {
                    var dal = new DalGenerator();
                    dal.SaveChanges();
                }
            }
        }

        private void UpdateDal(string[] p)
        {
            var m = Newtonsoft.Json.JsonConvert.DeserializeObject<GenerateSettings>(System.IO.File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + @"\FrameworkSettings.json"));
            ReflectionHelper helper = new ReflectionHelper(m.DataFileName);
            var myclasses = helper.GetClassObjects();
            m.ModelNames = myclasses;
            System.IO.File.WriteAllText(System.IO.Directory.GetCurrentDirectory() + @"\FrameworkSettings.json", Newtonsoft.Json.JsonConvert.SerializeObject(m));
        }

        private void GenDal(string[] p)
        {
            try
            {
                var m = Newtonsoft.Json.JsonConvert.DeserializeObject<GenerateSettings>(System.IO.File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + @"\FrameworkSettings.json"));
                ReflectionHelper helper = new ReflectionHelper(m.DataFileName);
                var myclasses = helper.GetClassObjects();
                m.ModelNames = myclasses;
                if (m.ModelNames.Count == 0)
                {
                    Console.WriteLine("Invalid Data File, cannot retrieve objects.");
                }
                else
                {
                    DalGenMain frame = new DalGenMain();
                    frame.createDAL(m);
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
         

            

        }

        private void InitDal(string[] p)
        {
            
           

            if (p.Length < 3)
            {
                Console.WriteLine("Project name is required");
            }
            else
            {
                GenerateSettings m = new GenerateSettings
                {
                    AuthorName = "",
                    CoreSdkVer = "2.1.604",
                    SolutionName =p[2]+ @".sln",
                    BaseDirectory = System.IO.Directory.GetCurrentDirectory(),
                    CompanyName = "",
                    CopyRight = " 2019",
                    DalSpaceName =p[2]+ ".Business",
                    DalPackages = new System.Collections.Generic.List<string>(),
                    DalRefferences = new System.Collections.Generic.List<string>(),
                    ModelNames = new List<string>(),
                    DataFileName = System.IO.Directory.GetCurrentDirectory()+ @"\" +p[2]+@"\"+p[2]+@".Data\bin\Release\netstandard2.0\"+p[2]+".Data.dll",
                    DbContextName =p[2]+ "DbContext",
                    DevConnectionString = "DevConnection",

                    ModelSpaceName =p[2]+ ".data",
                    ProjectName = p[2],
                    WebApiCreation = false,
                    WebApiFolder =p[2]+ ".WebApi",
                    WebApiPackages = new System.Collections.Generic.List<string>(),
                    WebApiRefferences = new System.Collections.Generic.List<string>(),
                    SDKCreation = false,
                    SDKFolder = p[2]+".SDK",
                    SDKPackages = new System.Collections.Generic.List<string>(),
                    SDKRefferences = new System.Collections.Generic.List<string>(),
                    WebCreation = false,
                    WebFolder = p[2]+".Web",
                    WebPackages = new System.Collections.Generic.List<string>(),
                    WebRefferences = new System.Collections.Generic.List<string>()
                };

               


               

                m.DalRefferences.Add(System.IO.Directory.GetCurrentDirectory() + @"\"+p[2]+@"\"+p[2]+@".Data\"+p[2]+@".Data.csproj");
                m.DalPackages.Add(@"Microsoft.EntityFrameworkCore ");
                m.DalPackages.Add(@"Newtonsoft.Json ");

                m.WebApiRefferences.Add(System.IO.Directory.GetCurrentDirectory() + @"\"+p[2]+@"\"+p[2]+@".Business\"+p[2]+@".Business.csproj");
                m.WebApiRefferences.Add(System.IO.Directory.GetCurrentDirectory() + @"\"+p[2]+@"\"+p[2]+@".Data\"+p[2]+@".Data.csproj");

                m.WebApiPackages.Add(@"Microsoft.EntityFrameworkCore ");
                m.WebApiPackages.Add(@"Newtonsoft.Json ");

                System.IO.File.WriteAllText(System.IO.Directory.GetCurrentDirectory() + @"\FrameworkSettings.json", Newtonsoft.Json.JsonConvert.SerializeObject(m));

                var db = new DalGenerator();
                db.InitFiles();
            }
        }

        private void ShowOption()
        {
            Console.WriteLine("GenDal.exe options.........."+Environment.NewLine);

            Console.WriteLine("list  Display all available options" + Environment.NewLine);

            Console.WriteLine("init  Initialize Framework with default settings" + Environment.NewLine);

            Console.WriteLine("-project  Name of project(required)" + Environment.NewLine);

            //Console.WriteLine("-dataFile  Path to Domain dll (optional)" + Environment.NewLine);

            //Console.WriteLine("-dataProject  Path to Domain project file (optional)" + Environment.NewLine);

            //Console.WriteLine("-author  Author name (optional)" + Environment.NewLine);

            //Console.WriteLine("-company  Company name (optional)" + Environment.NewLine);

            Console.WriteLine("updateSettings  update existing settings" + Environment.NewLine);

            Console.WriteLine("generate  Run the Generator (required init first)" + Environment.NewLine);

            Console.WriteLine("updateDALFiles  Update the generator templates" + Environment.NewLine);
        }
    }
}
