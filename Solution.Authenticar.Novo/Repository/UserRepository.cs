using Solution.Authenticar.Novo.Contract;
using Solution.Authenticar.Novo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Solution.Authenticar.Novo.Repository
{
    public static class UserRepository
    {
        public static User Get(UserRequest request)
        {
            List<User> user = new List<User>();
            user.Add(new User { Id = 1, Nome="flavio", Senha="123", Role="Admin"});
            user.Add(new User { Id = 2, Nome = "augusto", Senha = "123", Role = "User" });

            return user.Where(x => x.Nome == request.Nome && x.Senha == request.Senha).FirstOrDefault();
        }

        public static List<User> GetAll()
        {
            List<User> user = new List<User>();
            user.Add(new User { Id = 1, Nome = "flavio", Senha = "123", Role = "Admin" });
            user.Add(new User { Id = 2, Nome = "augusto", Senha = "123", Role = "User" });

            return user;
        }
    }
}
