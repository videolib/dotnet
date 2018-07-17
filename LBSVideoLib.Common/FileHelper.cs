using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LBFVideoLib.Common
{
    class FileHelper
    {
        // Delete file
        public void DeleteFile(string filePath)
        {
            System.IO.File.Delete(filePath);
        }
    }
}
