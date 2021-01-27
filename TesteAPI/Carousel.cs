using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TesteAPI
{
    public class Carousel
    {
        public string itemType { get; set; }
        public List<Header> items { get; set; }

    }
    public class Header
    {
        public HeaderBody header { get; set; }
    }

    public class HeaderBody
    {
        public string type { get; set; }
        public Value value { get; set; }
    }

    public class Value
    {
        public string title { get; set; }
        public string text { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }
}
