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
<<<<<<< HEAD
      PointOfEntry.Run();
=======
			//PointOfEntry.Run();
>>>>>>> e71a87e7ad3f41e9ed55e8201dd0597f80994318

      /*Employee employee = new Employee();*/
      //employee.EmployeeId = 6;
      //Query.DeleteEmployeeRecord(employee);

      //Query.AddNewEmployee();
      //Console.ReadLine();
      /*Admin admin = new Admin();
				  admin.LogIn();*/

      //Animal animal = new Animal(); //Testing to see if UpdateShots works
      //animal.AnimalId = 2;
      //Query.UpdateShot("RabiesShot", animal);
      //Console.ReadLine();

      //Employee employee = new Employee(); //Testing to see if deleteEmployee method works properly
      //employee.EmployeeId = 4;
      //Query.RunEmployeeQueries(employee, "delete");

			UserEmployee employee = new UserEmployee();
			employee.LogIn();


    }
  }
}
