using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.Models
{
    public class UserEmailOptions
    {
        public string ToEmail { get; set; }
        public String Subject { get; set; }
        public string TemplateName { get; set; }

        public List<KeyValuePair<string,string>> PlaceHolders { get; set; }
        public string Heading { get; set; }

    }
}
