using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Entites
{
    public class FileDto
    {
        public string FilePath { get; set; }
        public Stream Stream { get; set; }
    }
}
