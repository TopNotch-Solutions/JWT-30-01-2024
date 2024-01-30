using JWT_Token.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JWT_Token.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepository studentRepository;
        public StudentController(IStudentRepository studentRepository)
        {
            this.studentRepository = studentRepository;
        }
        //[Route("CreateStudent")]
        [AllowAnonymous]
        [HttpPost("CreateStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Student>> CreateStudent(Student student)
        {
            try
            {
                if (student == null)
                {
                    return BadRequest();
                }
                var result = await studentRepository.GetStudentByEmail(student.Email);
                if(result != null)
                {
                    return BadRequest("Student already exists");
                }
                var createStudent = await studentRepository.AddStudent(student);
                return CreatedAtAction(nameof(studentRepository.GetStudentById), new { id = createStudent.StudentId }, createStudent);
            }
            catch(Exception ex) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }
        [Authorize]
        [Route("AllStudents")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAllStudents()
        {
            try
            {
                return Ok(await studentRepository.GetAllStudents());
            }catch(Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        [Authorize]
        [Route("AllStudentsById")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Student>> GetASingleStudent(int id)
        {
            try
            {
                var result = await studentRepository.GetStudentById(id);
                if(result == null)
                {
                    return NotFound("Student not found");
                }
                return Ok(result);
            }
            catch(Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        [Authorize]
        [Route("AllStudentsByEmail")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Student>> GetASingleStudent(string email)
        {
            try
            {
                var result = await studentRepository.GetStudentByEmail(email);
                if (result == null)
                {
                    return NotFound("Student not found");
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        [AllowAnonymous]
        [Route("Login")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Student>> Login(string name , string password)
        {
            try
            {
                var result = await studentRepository.Login(name, password);
                if (result == null)
                {
                    return NotFound("Invalid credentials");
                }
                var token =  studentRepository.GenerateToken(name, password);
                return Ok(new {token = token,
                    studentId = result.StudentId,
                    studentName = result.StudentName,
                    studentSurname = result.StudentSurname,
                    studentAge = result.Age,
                    studentAddress = result.Address,
                    studentGender = result.Gender,
                    studentEmail = result.Email
                });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        [Authorize]
        [Route("Update")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Student>> Update(Student student)
        {
            try
            {
                if(student == null)
                {
                    return BadRequest();
                }
                var studentUpdate =await studentRepository.GetStudentById(student.StudentId);
                if(studentUpdate == null)
                {
                    return NotFound("Student not found");
                }
                return await studentRepository.UpdateStudent(student);
            }catch(Exception e) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        [Authorize]
        [Route("Delete")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteStudent(int id)
        {
            try
            {
                var result = await studentRepository.GetStudentById(id);
                if (result == null)
                {
                    return NotFound();
                }
                await studentRepository.Delete(id);
                return Ok($"Student {id} has been deleted!");

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
