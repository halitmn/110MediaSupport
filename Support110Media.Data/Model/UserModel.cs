using System.ComponentModel.DataAnnotations;

namespace Support110Media.Data.Model
{
    public class UserModel
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string MailAddress { get; set; }
    }
}
