
using CollegeApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using CollegeApp.Data;

namespace CollegeApp.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly CollegeDBContext _dBContext;

        public StudentController(CollegeDBContext dBContext)
        {
            _dBContext = dBContext;
        }

        [HttpGet]
        [Route("All", Name = "GetAllStudents")]
        public ActionResult<IEnumerable<StudentDTO>> GetAllStudents()
        {
            //var students = _dBContext.Students.ToList();
            var students = _dBContext.Students.Select(s => new StudentDTO()
            {
                Id      = s.Id,
                Name    = s.StudentName,
                Email   = s.Email,
                Address = s.Address,
                DOB     = s.DOB
            }).ToList();  

            return Ok(students);
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetStudentByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //api/Student/id
        public ActionResult<StudentDTO> GetStudentById(int id)
        {
            //BadRequest - 400 - Client error
            if (id <= 0)
                return BadRequest();

            var student = _dBContext.Students.Where(s => s.Id == id).FirstOrDefault();

            //Not Found - 404
            if (student == null)
                return NotFound($"Student with id {id} Not Found");

            var studentDTO =  new StudentDTO
            {
                Id      = student.Id,
                Name    = student.StudentName,
                Email   = student.Email,
                Address = student.Address,
                DOB     = student.DOB
            };

            //Ok - Success 200
            return Ok(student);
        }


        [HttpGet]
        [Route("{name:alpha}", Name = "GetStudentByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //api/Student/name
        public ActionResult<StudentDTO> GetStudentByName(string name)
        {
            //BadRequest - 400 - Client error
            if (string.IsNullOrEmpty(name))
                return BadRequest("Name is required");


            var student = _dBContext.Students.Where(s => s.StudentName == name).FirstOrDefault();
            //Not Found - 404
            if (student == null)
                return NotFound($"Student with name {name} Not Found");

            var studentDTO = new StudentDTO
            {
                Id      = student.Id,
                Name    = student.StudentName,
                Email   = student.Email,
                Address = student.Address,
                DOB     = student.DOB
            };

            //Ok - Success 200
            return Ok(student);
            
        }

        [HttpPost]
        [Route("Create", Name = "CreateStudent")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //api/student/create
        public ActionResult<StudentDTO> CreateStudent([FromBody]StudentDTO model)
        {
            //if(!ModelState.IsValid)
            //    return BadRequest(ModelState);

            if (model == null)
                return BadRequest();

            

            Student student = new Student
            {
                
                StudentName = model.Name,
                Email       = model.Email,
                Address     = model.Address,
                DOB         = model.DOB
            };
            _dBContext.Students.Add(student);
            _dBContext.SaveChanges();
            model.Id = student.Id;

            //201 - Created
            //creates new url for the new id
            //returns new student details
            return CreatedAtRoute("GetStudentByID", new { id = model.Id }, model);
        }

        [HttpPut]
        [Route("Update", Name = "UpdateStudent")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //api/Student/update
        public ActionResult<StudentDTO> UpdateStudent([FromBody]StudentDTO model)
        {
            if(model == null || model.Id <= 0)
                return BadRequest();

            var existingStudent = _dBContext.Students.Where(s =>  s.Id == model.Id).FirstOrDefault();

            if(existingStudent == null)
                return NotFound($"Student not found");

            existingStudent.StudentName = model.Name;
            existingStudent.Email       = model.Email;
            existingStudent.Address     = model.Address;
            existingStudent.DOB         = model.DOB;
            _dBContext.SaveChanges();

            return NoContent();
        }


        [HttpPatch]
        [Route("{id:int}/UpdatePartial", Name = "UpdateStudentPartial")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //api/Student/1/update
        public ActionResult<StudentDTO> UpdateStudentPartial(int id, [FromBody] JsonPatchDocument patchDocument)
        {
            if (patchDocument == null || id <= 0)
                return BadRequest();

            var existingStudent = _dBContext.Students.Where(s => s.Id == id).FirstOrDefault();

            if (existingStudent == null)
                return NotFound($"Student not found");

            var studentDTO = new StudentDTO
            {
                Id      = existingStudent.Id,
                Name    = existingStudent.StudentName,
                Email   = existingStudent.Email,
                Address = existingStudent.Address,
                DOB     = existingStudent.DOB,
            };

            patchDocument.ApplyTo(studentDTO);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            existingStudent.StudentName     = studentDTO.Name;
            existingStudent.Email           = studentDTO.Email;
            existingStudent.Address         = studentDTO.Address;
            existingStudent.DOB             = studentDTO.DOB;
            _dBContext.SaveChanges();

            return NoContent();
        }


        [HttpDelete]
        [Route("{id:int}", Name = "DeleteStudentByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //api/Student/delete/1
        //example
        public ActionResult<bool> DeleteStudentById(int id)
        {
            //BadRequest
            if (id <= 0)
               return BadRequest("Id is required");

            var student = _dBContext.Students.Where(s => s.Id == id).FirstOrDefault();

            //NotFound
            if (student == null)
                return NotFound($"Student with id {id} not exists");

            _dBContext.Students.Remove(student);
            _dBContext.SaveChanges();

            //Success
            return Ok(true);
        }
    }
}
