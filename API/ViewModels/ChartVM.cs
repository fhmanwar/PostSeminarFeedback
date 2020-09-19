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
        public string Title { get; set; }
        public int Total { get; set; }
    }
    public class BarChartVM
    {
        public string date { get; set; }
        public string car { get; set; }
        public int days { get; set; }
    }

    public class TopTrainingVM
    {
        public string Title { get; set; }
        public string Trainer { get; set; }
        public string TypeTraining { get; set; }
        public double Rate { get; set; }
    }
}
