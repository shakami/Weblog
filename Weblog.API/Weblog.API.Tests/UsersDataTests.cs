using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Weblog.API.DbContexts;
using Weblog.API.Entities;
using Weblog.API.ResourceParameters;
using Weblog.API.Services;

namespace Weblog.API.Tests
{
    [TestClass]
    public class UsersDataTests
    {
        private static SqliteConnection _connection;
        private static WeblogContext _context;
        private static IWeblogDataRepository _repository;
        private static UsersResourceParameters _resourceParameters;

        [TestInitialize]
        public void TestInitialize()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<WeblogContext>()
                .UseSqlite(_connection)
                .Options;

            _context = new WeblogContext(options);
            _context.Database.EnsureCreated();

            _repository = new WeblogDataRepository(_context);

            _resourceParameters = new UsersResourceParameters
            {
                PageNumber = 1,
                PageSize = 10
            };
        }

        [TestMethod]
        public void AddUser()
        {
            //-- arrange
            var countBeforeAdd = _repository.GetUsers(_resourceParameters).Count;

            var user = new User
            {
                FirstName = "fname",
                LastName = "lname",
                EmailAddress = "email@users",
                Password = "secret"
            };

            //-- act
            _repository.AddUser(user);
            _repository.Save();

            var actual = _repository.GetUsers(_resourceParameters).Count;

            //-- assert
            Assert.AreEqual(countBeforeAdd + 1, actual);

            //-- clean up
            _repository.DeleteUser(user);
            _repository.Save();
        }

        [TestMethod]
        public void GetUsers()
        {
            //-- arrange
            var countBeforeAdd = _repository.GetUsers(_resourceParameters).Count;

            var users = new List<User>
            {
                new User
                {
                    FirstName = "fname1",
                    LastName = "lname1",
                    EmailAddress = "1@users",
                    Password = "secret1"
                },
                new User
                {
                    FirstName = "fname2",
                    LastName = "lname2",
                    EmailAddress = "2@users",
                    Password = "secret2"
                },
                new User
                {
                    FirstName = "fname3",
                    LastName = "lname3",
                    EmailAddress = "3@users",
                    Password = "secret3"
                }
            };

            foreach (var user in users)
            {
                _repository.AddUser(user);
            }
            _repository.Save();

            //-- act
            var actual = _repository.GetUsers(_resourceParameters);

            //-- assert
            Assert.AreEqual("fname1", actual.First().FirstName);
            Assert.AreEqual(countBeforeAdd + 3, actual.Count());

            //-- cleanup
            foreach (var user in users)
            {
                _repository.DeleteUser(user);
            }
            _repository.Save();
        }

        [TestMethod]
        public void GetUser()
        {
            //-- arrange
            var user = new User
            {
                FirstName = "fname",
                LastName = "lname",
                EmailAddress = "email@users",
                Password = "secret"
            };

            _repository.AddUser(user);
            _repository.Save();

            //-- act
            var actual = _repository.GetUser(1);

            //-- assert
            Assert.AreEqual("fname", actual.FirstName);

            //-- cleanup
            _repository.DeleteUser(user);
            _repository.Save();
        }

        [TestMethod]
        public void UpdateUser()
        {
            //-- arrange
            var user = new User
            {
                FirstName = "fname",
                LastName = "lname",
                EmailAddress = "old@email",
                Password = "secret"
            };

            _repository.AddUser(user);
            _repository.Save();

            user.EmailAddress = "new@email";

            //-- act
            _repository.UpdateUser(user);
            _repository.Save();

            var actual = _repository.GetUser(1);

            //-- assert
            Assert.AreEqual("new@email", actual.EmailAddress);

            //-- cleanup
            _repository.DeleteUser(user);
            _repository.Save();
        }

        [TestMethod]
        public void DeleteUser()
        {
            //-- arrange
            var user = new User
            {
                FirstName = "fname",
                LastName = "lname",
                EmailAddress = "email@users",
                Password = "secret"
            };

            _repository.AddUser(user);
            _repository.Save();

            var countBeforeDelete = _repository.GetUsers(_resourceParameters).Count();

            //-- act
            _repository.DeleteUser(user);
            _repository.Save();

            //-- assert
            var actual = _repository.GetUsers(_resourceParameters).Count();

            Assert.AreEqual(countBeforeDelete - 1, actual);
        }

        [TestMethod]
        public void UserExists()
        {
            //-- arrange
            var user = new User
            {
                FirstName = "fname",
                LastName = "lname",
                EmailAddress = "email@users",
                Password = "secret"
            };

            _repository.AddUser(user);
            _repository.Save();

            //-- act
            var actual = _repository.UserExists(1);

            //-- assert
            Assert.IsTrue(actual);

            //-- clean up
            _repository.DeleteUser(user);
            _repository.Save();
        }

        [TestMethod]
        public void UserExistsInvalidID()
        {
            //-- arrange

            //-- act
            var actual = _repository.UserExists(1);

            //-- assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void Authenticate()
        {
            //-- arrange
            var user = new User
            {
                FirstName = "fname",
                LastName = "lname",
                EmailAddress = "email@users",
                Password = "secret"
            };

            _repository.AddUser(user);
            _repository.Save();

            //-- act
            var actual = _repository.Authenticate("email@users", "secret");

            //-- assert
            Assert.AreEqual(user, actual);

            //-- clean up
            _repository.DeleteUser(user);
            _repository.Save();
        }

        [TestMethod]
        public void AuthenticateInvalidData()
        {
            //-- arrange
            var user = new User
            {
                FirstName = "fname",
                LastName = "lname",
                EmailAddress = "email@users",
                Password = "secret"
            };

            _repository.AddUser(user);
            _repository.Save();

            //-- act
            var actual1 = _repository.Authenticate("bad@email", "secret");
            var actual2 = _repository.Authenticate("email@users", "password");

            //-- assert
            Assert.AreNotEqual(user, actual1);
            Assert.IsNull(actual1);
            
            Assert.AreNotEqual(user, actual2);
            Assert.IsNull(actual2);

            //-- clean up
            _repository.DeleteUser(user);
            _repository.Save();
        }

        [TestMethod]
        public void Authorized()
        {
            //-- arrange
            var user = new User
            {
                FirstName = "fname",
                LastName = "lname",
                EmailAddress = "email@users",
                Password = "secret"
            };

            _repository.AddUser(user);
            _repository.Save();

            //-- act
            var actual = _repository.Authorized(1, "email@users", "secret");

            //-- assert
            Assert.IsTrue(actual);

            //-- clean up
            _repository.DeleteUser(user);
            _repository.Save();
        }

        [TestMethod]
        public void AuthorizedInvalidData()
        {
            //-- arrange
            var user = new User
            {
                FirstName = "fname",
                LastName = "lname",
                EmailAddress = "email@users",
                Password = "secret"
            };

            _repository.AddUser(user);
            _repository.Save();

            //-- act
            var actual1 = _repository.Authorized(0, "email@users", "secret");
            var actual2 = _repository.Authorized(1, "bad@email", "secret");
            var actual3 = _repository.Authorized(1, "email@users", "password");

            //-- assert
            Assert.IsFalse(actual1);
            Assert.IsFalse(actual2);
            Assert.IsFalse(actual3);

            //-- clean up
            _repository.DeleteUser(user);
            _repository.Save();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _connection.Close();
        }
    }
}
