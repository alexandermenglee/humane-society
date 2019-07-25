using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumaneSociety
{
    public static class Query
    {        
        static HumaneSocietyDataContext db;

        static Query()
        {
            db = new HumaneSocietyDataContext();
        }

        internal static List<USState> GetStates()
        {
            List<USState> allStates = db.USStates.ToList();       

            return allStates;
        }
            
        internal static Client GetClient(string userName, string password)
        {
            Client client = db.Clients.Where(c => c.UserName == userName && c.Password == password).Single();

            return client;
        }

        internal static List<Client> GetClients()
        {
            List<Client> allClients = db.Clients.ToList();

            return allClients;
        }

        internal static void AddNewClient(string firstName, string lastName, string username, string password, string email, string streetAddress, int zipCode, int stateId)
        {
            Client newClient = new Client();

            newClient.FirstName = firstName;
            newClient.LastName = lastName;
            newClient.UserName = username;
            newClient.Password = password;
            newClient.Email = email;

            Address addressFromDb = db.Addresses.Where(a => a.AddressLine1 == streetAddress && a.Zipcode == zipCode && a.USStateId == stateId).FirstOrDefault();

            // if the address isn't found in the Db, create and insert it
            if (addressFromDb == null)
            {
                Address newAddress = new Address();
                newAddress.AddressLine1 = streetAddress;
                newAddress.City = null;
                newAddress.USStateId = stateId;
                newAddress.Zipcode = zipCode;                

                db.Addresses.InsertOnSubmit(newAddress);
                db.SubmitChanges();

                addressFromDb = newAddress;
            }

            // attach AddressId to clientFromDb.AddressId
            newClient.AddressId = addressFromDb.AddressId;

            db.Clients.InsertOnSubmit(newClient);

            db.SubmitChanges();
        }

        internal static void UpdateClient(Client clientWithUpdates)
        {
            // find corresponding Client from Db
            Client clientFromDb = null;

            try
            {
                clientFromDb = db.Clients.Where(c => c.ClientId == clientWithUpdates.ClientId).Single();
            }
            catch(InvalidOperationException e)
            {
                Console.WriteLine("No clients have a ClientId that matches the Client passed in.");
                Console.WriteLine("No update have been made.");
                return;
            }
            
            // update clientFromDb information with the values on clientWithUpdates (aside from address)
            clientFromDb.FirstName = clientWithUpdates.FirstName;
            clientFromDb.LastName = clientWithUpdates.LastName;
            clientFromDb.UserName = clientWithUpdates.UserName;
            clientFromDb.Password = clientWithUpdates.Password;
            clientFromDb.Email = clientWithUpdates.Email;

            // get address object from clientWithUpdates
            Address clientAddress = clientWithUpdates.Address;

            // look for existing Address in Db (null will be returned if the address isn't already in the Db
            Address updatedAddress = db.Addresses.Where(a => a.AddressLine1 == clientAddress.AddressLine1 && a.USStateId == clientAddress.USStateId && a.Zipcode == clientAddress.Zipcode).FirstOrDefault();

            // if the address isn't found in the Db, create and insert it
            if(updatedAddress == null)
            {
                Address newAddress = new Address();
                newAddress.AddressLine1 = clientAddress.AddressLine1;
                newAddress.City = null;
                newAddress.USStateId = clientAddress.USStateId;
                newAddress.Zipcode = clientAddress.Zipcode;                

                db.Addresses.InsertOnSubmit(newAddress);
                db.SubmitChanges();

                updatedAddress = newAddress;
            }

            // attach AddressId to clientFromDb.AddressId
            clientFromDb.AddressId = updatedAddress.AddressId;
            
            // submit changes
            db.SubmitChanges();
        }
        
        internal static void AddUsernameAndPassword(Employee employee)
        {
            Employee employeeFromDb = db.Employees.Where(e => e.EmployeeId == employee.EmployeeId).FirstOrDefault();

            employeeFromDb.UserName = employee.UserName;
            employeeFromDb.Password = employee.Password;

            db.SubmitChanges();
        }

        internal static Employee RetrieveEmployeeUser(string email, int employeeNumber)
        {
            Employee employeeFromDb = db.Employees.Where(e => e.Email == email && e.EmployeeNumber == employeeNumber).FirstOrDefault();

            if (employeeFromDb == null)
            {
                throw new NullReferenceException();
            }
            else
            {
                return employeeFromDb;
            }
        }

        internal static Employee EmployeeLogin(string userName, string password)
        {
            Employee employeeFromDb = db.Employees.Where(e => e.UserName == userName && e.Password == password).FirstOrDefault();

            return employeeFromDb;
        }

        internal static bool CheckEmployeeUserNameExist(string userName)
        {
            Employee employeeWithUserName = db.Employees.Where(e => e.UserName == userName).FirstOrDefault();

            return employeeWithUserName == null;
        }


		//// TODO Items: ////

		// TODO: Allow any of the CRUD operations to occur here
		internal static void RunEmployeeQueries(Employee employee, string crudOperation)
		{
			switch (crudOperation.ToLower())
			{
				case "create":
					AddEmployeeRecord(employee);
					break;
				case "read":
					ReadEmployeeRecord(employee);
					break;
				case "update":
					UpdateEmployeeRecord(employee);
					break;
				case "delete":
					DeleteEmployeeRecord(employee);
					break;
				default:
					break;
			}
		}

		public static void AddEmployeeRecord(Employee employee)
		{
			db.Employees.InsertOnSubmit(employee);

			try
			{
				db.SubmitChanges();
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				db.SubmitChanges();
			}
			
		}

		public static void ReadEmployeeRecord(Employee employee)
		{
			db.ObjectTrackingEnabled = false;
			Employee employeeRecord = db.Employees.Where(e => e.EmployeeId == employee.EmployeeId).FirstOrDefault();
				Console.WriteLine(employeeRecord.EmployeeNumber + " " + employeeRecord.FirstName + " " + employeeRecord.LastName + " " + employeeRecord.UserName + " " + employeeRecord.Email);
			Console.ReadLine();
		}

		public static void UpdateEmployeeRecord(Employee employee)
		{
			Employee foundEmployee = db.Employees.Where(e => e.EmployeeNumber == employee.EmployeeNumber).FirstOrDefault();

			if (foundEmployee == null)
			{
				Console.WriteLine("That employee number was not found, no record exists to be updated");
				Console.ReadLine();
			}

			else
			{
				db.Employees.InsertOnSubmit(employee);
			}

			try
			{
				db.SubmitChanges();
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				db.SubmitChanges();
			}
		}

		public static void DeleteEmployeeRecord(Employee employee)
		{
			var animal = db.Animals.Where(a => a.EmployeeId == employee.EmployeeId).Select(x => x);
			foreach (var item in animal)
			{
				item.EmployeeId = null;
			}

			var deleteEmployee = db.Employees.Where(e => e.EmployeeId == employee.EmployeeId).FirstOrDefault();
			db.Employees.DeleteOnSubmit(deleteEmployee);

			try
			{
				db.SubmitChanges();
				Console.WriteLine("This code worked!");
				Console.ReadLine();
			}
			catch (Exception e)
			{

				Console.WriteLine(e);
			}
		}
		public static void DisplayEmployeeRecord(Employee employee, string lastName)
		{
			var user = db.Employees.Where(e => e.LastName.Equals(lastName)).Select(e => e).FirstOrDefault();
			Console.WriteLine(user);
			Console.ReadLine();
		}

		// TODO: Animal CRUD Operations
		internal static void AddAnimal(Animal animal)
    {
      db.Animals.InsertOnSubmit(animal);

      try
      {
        db.SubmitChanges();
      }
      catch(Exception exception)
      {
        throw exception;
      }

    }

    internal static Animal GetAnimalByID(int id)
    {
      Animal animal = db.Animals.Where(a => a.AnimalId == id).Single();
      Console.WriteLine(animal.Name);
      return animal;
    }

    internal static void UpdateAnimal(int animalId, Dictionary<int, string> updates)
    {
      // Animal from DB with matching ID
      Animal animal = db.Animals.Where(a => a.AnimalId == animalId).FirstOrDefault();

      // Updates contains values to update the current animal (animalid)
      foreach (var item in updates)
      {
        switch (item.Key)
        {
          case 1:
            animal.CategoryId = int.Parse(item.Value);
            break;
          case 2:
            animal.Name = item.Value;
            break;
          case 3:
            animal.Age = int.Parse(item.Value);
            break;
          case 4:
            animal.Demeanor = item.Value;
            break;
          case 5:
            // Needs testing
            animal.KidFriendly = bool.Parse(item.Value);
            break;
          case 6:
            // Needs testing
            bool thing = bool.Parse(item.Value);
            animal.PetFriendly = thing;
            break;
          case 7:
            animal.Weight = int.Parse(item.Value);
            break;
        }

        // Save to database
        try
        {
          db.SubmitChanges();
        }
        catch(Exception exception)
        {
          throw exception;
        }
      }
    }

    internal static void RemoveAnimal(Animal animal)
    {
      // All tables with AnimalId foreign key
      /* Adoptions
         AnimalShots
         Rooms */
      IQueryable<Adoption> adoptions = db.Adoptions;
      IQueryable<AnimalShot> animalShots = db.AnimalShots;
      IQueryable<Room> rooms = db.Rooms;

      // Loop through adoptions and remove the adoption with the matching AnimalId
      foreach(var adoption in adoptions)
      {
        if(adoption.AnimalId == animal.AnimalId)
        {
          db.Adoptions.DeleteOnSubmit(adoption);
        }
      }

      // Loop through AnimalShots and remove the animalShot wit the matching AnimalId
      foreach(var animalShot in animalShots)
      {
        if(animalShot.AnimalId == animal.AnimalId)
        {
          db.AnimalShots.DeleteOnSubmit(animalShot);
        }
      }

      // Loop through Rooms and set the AnimalId to Null where the room's AnimaldId equals the animal's ID
      foreach(var room in rooms)
      {
        if(room.AnimalId == animal.AnimalId)
        {
          room.AnimalId = null;
        }
      }

      db.Animals.DeleteOnSubmit(animal);

      try
      {
        db.SubmitChanges();
      }
      catch(Exception exception)
      {
        throw exception;
      }
    }
        
        // TODO: Animal Multi-Trait Search
    internal static IQueryable<Animal> SearchForAnimalsByMultipleTraits(Dictionary<int, string> updates) // parameter(s)?
    {
      // IQueryable handles filter logic on server side
        // Less network traffic
      // IEnumerable handles filer logic on client side

      // updates = {[1, "1"], [3, "57"], [4, "Aggressive"]}

      /* Return results into IQueryable results 
       * Then loop through IQueryable results to display all results that came back with multiple traits
       */

      IQueryable<Animal> results = db.Animals;

      foreach (var update in updates)
      {
        switch (update.Key)
        {
          case 1:
            results = results.Where(a => a.CategoryId.Equals(update.Value));
            break;
          case 2:
            results = results.Where(a => a.Name.Equals(update.Value));
            break;
          case 3:
            results = results.Where(a => a.Age.Equals(update.Value));
            break;
          case 4:
            results = results.Where(a => a.Demeanor.Equals(update.Value));
            break;
          case 5:
            results = results.Where(a => a.KidFriendly.Equals(update.Value));
            break;
          case 6:
            results = results.Where(a => a.PetFriendly.Equals(update.Value));
            break;
          case 7:
            results = results.Where(a => a.Weight.Equals(update.Value));
            break;
          case 8:
            results = results.Where(a => a.AnimalId.Equals(update.Value));
            break;
        }
      }

      return results;
    }
         
        // TODO: Misc Animal Things
    internal static int GetCategoryId(string categoryName)
    {
      return db.Categories.Where(c => categoryName.Equals(c.Name)).Select(c => c.CategoryId).Single();
    }
       
        internal static Room GetRoom(int animalId)
        {
			return db.Rooms.Where(e => e.AnimalId == animalId).FirstOrDefault();
        }
        
    internal static int GetDietPlanId(string dietPlanName)
    {
      return db.DietPlans.Where(d => d.Name.Equals(dietPlanName)).Select(d => d.DietPlanId).Single();
    }

        // TODO: Adoption CRUD Operations
    internal static void Adopt(Animal animal, Client client)
    {
      Adoption newAdoption = new Adoption()
      {
        ClientId = client.ClientId,
        AnimalId = animal.AnimalId,
        AdoptionFee = 75,
        ApprovalStatus = "Pending"
      };

      //newAdoption.AnimalId = animal.AnimalId;
      //newAdoption.ClientId = client.ClientId;
      //newAdoption.AdoptionFee = 75;

      db.Adoptions.InsertOnSubmit(newAdoption);

      try
      {
        db.SubmitChanges();
      }
      catch(Exception exception)
      {
        throw exception;
      }
    }

        internal static IQueryable<Adoption> GetPendingAdoptions()
        {
            throw new NotImplementedException();
        }

        internal static void UpdateAdoption(bool isAdopted, Adoption adoption)
        {
            throw new NotImplementedException();
        }

        internal static void RemoveAdoption(int animalId, int clientId)
        {
            throw new NotImplementedException();
        }

        // TODO: Shots Stuff
        internal static IQueryable<AnimalShot> GetShots(Animal animal)
        {
			IQueryable<AnimalShot> allShots = db.AnimalShots.Where(e => e.AnimalId == animal.AnimalId);
			return allShots;
		}

        internal static void UpdateShot(string shotName, Animal animal)
        {
			//First check that the shot being passed in is in the database
			//Then add that shot to the AnimalShots table
			//**If time permits, look to add the 'shot' if it doesn't exist already in the table
			Shot foundShotId = db.Shots.Where(e => e.Name.Equals(shotName)).FirstOrDefault();
			AnimalShot newShot = new AnimalShot()
			{
				AnimalId = animal.AnimalId,
				ShotId = foundShotId.ShotId,
				DateReceived = DateTime.Now
			};

			db.AnimalShots.InsertOnSubmit(newShot);

			try
			{
				db.SubmitChanges();
				Console.WriteLine("This code worked!");
			}
			catch (Exception e)
			{

				Console.WriteLine(e);
				Console.WriteLine("This code did not work");
				db.SubmitChanges();
			}

        }
    }
}