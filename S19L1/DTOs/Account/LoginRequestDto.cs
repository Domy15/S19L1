namespace S19L1.DTOs.Account
{
    public class LoginRequestDto
    {
        public required string Email { get; set; }    
        public required string Password { get; set; }
    }
}
