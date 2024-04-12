using System;

namespace LegacyApp
{
    public class UserService
    {
        static DateTime limit = DateTime.Now.addYears(-21);
        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {

            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || (!email.Contains("@") && !email.Contains(".")) || dateOfBirth > limit)
            {
                return false;
            }

            var clientRepository = new ClientRepository();
            var client = clientRepository.GetById(clientId);
            var userCreditService = new UserCreditService();

            var user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName,
                HasCreditLimit = !(client.Type == "VeryImportantClient"),
                CreditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth)

            };

            if (client.Type == "ImportantClient")
            {
                user.HasCreditLimit *= 2;
            }
            
            if (user.HasCreditLimit && user.CreditLimit < 500)
            {
                return false;
            }

            UserDataAccess.AddUser(user);
            return true;
        }
    }
}
