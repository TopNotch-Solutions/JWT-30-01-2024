namespace JWT_Token.Models
{
    public interface IStudentRepository
    {
        Task<Student> AddStudent(Student student);
        Task<IEnumerable<Student>> GetAllStudents();
        Task<Student> GetStudentById(int id);
        Task<Student> GetStudentByEmail(string email);
        Task<Student> Login(string username, string password);
        Task<Student> UpdateStudent(Student student);
        string GenerateToken(string username, string password);
        Task Delete(int id);
    }
}
