using CollegeApp.Repository;
using CollegeApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;

namespace CollegeApp.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        [HttpGet]
        [Route("All", Name = "GetAllStudents")]

        public ActionResult<IEnumerable<StudentDTO>> GetAllStudents()
        { 

            var students = CollegeRepository.Students.Select(s => new StudentDTO()
            {
                Id = s.Id,
                Name = s.Name,
                Email = s.Email,
                Address = s.Address
            });

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

            var student = CollegeRepository.Students.Where(s => s.Id == id).FirstOrDefault();

            //Not Found - 404
            if (student == null)
                return NotFound($"Student with id {id} Not Found");

            var studentDTO =  new StudentDTO
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                Address = student.Address
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


            var student = CollegeRepository.Students.Where(s => s.Name == name).FirstOrDefault();
            //Not Found - 404
            if (student == null)
                return NotFound($"Student with name {name} Not Found");

            var studentDTO = new StudentDTO
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                Address = student.Address
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

            int newID = CollegeRepository.Students.LastOrDefault().Id + 1;

            Student student = new Student
            {
                Id = newID,
                Name = model.Name,
                Email = model.Email,
                Address = model.Address
            };
            CollegeRepository.Students.Add(student);

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

            var existingStudent = CollegeRepository.Students.Where(s =>  s.Id == model.Id).FirstOrDefault();

            if(existingStudent == null)
                return NotFound($"Student not found");

            existingStudent.Name = model.Name;
            existingStudent.Email = model.Email;
            existingStudent.Address = model.Address;

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

            var existingStudent = CollegeRepository.Students.Where(s => s.Id == id).FirstOrDefault();

            if (existingStudent == null)
                return NotFound($"Student not found");

            var studentDTO = new StudentDTO
            {
                Id = existingStudent.Id,
                Name = existingStudent.Name,
                Email = existingStudent.Email,
                Address = existingStudent.Address,

            };

            patchDocument.ApplyTo(studentDTO);

            
            existingStudent.Name    = studentDTO.Name;
            existingStudent.Email   = studentDTO.Email;
            existingStudent.Address = studentDTO.Address;

            return NoContent();
        }




        [HttpDelete]
        [Route("{id:int}", Name = "DeleteStudentByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //api/Student/delete/1
        //
        public ActionResult<bool> DeleteStudentById(int id)
        {
            //BadRequest
            if (id <= 0)
               return BadRequest("Id is required");

            var student = CollegeRepository.Students.Where(s => s.Id == id).FirstOrDefault();

            //NotFound
            if (student == null)
                return NotFound($"Student with id {id} not exists");

            CollegeRepository.Students.Remove(student);

            //Success
            return Ok(true);
        }
    }
}
