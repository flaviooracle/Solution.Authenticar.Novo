using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Solution.Authenticar.Novo.Contract;
using Solution.Authenticar.Novo.Models;
using Solution.Authenticar.Novo.Repository;
using Solution.Authenticar.Novo.Service;

namespace Solution.Authenticar.Novo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CadastroController : ControllerBase
    {
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> GetToken([FromBody] TokenRequest tokenRequest)
        {
            if (tokenRequest.Nome == null)
                return BadRequest("Usuário deve ser informado.");

            var userDb = UserRepository.Get(new UserRequest {Nome = tokenRequest.Nome, Senha = tokenRequest.Senha });
            if(userDb.Nome == null)
                return BadRequest("Usuário não encontrado.");

            var userToken = new User()
            {
                Nome = userDb.Nome,
                Senha = userDb.Senha,
                Role = userDb.Role
            };

            var token = TokenService.GenerateToken(userDb);
           
            var rtoken = new {
                userToken, 
                token 
            };
            
            return Ok(rtoken);
        }

        [HttpGet]
        [Route("Any")]
        [Authorize]
      //  [Authorize("User")]
      //  [Authorize("Admin")]
        public string GetAnyUser()
        {
            return String.Format("Autenticado - {0}", User.Identity.Name);
        }

        [HttpGet]
        [Route("Admin")]
        [Authorize(Roles = "Admin")]
        //  [Authorize("User")]
        //  [Authorize("Admin")]
        public string GetOnlyAdmin(int id)
        {
            return String.Format("Autenticado - {0}", User.Identity.Name);
            
        }

        [HttpGet]
        [AllowAnonymous]
        public string anonimo() => "Anonimo";

    }
}
