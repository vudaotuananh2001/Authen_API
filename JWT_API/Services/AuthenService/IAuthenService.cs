using JWT_API.DTO;
using JWT_API.Models;

namespace JWT_API.Services.AuthenService
{
    public interface IAuthenService
    {
        User Register(UserDTO userDTO);
        string Login (UserLoginDTO loginDTO);
    }
}
