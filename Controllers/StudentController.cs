
using CollegeApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using CollegeApp.Data;
using AutoMapper;
using CollegeApp.Data.Repository;

namespace CollegeApp.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public StudentController(IMapper mapper, IStudentRepository studentRepository)
        {
            
            _mapper = mapper;
            _studentRepository = studentRepository;
        }

        [HttpGet]
        [Route("All", Name = "GetAllStudents")]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetAllStudentsAsync()
        {
            var students = await _studentRepository.GetAllAsync();

            var studentsDTOData = _mapper.Map<List<StudentDTO>>(students);

            return Ok(studentsDTOData);
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetStudentByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //api/Student/id
        public async Task<ActionResult<StudentDTO>> GetStudentByIdAsync(int id)
        {
            //BadRequest - 400 - Client error
            if (id <= 0)
                return BadRequest();

            var student = await _studentRepository.GetByIDAsync(id);

            //Not Found - 404
            if (student == null)
                return NotFound($"Student with id {id} Not Found");

            var studentDTO =  _mapper.Map<StudentDTO>(student);

            //Ok - Success 200
            return Ok(studentDTO);
        }



        [HttpGet]
        [Route("{name:alpha}", Name = "GetStudentByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //api/Student/name
        public async Task<ActionResult<StudentDTO>> GetStudentByNameAsync(string name)
        {
            //BadRequest - 400 - Client error
            if (string.IsNullOrEmpty(name))
                return BadRequest("Name is required");


            var student = await _studentRepository.GetByNameAsync(name);
            //Not Found - 404
            if (student == null)
                return NotFound($"Student with name {name} Not Found");

            var studentDTO = _mapper.Map<StudentDTO>(student);

            //Ok - Success 200
            return Ok(studentDTO);
            
        }

        [HttpPost]
        [Route("Create", Name = "CreateStudent")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //api/student/create
        public async Task<ActionResult<StudentDTO>> CreateStudentAsync([FromBody]StudentDTO model)
        {
            //if(!ModelState.IsValid)
            //    return BadRequest(ModelState);

            if (model == null)
                return BadRequest();

            Student student = _mapper.Map<Student>(model);

            await _studentRepository.CreateAsync(student); 
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
        public async Task<ActionResult<StudentDTO>> UpdateStudentAsync([FromBody]StudentDTO model)
        {
            if(model == null || model.Id <= 0)
                return BadRequest();

            var existingStudent = await _studentRepository.GetByIDAsync(model.Id, true);

            if(existingStudent == null)
                return NotFound($"Student not found");

            var newRecord = _mapper.Map<Student>(model);

            await _studentRepository.UpdateAsync(newRecord);

            return NoContent();
        }

        [HttpPatch]
        [Route("{id:int}/UpdatePartial", Name = "UpdateStudentPartial")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //api/Student/1/update
        public async Task<ActionResult<StudentDTO>> UpdateStudentPartialAsync(int id, [FromBody] JsonPatchDocument patchDocument)
        {
            if (patchDocument == null || id <= 0)
                return BadRequest();

            var existingStudent = await _studentRepository.GetByIDAsync(id, true);

            if (existingStudent == null)
                return NotFound($"Student not found");

            var studentDTO = _mapper.Map<Student>(existingStudent);

            patchDocument.ApplyTo(studentDTO);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            existingStudent = _mapper.Map<Student>(studentDTO);
            
            await _studentRepository.UpdateAsync(existingStudent);

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
        public async Task<ActionResult<bool>> DeleteStudentByIdAsync(int id)
        {
            //BadRequest
            if (id <= 0)
               return BadRequest("Id is required");

            var student = await _studentRepository.GetByIDAsync(id);

            //NotFound
            if (student == null)
                return NotFound($"Student with id {id} not exists");

            await _studentRepository.DeleteAsync(student);

            //Success
            return Ok(true);
        }
    }
}
