using FluentBlog.Infrastructure;
using FluentBlog.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace FluentBlog.DataRepositories
{
    public class CustomUserManager : UserManager<User>
    {
        public CustomUserManager(IUserStore<User> store,
                                 IOptions<IdentityOptions> optionsAccessor,
                                 IPasswordHasher<User> passwordHasher,
                                 IEnumerable<IUserValidator<User>> userValidators,
                                 IEnumerable<IPasswordValidator<User>> passwordValidators,
                                 ILookupNormalizer keyNormalizer,
                                 IdentityErrorDescriber errors,
                                 System.IServiceProvider services,
                                 ILogger<UserManager<User>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {

        }

        // 用id查用户
        public User GetUserById(string id)
        {
            User user = Users.FirstOrDefault(u => u.Id.Equals(id));
            return user;
        }
}
}