using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace JWT_Token.Models
{
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext appDbContext;
        private readonly IConfiguration configuration;
        public StudentRepository(AppDbContext appDbContext, IConfiguration configuration)
        {
            this.appDbContext = appDbContext;
            this.configuration = configuration;
        }
        async Task<Student> IStudentRepository.AddStudent(Student student)
        {
            var result = await appDbContext.Students.AddAsync(student);
            await appDbContext.SaveChangesAsync();
            return result.Entity;
        }

        async Task IStudentRepository.Delete(int id)
        {
            var result = await appDbContext.Students.FirstOrDefaultAsync(x => x.StudentId == id);
            if(result != null)
            {
                appDbContext.Students.Remove(result);
                await appDbContext.SaveChangesAsync();
            }
        }

         string IStudentRepository.GenerateToken(string username, string password)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(configuration["Jwt:Issuer"], configuration["Jwt:Audience"], null,
                expires: DateTime.Now.AddMinutes(1),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        async Task<IEnumerable<Student>> IStudentRepository.GetAllStudents()
        {
            return await appDbContext.Students.ToListAsync();
        }

        async Task<Student> IStudentRepository.GetStudentByEmail(string email)
        {
            return await appDbContext.Students.FirstOrDefaultAsync(x => x.Email ==email);
        }

        async Task<Student> IStudentRepository.GetStudentById(int id)
        {
            return await appDbContext.Students.FirstOrDefaultAsync(x => x.StudentId ==id);
        }

        async Task<Student> IStudentRepository.Login(string username, string password)
        {
           return await appDbContext.Students.FirstOrDefaultAsync(x =>x.StudentName == username && x.Password == password);
            
        }

        async Task<Student> IStudentRepository.UpdateStudent(Student student)
        {
            var result = await appDbContext.Students.FirstOrDefaultAsync(x => x.StudentId == student.StudentId);
            if(result != null)
            {
                result.StudentName =student.StudentName;
                result.Address = student.Address;
                result.Age = student.Age;
                result.Email = student.Email;
                result.Gender =student.Gender;
                result.Password = student.Password;
                await appDbContext.SaveChangesAsync();
            }
            return null;
        }
    }
}
