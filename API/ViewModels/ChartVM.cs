using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels
{
    public class ChartVM
    {
    }

    public class PieChartVM
    {
        public string DepartmentName { get; set; }
        public int total { get; set; }
    }
    public class BarChartVM
    {
        public string date { get; set; }
        public string car { get; set; }
        public int days { get; set; }
    }
}
