using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkshopProject.System_Service
{
     public static class InitSystem
    {
        public static string filePath = "../../../hadas.txt";
        
        public static void updatePath(string new_path) { filePath = new_path; }

        public static void initSystem()
        {
            string[] lines = System.IO.File.ReadAllLines(@filePath);

        }
    }
}
