namespace Contact_APP.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Contact_APP.Data;
    using Contact_APP.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

    namespace ContactAppBackend.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class ContactsController : ControllerBase
        {
            private readonly ContactContext _context;

            public ContactsController(ContactContext context)
            {
                _context = context;
            }

            [HttpGet]
            public async Task<ActionResult<IEnumerable<Contact>>> GetContacts()
            {
                return await _context.Contacts.ToListAsync();
            }

            [HttpGet("{id}")]
            public async Task<ActionResult<Contact>> GetContact(int id)
            {
                var contact = await _context.Contacts.FindAsync(id);

                if (contact == null)
                {
                    return NotFound();
                }

                return contact;
            }

            [HttpPost]
            public async Task<ActionResult<Contact>> PostContact(Contact contact)
            {
                _context.Contacts.Add(contact);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetContact", new { id = contact.Id }, contact);
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> PutContact(int id, Contact contact)
            {
                if (id != contact.Id)
                {
                    return BadRequest();
                }

                _context.Entry(contact).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactExists(id))
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

            private bool ContactExists(int id)
            {
                return _context.Contacts.Any(e => e.Id == id);
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteContact(int id)
            {
                var contact = await _context.Contacts.FindAsync(id);
                if (contact == null)
                {
                    return NotFound();
                }

                _context.Contacts.Remove(contact);
                await _context.SaveChangesAsync();

                return NoContent();
            }

        }
    }

}
