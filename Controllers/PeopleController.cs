using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiwithEf.Context;
using WebApiwithEf.Model;

namespace WebApiwithEf.Controllers
{

    public class PostPersonDTO
    {
       
        public string Name { get; set; }
        public string Email { get; set; }
    }
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly BankContext _context;

        public PeopleController(BankContext context)
        {
            _context = context;
        }

        // GET: api/People
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetPersons()
        {
            var result = await _context.Persons.Select(x => new {
            id = x.Id,
            name = x.Name,
            email = x.Email,
            balance = x.Account.Balance == null ? 0 : x.Account.Balance
            
            }).ToListAsync();

            if(result == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(result);
            }
            //return await _context.Persons.ToListAsync();
        }



        // GET: api/People/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPerson(int id)
        {
            // 그냥 단순하게 했을 때
            //var person = await _context.Persons.FindAsync(id);

            //if (person == null)
            //{
            //    return NotFound();
            //}
            // return person;

            // LINQ를 사용했을 때
            //var linqPerson = from element in _context.Persons
            //                 where element.Id == id
            //                 select element;

            //var person = await linqPerson.ToListAsync();

            //if(person.Count == 0)
            //{
            //    return NotFound();
            //}
            //else
            //{
            //    return Ok(person);
            //}

            //// 람다를 사용했을 때  -- 실패
            //var person = await _context.Persons.Where(x => x.Id == id)
            //    .Include(x => x.Account).ToListAsync();

            //if(person == null)
            //{
            //    return NotFound();
            //}
            //else
            //{
            //    return Ok(person);
            //}

            // 이걸 쓰자. 
            var person = await _context.Persons
                .Where(x => x.Id == id).Select(x => new { 
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    Account = x.Account.Id,
                    Balance = x.Account.Balance
                }).FirstOrDefaultAsync();

            if(person == null)
            {
                return NotFound();
            }

            return Ok(person);
        }

        // PUT: api/People/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPerson(int id, Person person)
        {
            if (id != person.Id)
            {
                return BadRequest();
            }

            _context.Entry(person).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/People
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Person>> PostPerson([FromBody] PostPersonDTO postPersonDTO)
        {
            // 사용자 정의형 
            var person = new Person
            {
                Name = postPersonDTO.Name,
                Email = postPersonDTO.Email,

            };

            _context.Persons.Add(person);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPerson", new { id = person.Id }, person);
        }

        // DELETE: api/People/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            var person = await _context.Persons.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PersonExists(int id)
        {
            return _context.Persons.Any(e => e.Id == id);
        }
    }
}
