using JWT_API.DTO;
using JWT_API.Models;
using JWT_API.Repository.Authen;

namespace JWT_API.Services.AuthenService
{
    public class AuthenServiceImpl : IAuthenService
    {
        private readonly IAuthen _authenRepository;
        public AuthenServiceImpl(IAuthen authenRepository)
        {
            this._authenRepository = authenRepository;            
        }

        public string Login(UserLoginDTO loginDTO)
        {
            try
            {
                string token  = _authenRepository.Login(loginDTO);
                return token;   
            }
            catch (Exception ex) {
                throw new Exception("Errors Login service");
            }
        }

        public User Register(UserDTO userDTO)
        {
            try
            {
                User user = _authenRepository.Register(userDTO);
                return user;    
            }
            catch (Exception ex) {
                throw new Exception("Errors Register service");
            }
        }
    }
}
