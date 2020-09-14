using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class Helper
    {
    }
    public class AttrEmail
    {
        public string mail = "rio.mii.b36@gmail.com";
        public string pass = "bootcamp36";
    }

    public class RandomDigit
    {
        private Random _random = new Random();
        public string GenerateRandom()
        {
            return _random.Next(0, 9999).ToString("D4");
        }
    }
}
