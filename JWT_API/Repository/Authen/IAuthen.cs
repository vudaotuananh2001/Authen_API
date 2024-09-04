using JWT_API.DTO;
using JWT_API.Models;

namespace JWT_API.Repository.Authen
{
    public interface IAuthen
    {
        User Register (UserDTO userDTO);
        string Login (UserLoginDTO loginDTO);
    }
}
