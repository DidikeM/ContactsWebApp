using AutoMapper;
using ContactsWebApp.Server.Models;
using ContactsWebApp.Server.Services.Abstract;
using ContactsWebApp.Shared.DTOs;
using ContactsWebApp.Shared.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ContactsWebApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ContactController(IContactService _contactService, IMapper _mapper) : Controller
    {
        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            try
            {
                int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var contacts = _mapper.Map<List<ContactDto>>(_contactService.GetContacts(userId));
                return Ok(new ApiResponse<List<ContactDto>>(true, contacts));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(false, ex.Message));
            }
        }

        [HttpGet("getbyid/{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var contact = _mapper.Map<ContactDto>(_contactService.GetContactById(id));
                return Ok(new ApiResponse<ContactDto>(true, contact));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(false, ex.Message));
            }
        }

        [HttpPost("add")]
        public IActionResult Add(ContactDto contactDto)
        {
            try
            {
                var contact = _mapper.Map<Contact>(contactDto);
                int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                contact.UserId = userId;
                _contactService.AddContact(contact);
                return Ok(new ApiResponse(true, "Contact added"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(false, ex.Message));
            }
        }

        [HttpPost("update")]
        public IActionResult Update(ContactDto contactDto)
        {
            try
            {
                var contact = _mapper.Map<Contact>(contactDto);
                _contactService.UpdateContact(contact);
                return Ok(new ApiResponse(true, "Contact updated"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(false, ex.Message));
            }
        }

        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _contactService.DeleteContact(id);
                return Ok(new ApiResponse(true, "Contact deleted"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(false, ex.Message));
            }
        }
    }
}
