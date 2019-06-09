using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace DALGen
{
    public class DalGenMain
    {

        public DalGenerator db { get; set; }

        public DalGenMain()
        {
        }



        private bool updDALEntries(GenerateSettings settings)
        {
            bool resultCode = true;

            var db = new DalGenerator();

            //Get Data from Data Project.
            var frameworkEntries = db.FrameworkTemplates.Where(o=>o.StepNo ==0).Where(o =>  o.Ovveride == true).OrderBy(o => o.CallPriority).ToList();

            foreach (FrameworkTemplate item in frameworkEntries.Where(o => o.Mode < 4).OrderBy(o => o.CallPriority))
            {
                if (item.Mode != 1 && item.Mode != 3)
                {
                    //DbContext
                    var DalBaseDir = settings.BaseDirectory + @"\" + settings.DalSpaceName;
                    if (System.IO.Directory.Exists(DalBaseDir + item.RelativePath))
                    {
                        System.IO.Directory.CreateDirectory(DalBaseDir + item.RelativePath);
                    }
                    var fileCnt = System.Text.Encoding.Default.GetString(item.TemplateContent);
                    var fileName = settings.BaseDirectory + @"\" + settings.DalSpaceName + item.RelativePath + clearEntry(item.TemplateCodeFile, settings);
                    fileCnt = clearEntry(fileCnt, settings);
                    var defaultdbSets = "public virtual DbSet<#Model#> #Model# { get; set; }";
                    var allDbSets = "";
                    foreach (var modelEntry in settings.ModelNames)
                    {
                        allDbSets += defaultdbSets.Replace("#Model#", modelEntry) + Environment.NewLine;
                    }
                    fileCnt = fileCnt.Replace("#public virtual DbSet<#Model#> #Model# { get; set; }#", allDbSets);

                    System.IO.File.WriteAllText(fileName, fileCnt);
                }
                else
                {

                    if (item.PerModel == false)
                    {
                        var DalBaseDir = settings.BaseDirectory + @"\" + settings.DalSpaceName;
                        if (System.IO.Directory.Exists(DalBaseDir + item.RelativePath))
                        {
                            System.IO.Directory.CreateDirectory(DalBaseDir + item.RelativePath);
                        }
                        var fileCnt = System.Text.Encoding.Default.GetString(item.TemplateContent);
                        fileCnt = clearEntry(fileCnt, settings);
                        var fileName = settings.BaseDirectory + @"\" + settings.DalSpaceName + item.RelativePath + clearEntry(item.TemplateCodeFile, settings);
                        System.IO.File.WriteAllText(fileName, fileCnt);
                    }
                    else
                    {
                        var DalBaseDir = settings.BaseDirectory + @"\" + settings.DalSpaceName;
                       
                        foreach (var modelEntry in settings.ModelNames)
                        {
                            var fileCnt = System.Text.Encoding.Default.GetString(item.TemplateContent);
                            var relativePath = clearEntry(item.RelativePath, settings, modelEntry);
                            if (System.IO.Directory.Exists(DalBaseDir + relativePath))
                            {
                                System.IO.Directory.CreateDirectory(DalBaseDir + relativePath);
                            }
                            fileCnt = clearEntry(fileCnt, settings, modelEntry);
                            var fileName = settings.BaseDirectory + @"\" + settings.DalSpaceName + relativePath + clearEntry(item.TemplateCodeFile, settings, modelEntry);
                            System.IO.File.WriteAllText(fileName, fileCnt);
                        }
                    }
                }
            }

            var privateObjects = "";
            var publicObjects = "";
            foreach (var item in frameworkEntries.Where(o => o.Mode == 4).OrderBy(o => o.CallPriority))
            {

                if (item.TemplateCodeFile.StartsWith("private"))
                {
                    var privateContent = System.Text.Encoding.Default.GetString(item.TemplateContent);
                    foreach (var modelEntry in settings.ModelNames)
                    {
                        privateObjects += clearEntry(privateContent, settings, modelEntry) + Environment.NewLine;
                    }
                }
                if (item.TemplateCodeFile.StartsWith("public"))
                {
                    var publicContent = System.Text.Encoding.Default.GetString(item.TemplateContent);
                    foreach (var modelEntry in settings.ModelNames)
                    {
                        publicObjects += clearEntry(publicContent, settings, modelEntry) + Environment.NewLine;
                    }
                }
            }

            var finalDalItem = frameworkEntries.Where(o => o.Mode == 5).FirstOrDefault();
            var finalCnt = System.Text.Encoding.Default.GetString(finalDalItem.TemplateContent);
            finalCnt = clearEntry(finalCnt, settings);
            finalCnt = finalCnt.Replace("#PrivateObjects#", privateObjects);
            finalCnt = finalCnt.Replace("#PublicObjects#", publicObjects);
            var finalfileName = settings.BaseDirectory + @"\" + settings.DalSpaceName + finalDalItem.RelativePath + clearEntry(finalDalItem.TemplateCodeFile, settings);
            System.IO.File.WriteAllText(finalfileName, finalCnt);

            return resultCode;
        }

        private bool generateDALEntries(GenerateSettings settings)
        {
            bool resultCode = true;

            var db = new DalGenerator();

            //Get Data from Data Project.
            var frameworkEntries = db.FrameworkTemplates.Where(o=>o.StepNo == 0).OrderBy(o=>o.CallPriority).ToList();

            foreach (FrameworkTemplate item in frameworkEntries.Where(o=>o.Mode<4).OrderBy(o=>o.CallPriority))
            {
                if(item.Mode!=1 && item.Mode!=3)
                {
                    //DbContext
                    var DalBaseDir = settings.BaseDirectory + @"\" + settings.DalSpaceName;
                    if (!System.IO.Directory.Exists(DalBaseDir + item.RelativePath))
                    {
                        System.IO.Directory.CreateDirectory(DalBaseDir + item.RelativePath);
                    }
                    var fileCnt = System.Text.Encoding.Default.GetString(item.TemplateContent);
                    var fileName = settings.BaseDirectory + @"\" + settings.DalSpaceName + item.RelativePath + clearEntry(item.TemplateCodeFile, settings);
                    fileCnt = clearEntry(fileCnt, settings);
                    var defaultdbSets = "public virtual DbSet<#Model#> #Model# { get; set; }";
                    var allDbSets = "";
                    foreach (var modelEntry in settings.ModelNames)
                    {
                        allDbSets += defaultdbSets.Replace("#Model#", modelEntry)+Environment.NewLine;
                    }
                        fileCnt = fileCnt.Replace("#public virtual DbSet<#Model#> #Model# { get; set; }#", allDbSets);

                    System.IO.File.WriteAllText(fileName, fileCnt);
                }
                else
                {
                   
                    if(item.PerModel==false)
                    {
                        var DalBaseDir = settings.BaseDirectory + @"\" + settings.DalSpaceName;
                        if (!System.IO.Directory.Exists(DalBaseDir + item.RelativePath))
                        {
                            System.IO.Directory.CreateDirectory(DalBaseDir + item.RelativePath);
                        }
                        var fileCnt = System.Text.Encoding.Default.GetString(item.TemplateContent);
                        fileCnt = clearEntry(fileCnt, settings);
                        var fileName = DalBaseDir + item.RelativePath + clearEntry(item.TemplateCodeFile, settings);
                        System.IO.File.WriteAllText(fileName, fileCnt);
                    }
                    else
                    {
                        var DalBaseDir = settings.BaseDirectory + @"\" + settings.DalSpaceName;
                        
                        foreach (var modelEntry in settings.ModelNames)
                        {
                            var fileCnt = System.Text.Encoding.Default.GetString(item.TemplateContent);
                            var relativePath = clearEntry(item.RelativePath, settings, modelEntry);
                            if (!System.IO.Directory.Exists(DalBaseDir + relativePath))
                            {
                                System.IO.Directory.CreateDirectory(DalBaseDir + relativePath);
                            }
                            fileCnt = clearEntry(fileCnt, settings,modelEntry);
                            var fileName = settings.BaseDirectory + @"\" + settings.DalSpaceName + relativePath + clearEntry(item.TemplateCodeFile, settings,modelEntry);
                            System.IO.File.WriteAllText(fileName, fileCnt);
                        }
                    }
                }
            }

            var privateObjects = "";
            var publicObjects = "";
            foreach (var item in frameworkEntries.Where(o => o.Mode == 4).OrderBy(o => o.CallPriority))
            {
                
                if(item.TemplateCodeFile.StartsWith("private"))
                {
                    var privateContent = System.Text.Encoding.Default.GetString(item.TemplateContent);
                    foreach (var modelEntry in settings.ModelNames)
                    {
                        privateObjects += clearEntry(privateContent, settings, modelEntry) + Environment.NewLine;
                    }
                }
                if (item.TemplateCodeFile.StartsWith("public"))
                {
                    var publicContent = System.Text.Encoding.Default.GetString(item.TemplateContent);
                    foreach (var modelEntry in settings.ModelNames)
                    {
                        publicObjects += clearEntry(publicContent, settings, modelEntry) + Environment.NewLine;
                    }
                }
            }

            var finalDalItem = frameworkEntries.Where(o => o.Mode == 5).FirstOrDefault();
            var finalCnt = System.Text.Encoding.Default.GetString(finalDalItem.TemplateContent);
            finalCnt = clearEntry(finalCnt, settings);
            finalCnt = finalCnt.Replace("#PrivateObjects#", privateObjects);
            finalCnt = finalCnt.Replace("#PublicObjects#", publicObjects);

            var refObj = "";
            var baseRef = "using "+ settings.DalSpaceName+".DAL.#Model#DAL;";
            foreach (var item in settings.ModelNames)
            {
                refObj += refObj + baseRef.Replace("#Model#", item) + Environment.NewLine;
            }
            finalCnt = finalCnt.Replace("#using "+ settings.DalSpaceName+ ".DAL.#Model#DAL;#", refObj);

            var finalfileName = settings.BaseDirectory + @"\" + settings.DalSpaceName + finalDalItem.RelativePath + clearEntry(finalDalItem.TemplateCodeFile, settings);
            System.IO.File.WriteAllText(finalfileName, finalCnt);

            return resultCode;
        }


        private bool updApi(GenerateSettings settings)
        {
            bool resultCode = true;

            //var ctrlDir = settings.BaseDirectory + @"\" + settings.WebApiFolder;

            //var db = new DalGenerator();

            //var apiEntries = db.FrameworkTemplates.Where(o => o.StepNo == 1 && o.Ovveride == true).OrderBy(o => o.CallPriority).ToList();

            //foreach (var item in apiEntries)
            //{
            //    if (item.PerModel == true)
            //    {
            //        foreach (var modelEntry in settings.ModelNames)
            //        {
            //            var baseDir = ctrlDir + item.RelativePath;
            //            if (!System.IO.Directory.Exists(baseDir))
            //            {
            //                System.IO.Directory.CreateDirectory(baseDir);
            //            }
            //            var fileCnt = System.Text.Encoding.Default.GetString(item.TemplateContent);
            //            fileCnt = clearEntry(fileCnt, settings, modelEntry);
            //            var fileName = settings.BaseDirectory + @"\" + settings.WebApiFolder + item.RelativePath + clearEntry(item.TemplateCodeFile, settings, modelEntry);
            //            System.IO.File.WriteAllText(fileName, fileCnt);
            //        }
            //    }
            //    else
            //    {
            //        var baseDir = ctrlDir + item.RelativePath;
            //        if (!System.IO.Directory.Exists(baseDir))
            //        {
            //            System.IO.Directory.CreateDirectory(baseDir);
            //        }
            //        var fileCnt = System.Text.Encoding.Default.GetString(item.TemplateContent);
            //        fileCnt = clearEntry(fileCnt, settings);
            //        var fileName = settings.BaseDirectory + @"\" + settings.WebApiFolder + item.RelativePath + clearEntry(item.TemplateCodeFile, settings);
            //        System.IO.File.WriteAllText(fileName, fileCnt);
            //    }

            //}

            return resultCode;
        }

        private bool createApi(GenerateSettings settings)
        {
            bool resultCode = true;

            var ctrlDir = settings.BaseDirectory + @"\" + settings.WebApiFolder;

            var db = new DalGenerator();

            var apiEntries = db.FrameworkTemplates.Where(o => o.StepNo == 1).OrderBy(o => o.CallPriority).ToList();

            foreach (var item in apiEntries)
            {
                if (item.PerModel == true)
                {
                    foreach (var modelEntry in settings.ModelNames)
                    {
                        var baseDir = ctrlDir + item.RelativePath;
                        if (!System.IO.Directory.Exists(baseDir))
                        {
                            System.IO.Directory.CreateDirectory(baseDir);
                        }
                        var fileCnt = System.Text.Encoding.Default.GetString(item.TemplateContent);
                        fileCnt = clearEntry(fileCnt, settings, modelEntry);
                        var fileName = settings.BaseDirectory + @"\" + settings.WebApiFolder + item.RelativePath + clearEntry(item.TemplateCodeFile, settings, modelEntry);
                        System.IO.File.WriteAllText(fileName, fileCnt);
                    }
                }
                else
                {
                    var baseDir = ctrlDir + item.RelativePath;
                    if (!System.IO.Directory.Exists(baseDir))
                    {
                        System.IO.Directory.CreateDirectory(baseDir);
                    }
                    var fileCnt = System.Text.Encoding.Default.GetString(item.TemplateContent);
                    fileCnt = clearEntry(fileCnt, settings);
                    var fileName = settings.BaseDirectory + @"\" + settings.WebApiFolder + item.RelativePath + clearEntry(item.TemplateCodeFile, settings);
                    System.IO.File.WriteAllText(fileName, fileCnt);
                }

            }


            if (settings.SDKCreation == true)
            {
                if (resultCode == true)
                {

                    //Create SDk , 
                    resultCode = CoreHelper.CreateNewCoreProject(settings.BaseDirectory, settings.SDKFolder, "classlib");
                    if (resultCode == true)
                    {
                        foreach (var item in settings.SDKRefferences)
                        {
                            resultCode = CoreHelper.AddRefferenceToProject(settings.BaseDirectory, settings.SDKFolder + @"/" + settings.SDKFolder + @".csproj", item);
                        }
                        foreach (var item in settings.SDKPackages)
                        {
                            resultCode = CoreHelper.AddPackageToProject(settings.BaseDirectory, settings.SDKFolder + @"/" + settings.SDKFolder + @".csproj", item);
                        }
                        resultCode = CoreHelper.AddProjectToSolution(settings.BaseDirectory, settings.SDKFolder + @"/" + settings.SDKFolder + @".csproj", settings.SolutionName);
                        resultCode = createSdk(settings);
                    }

                }
            }
            return resultCode;
        }

        private bool updSdk(GenerateSettings settings)
        {
            bool resultCode = true;

            //var ctrlDir = settings.BaseDirectory + @"\" + settings.SDKFolder;

            //var db = new DalGenerator();

            //var apiEntries = db.FrameworkTemplates.Where(o => o.StepNo == 2 && o.Ovveride == true).OrderBy(o => o.CallPriority).ToList();

            //foreach (var item in apiEntries.Where(o => o.Mode != 1).OrderBy(o => o.CallPriority).ToList())
            //{

            //    if (item.PerModel == true)
            //    {
            //        foreach (var modelEntry in settings.ModelNames)
            //        {
            //            var baseDir = ctrlDir + item.RelativePath;
            //            if (!System.IO.Directory.Exists(baseDir))
            //            {
            //                System.IO.Directory.CreateDirectory(baseDir);
            //            }
            //            var fileCnt = System.Text.Encoding.Default.GetString(item.TemplateContent);
            //            fileCnt = clearEntry(fileCnt, settings, modelEntry);
            //            var fileName = settings.BaseDirectory + @"\" + settings.SDKFolder + item.RelativePath + clearEntry(item.TemplateCodeFile, settings, modelEntry);
            //            System.IO.File.WriteAllText(fileName, fileCnt);
            //        }
            //    }
            //    else
            //    {
            //        var baseDir = ctrlDir + item.RelativePath;
            //        if (!System.IO.Directory.Exists(baseDir))
            //        {
            //            System.IO.Directory.CreateDirectory(baseDir);
            //        }
            //        var fileCnt = System.Text.Encoding.Default.GetString(item.TemplateContent);
            //        fileCnt = clearEntry(fileCnt, settings);
            //        var fileName = settings.BaseDirectory + @"\" + settings.SDKFolder + item.RelativePath + clearEntry(item.TemplateCodeFile, settings);
            //        System.IO.File.WriteAllText(fileName, fileCnt);
            //    }



            //}

            //var privateObjects = "";
            //var publicObjects = "";
            //foreach (var item in apiEntries.Where(o => o.Mode == 4).OrderBy(o => o.CallPriority))
            //{

            //    if (item.TemplateCodeFile.StartsWith("private"))
            //    {
            //        var privateContent = System.Text.Encoding.Default.GetString(item.TemplateContent);
            //        foreach (var modelEntry in settings.ModelNames)
            //        {
            //            privateObjects += clearEntry(privateContent, settings, modelEntry) + Environment.NewLine;
            //        }
            //    }
            //    if (item.TemplateCodeFile.StartsWith("public"))
            //    {
            //        var publicContent = System.Text.Encoding.Default.GetString(item.TemplateContent);
            //        foreach (var modelEntry in settings.ModelNames)
            //        {
            //            publicObjects += clearEntry(publicContent, settings, modelEntry) + Environment.NewLine;
            //        }
            //    }
            //}

            //var finalDalItem = apiEntries.Where(o => o.Mode == 5).FirstOrDefault();
            //if (finalDalItem != null)
            //{
            //    var finalCnt = System.Text.Encoding.Default.GetString(finalDalItem.TemplateContent);
            //    finalCnt = clearEntry(finalCnt, settings);
            //    finalCnt = finalCnt.Replace("#PrivateObjects#", privateObjects);
            //    finalCnt = finalCnt.Replace("#PublicObjects#", publicObjects);
            //    var finalfileName = settings.BaseDirectory + @"\" + settings.SDKFolder + finalDalItem.RelativePath + clearEntry(finalDalItem.TemplateCodeFile, settings);
            //    System.IO.File.WriteAllText(finalfileName, finalCnt);
            //}

            return resultCode;
        }

        private bool createSdk(GenerateSettings settings)
        {
            bool resultCode = true;

            //var ctrlDir = settings.BaseDirectory + @"\" + settings.SDKFolder;

            //var db = new DalGenerator();

            //var apiEntries = db.FrameworkTemplates.Where(o => o.StepNo == 2).OrderBy(o => o.CallPriority).ToList();

            //foreach (var item in apiEntries.Where(o=>o.Mode!=1).OrderBy(o=>o.CallPriority).ToList())
            //{
                
            //        if (item.PerModel == true)
            //        {
            //            foreach (var modelEntry in settings.ModelNames)
            //            {
            //                var baseDir = ctrlDir + item.RelativePath;
            //                if (!System.IO.Directory.Exists(baseDir))
            //                {
            //                    System.IO.Directory.CreateDirectory(baseDir);
            //                }
            //                var fileCnt = System.Text.Encoding.Default.GetString(item.TemplateContent);
            //                fileCnt = clearEntry(fileCnt, settings, modelEntry);
            //                var fileName = settings.BaseDirectory + @"\" + settings.SDKFolder + item.RelativePath + clearEntry(item.TemplateCodeFile, settings, modelEntry);
            //                System.IO.File.WriteAllText(fileName, fileCnt);
            //            }
            //        }
            //        else
            //        {
            //            var baseDir = ctrlDir + item.RelativePath;
            //            if (!System.IO.Directory.Exists(baseDir))
            //            {
            //                System.IO.Directory.CreateDirectory(baseDir);
            //            }
            //            var fileCnt = System.Text.Encoding.Default.GetString(item.TemplateContent);
            //            fileCnt = clearEntry(fileCnt, settings);
            //            var fileName = settings.BaseDirectory + @"\" + settings.SDKFolder + item.RelativePath + clearEntry(item.TemplateCodeFile, settings);
            //            System.IO.File.WriteAllText(fileName, fileCnt);
            //        }
                
                

            //}

            //var privateObjects = "";
            //var publicObjects = "";
            //foreach (var item in apiEntries.Where(o => o.Mode == 4).OrderBy(o => o.CallPriority))
            //{

            //    if (item.TemplateCodeFile.StartsWith("private"))
            //    {
            //        var privateContent = System.Text.Encoding.Default.GetString(item.TemplateContent);
            //        foreach (var modelEntry in settings.ModelNames)
            //        {
            //            privateObjects += clearEntry(privateContent, settings, modelEntry) + Environment.NewLine;
            //        }
            //    }
            //    if (item.TemplateCodeFile.StartsWith("public"))
            //    {
            //        var publicContent = System.Text.Encoding.Default.GetString(item.TemplateContent);
            //        foreach (var modelEntry in settings.ModelNames)
            //        {
            //            publicObjects += clearEntry(publicContent, settings, modelEntry) + Environment.NewLine;
            //        }
            //    }
            //}

            //var finalDalItem = apiEntries.Where(o => o.Mode == 5).FirstOrDefault();
            //if (finalDalItem != null)
            //{
            //    var finalCnt = System.Text.Encoding.Default.GetString(finalDalItem.TemplateContent);
            //    finalCnt = clearEntry(finalCnt, settings);
            //    finalCnt = finalCnt.Replace("#PrivateObjects#", privateObjects);
            //    finalCnt = finalCnt.Replace("#PublicObjects#", publicObjects);
            //    var finalfileName = settings.BaseDirectory + @"\" + settings.SDKFolder + finalDalItem.RelativePath + clearEntry(finalDalItem.TemplateCodeFile, settings);
            //    System.IO.File.WriteAllText(finalfileName, finalCnt);
            //}


            //if (settings.WebCreation == true)
            //{
            //    //Create Web Api , 
            //    resultCode = CoreHelper.CreateNewCoreProject(settings.BaseDirectory, settings.WebFolder, "classlib");
            //    if (resultCode == true)
            //    {
            //        foreach (var item in settings.WebRefferences)
            //        {
            //            resultCode = CoreHelper.AddRefferenceToProject(settings.BaseDirectory, settings.WebFolder + @"/" + settings.WebFolder + @".csproj", item);
            //        }
            //        foreach (var item in settings.WebPackages)
            //        {
            //            resultCode = CoreHelper.AddPackageToProject(settings.BaseDirectory, settings.WebFolder + @"/" + settings.WebFolder + @".csproj", item);
            //        }
            //        resultCode = CoreHelper.AddProjectToSolution(settings.BaseDirectory, settings.WebFolder + @"/" + settings.WebFolder + @".csproj", settings.SolutionName);
            //        resultCode = createWeb(settings);
            //    }
                
            //}
            return resultCode;
        }

        private bool updWeb(GenerateSettings settings)
        {
            bool resultCode = true;

            //var ctrlDir = settings.BaseDirectory + @"\" + settings.WebFolder;

            //var db = new DalGenerator();

            //var apiEntries = db.FrameworkTemplates.Where(o => o.StepNo == 3 && o.Ovveride == true).OrderBy(o => o.CallPriority).ToList();

            //foreach (var item in apiEntries.OrderBy(o => o.CallPriority).ToList())
            //{


            //    if (item.PerModel == true)
            //    {

            //        foreach (var modelEntry in settings.ModelNames)
            //        {
            //            var baseDir = ctrlDir + item.RelativePath;
            //            if (!System.IO.Directory.Exists(baseDir))
            //            {
            //                System.IO.Directory.CreateDirectory(baseDir);
            //            }
            //            var fileCnt = System.Text.Encoding.Default.GetString(item.TemplateContent);
            //            fileCnt = clearEntry(fileCnt, settings, modelEntry);
            //            var fileName = settings.BaseDirectory + @"\" + settings.WebFolder + item.RelativePath + clearEntry(item.TemplateCodeFile, settings, modelEntry);
            //            System.IO.File.WriteAllText(fileName, fileCnt);
            //        }
            //    }
            //    else
            //    {
            //        var baseDir = ctrlDir + item.RelativePath;
            //        if (!System.IO.Directory.Exists(baseDir))
            //        {
            //            System.IO.Directory.CreateDirectory(baseDir);
            //        }
            //        var fileCnt = System.Text.Encoding.Default.GetString(item.TemplateContent);
            //        fileCnt = clearEntry(fileCnt, settings);
            //        var fileName = settings.BaseDirectory + @"\" + settings.WebFolder + item.RelativePath + clearEntry(item.TemplateCodeFile, settings);
            //        System.IO.File.WriteAllText(fileName, fileCnt);
            //    }



            //}

            return resultCode;
        }

        private bool createWeb(GenerateSettings settings)
        {
            bool resultCode = true;

            //var ctrlDir = settings.BaseDirectory + @"\" + settings.WebFolder;

            //var db = new DalGenerator();

            //var apiEntries = db.FrameworkTemplates.Where(o => o.StepNo == 3).OrderBy(o => o.CallPriority).ToList();

            //foreach (var item in apiEntries.OrderBy(o => o.CallPriority).ToList())
            //{
                

            //    if (item.PerModel == true)
            //    {

            //        foreach (var modelEntry in settings.ModelNames)
            //        {
            //            var baseDir = ctrlDir + item.RelativePath;
            //            if (!System.IO.Directory.Exists(baseDir))
            //            {
            //                System.IO.Directory.CreateDirectory(baseDir);
            //            }
            //            var fileCnt = System.Text.Encoding.Default.GetString(item.TemplateContent);
            //            fileCnt = clearEntry(fileCnt, settings, modelEntry);
            //            var fileName = settings.BaseDirectory + @"\" + settings.WebFolder + item.RelativePath + clearEntry(item.TemplateCodeFile, settings, modelEntry);
            //            System.IO.File.WriteAllText(fileName, fileCnt);
            //        }
            //    }
            //    else
            //    {
            //        var baseDir = ctrlDir + item.RelativePath;
            //        if (!System.IO.Directory.Exists(baseDir))
            //        {
            //            System.IO.Directory.CreateDirectory(baseDir);
            //        }
            //        var fileCnt = System.Text.Encoding.Default.GetString(item.TemplateContent);
            //        fileCnt = clearEntry(fileCnt, settings);
            //        var fileName = settings.BaseDirectory + @"\" + settings.WebFolder + item.RelativePath + clearEntry(item.TemplateCodeFile, settings);
            //        System.IO.File.WriteAllText(fileName, fileCnt);
            //    }



            //}

            return resultCode;
        }

        private string clearEntry(string item, GenerateSettings settings)
        {
            item =  item.Replace("#DALNameSpace#", settings.DalSpaceName);
            item = item.Replace("#ModelNameSpace#", settings.ModelSpaceName);
            item = item.Replace("#CompanyName#", settings.CompanyName);
            item = item.Replace("#CopyRight#", settings.CopyRight);
            item = item.Replace("#Author#", settings.AuthorName);
            item = item.Replace("#DbContextName#", settings.DbContextName);
            item = item.Replace("#ProjectName#", settings.ProjectName);
            item = item.Replace("#DevConnectionString#", settings.DevConnectionString);
            return item;
        }

        private string clearEntry(string item, GenerateSettings settings,string modelEntry)
        {
            item = item.Replace("#DALNameSpace#", settings.DalSpaceName);
            item = item.Replace("#ModelNameSpace#", settings.ModelSpaceName);
            item = item.Replace("#CompanyName#", settings.CompanyName);
            item = item.Replace("#CopyRight#", settings.CopyRight);
            item = item.Replace("#Author#", settings.AuthorName);
            item = item.Replace("#DbContextName#", settings.DbContextName);
            item = item.Replace("#ProjectName#", settings.ProjectName);
            item = item.Replace("#DevConnectionString#", settings.DevConnectionString);
            item = item.Replace("#Model#", modelEntry);
            item = item.Replace("#model#", modelEntry.Substring(0,1).ToLower()+modelEntry.Substring(1,modelEntry.Length-1));
            return item;
        }

        public Boolean UpdateDAL(GenerateSettings settings)
        {
            bool resultCode = true;
            db = new DalGenerator();

            resultCode = updDALEntries(settings);

            if (resultCode == true)
            {
                if (settings.WebApiCreation == true)
                {
                    resultCode = updApi(settings);
                    if (resultCode == true)
                    {
                        if (settings.SDKCreation == true)
                        {
                            resultCode = updSdk(settings);
                            if (resultCode == true)
                            {
                                if (settings.WebCreation == true)
                                {
                                    resultCode = updWeb(settings);
                                }
                            }
                        }
                    }
                }
            }

            return resultCode;

        }

        public Boolean createDAL(GenerateSettings settings)
        {
            bool resultCode = true;

            CoreHelper.NewJson(settings.BaseDirectory, settings.CoreSdkVer);

            db = new DalGenerator();
            resultCode = CoreHelper.CreateNewCoreProject(settings.BaseDirectory, settings.DalSpaceName, "classlib");

            if (resultCode == true)
            {
                resultCode = CoreHelper.AddProjectToSolution(settings.BaseDirectory, settings.DalSpaceName + @"/" + settings.DalSpaceName + @".csproj", settings.SolutionName);

                if (resultCode == true)
                {
                    foreach (var item in settings.DalRefferences)
                    {
                        resultCode = CoreHelper.AddRefferenceToProject(settings.BaseDirectory, settings.DalSpaceName + @"/" + settings.DalSpaceName + @".csproj", item);
                    }
                    foreach (var item in settings.DalPackages)
                    {
                        resultCode = CoreHelper.AddPackageToProject(settings.BaseDirectory, settings.DalSpaceName + @"/" + settings.DalSpaceName + @".csproj", item);
                    }
                    resultCode = CoreHelper.AddProjectToSolution(settings.BaseDirectory, settings.DalSpaceName + @"/" + settings.DalSpaceName + @".csproj", settings.SolutionName);
                }

                resultCode = generateDALEntries(settings);
            }
            if (resultCode == true)
            {
                if (settings.WebApiCreation == true)
                {
                    //Create Web Api , 
                    resultCode = CoreHelper.CreateNewCoreProject(settings.BaseDirectory, settings.WebApiFolder, "webapi ");
                    //CoreHelper.BuildProject(settings.BaseDirectory + @"\" + settings.DalSpaceName);
                    if (resultCode == true)
                    {
                        
                        foreach (var item in settings.WebApiRefferences)
                        {
                            resultCode = CoreHelper.AddRefferenceToProject(settings.BaseDirectory, settings.WebApiFolder + @"/" + settings.WebApiFolder + @".csproj", item);
                        }
                        foreach (var item in settings.WebApiPackages)
                        {
                            resultCode = CoreHelper.AddPackageToProject(settings.BaseDirectory, settings.WebApiFolder + @"/" + settings.WebApiFolder + @".csproj", item);
                        }
                        resultCode = CoreHelper.AddProjectToSolution(settings.BaseDirectory, settings.WebApiFolder + @"/" + settings.WebApiFolder + @".csproj", settings.SolutionName);
                        resultCode = createApi(settings);
                    }
                }
            }



            return resultCode;
        }

    }
}
