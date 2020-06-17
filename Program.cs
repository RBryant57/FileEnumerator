using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace ConsoleApp1
{
    class Program
    {
        public static IConfiguration Configuration { get; set; } 
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()     
                .AddJsonFile("appSettings.json");   
            Configuration = builder.Build();

            var finalFolderString = Configuration["FolderLocations:Final"];
            var sourceFolderString = Configuration["FolderLocations:Source"];
            var compareFolderString = Configuration["FolderLocations:Compare"];
            var allFileName = Configuration["FileNames:All"];
            var otherFileName = Configuration["FileNames:Other"];

            var compareFolders = compareFolderString.Split(',').ToList();
            var sourceList = new List<string>();
            var compareList = new List<string>();
            
            var sourceListDirectory = new DirectoryInfo(sourceFolderString);
            var sourceListFiles = sourceListDirectory.GetFiles().ToList();

            foreach(var list in sourceListFiles)
            {
                if(list.Extension == ".webp")
                {
                    var newName = list.FullName.Remove(list.FullName.LastIndexOf("."), 5);
                    newName = newName + ".jpg";
                    list.MoveTo(newName);
                }

                sourceList.Add(list.Name);
            }
            sourceList.Sort();

            foreach(var folder in compareFolders)
            {
                var compareListDirectory = new DirectoryInfo(folder);
                var compareListFiles = compareListDirectory.GetFiles().ToList();
                foreach (var list in compareListFiles)
                {
                    if(list.Extension == ".webp")
                {
                    var newName = list.FullName.Remove(list.FullName.LastIndexOf("."), 5);
                    newName = newName + ".jpg";
                    list.MoveTo(newName);
                }
                    compareList.Add(list.Name);
                }
            }
            compareList.Sort();

            var pathString = System.IO.Path.Combine(finalFolderString, allFileName);
            using (StreamWriter sw = File.CreateText(pathString))
            {
                foreach(var file in sourceList)
                {
                    sw.WriteLine(file);
                }
            }
            pathString = System.IO.Path.Combine(finalFolderString, otherFileName);
            using (StreamWriter sw = File.CreateText(pathString))
            {
                foreach (var file in compareList)
                {
                    sw.WriteLine(file);
                }
            }
        }
    }
}
