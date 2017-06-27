using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Data.Linq;

namespace TzedakahFund.Data
{
    public class UserManager
    {
        private string _connectionString;

        public UserManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddUser(string firstName, string lastName, string email, string password)
        {
            string salt = GenerateSalt();
            string hash = HashPassword(password, salt);

            using (var context = new TzedakahFundDataContext(_connectionString))
            {
                User user = new User
                {
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    PasswordHash = hash,
                    PasswordSalt = salt
                };
                context.Users.InsertOnSubmit(user);
                context.SubmitChanges();
            }
        }

        public User Login(string emailAddress, string password)
        {
            User user = GetUser(emailAddress);
            if (user == null)
            {
                return null;
            }

            bool isMatch = IsMatch(password, user.PasswordHash, user.PasswordSalt);
            if (isMatch)
            {
                return user;
            }

            return null;
        }

        public User GetUser(string email)
        {
            using (var context = new TzedakahFundDataContext(_connectionString))
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<User>(u => u.Applications);
                loadOptions.LoadWith<Application>(a => a.Category);
                context.LoadOptions = loadOptions;
                return context.Users.FirstOrDefault(u => u.Email == email);
            }
           
        }
        public User GetUser(int id)
        {
            using (var context = new TzedakahFundDataContext(_connectionString))
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<User>(u => u.Applications);
                loadOptions.LoadWith<Application>(a => a.Category);
                context.LoadOptions = loadOptions;
                return context.Users.FirstOrDefault(u => u.Id == id);
            }
        }
        public bool UserExists(string email)
        {
            using (var context = new TzedakahFundDataContext(_connectionString))
            {
                return context.Users.Any(u => u.Email == email);
            }
        }
        private static string HashPassword(string password, string salt)
        {
            SHA256Managed crypt = new SHA256Managed();

            string combinedString = password + salt;
            byte[] combined = Encoding.Unicode.GetBytes(combinedString);

            byte[] hash = crypt.ComputeHash(combined);
            return Convert.ToBase64String(hash);
        }

        private static string GenerateSalt()
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            byte[] bytes = new byte[10];
            provider.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        private static bool IsMatch(string passwordToCheck, string hashedPassword, string salt)
        {
            string hash = HashPassword(passwordToCheck, salt);
            return hash == hashedPassword;
        }

    }
}