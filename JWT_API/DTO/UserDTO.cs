namespace JWT_API.DTO
{
    public class UserDTO
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int Role_Id { get; set; }
        public bool Status  { get; set; } =true;  // default là true
    }
}
