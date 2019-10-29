using Newtonsoft.Json;

namespace ESDemo
{
    public class MyDocument
    {
        public string Title { get; set; }
        public string Notes { get; set; }

        public override string ToString()
        {
            // This will return the class in JSON format
            return JsonConvert.SerializeObject(this);
        }
    }
}
