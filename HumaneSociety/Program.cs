using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumaneSociety
{
    class Program
    {
        static void Main(string[] args)
        {
			Employee employee = new Employee();
			Query.DisplayEmployeeRecord(employee, "Elric");
            PointOfEntry.Run();
        }
    }
}
