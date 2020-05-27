using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalProtect.Models
{
    public class Image
    {
        public int id { get; set; }
        public string path { get; set; }
        public string sha { get; set; }

    }
}
