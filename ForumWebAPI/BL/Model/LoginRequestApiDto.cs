
namespace ForumWebAPI.BL.Model {
    public class LoginRequestApiDto {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
