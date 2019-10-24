using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Elasticsearch1
{
    public class MyDocument
    {
        public string Title { get; set; }
        public string Notes { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
