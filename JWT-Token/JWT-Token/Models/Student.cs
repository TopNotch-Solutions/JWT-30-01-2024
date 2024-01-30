using System.ComponentModel.DataAnnotations;

namespace JWT_Token.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentSurname { get; set; }
        public string Password { get; set; }
        public Gender Gender { get; set; }
        public int Age {  get; set; }
        public string Email {  get; set; }
        public string Address { get; set; }
    }
}
