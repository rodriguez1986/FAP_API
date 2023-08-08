using FAP_API.DatabaseClasses;
using FAP_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FAP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        MessageDbManager db = new MessageDbManager("Data Source=DatabaseFile/fap_API.db");
        [HttpGet("{id}/{code}", Name = "GetAllMessage")]
        //[Authorize]
        public IEnumerable<Message> Get(int id, int code)
        {
            return db.GetAllMessage(id, code);
        }

        [HttpGet("GetAllMessageUser/{id}", Name = "GetAllMessageUser")]
        //[Authorize]
        public IEnumerable<Message> GetAllMessageUser(int id)
        {
            return db.GetAllMesUser(id);
        }

        [HttpGet("GetMessage/{id}", Name = "GetMessage")]
        public ActionResult<Message> GetMessage(int id)
        {
            var message = db.GetMessageById(id);
            if (message == null)
            {
                return NotFound();
            }
            return message;
        }

        [HttpGet("VerifyExpMessage/{id}", Name = "GetExpMessage")]
        public ActionResult<Message> GetExpMessage(int id)
        {
            var message = db.GetExpMessage(id);
            return message;
        }

        [HttpGet("VerifyBenMessage/{id}", Name = "GetBenMessage")]
        public ActionResult<Message> GetBenMessage(int id)
        {
            var message = db.GetBenMessage(id);
            return message;
        }

        [HttpPost(Name = "CreateMessage")]
        public IActionResult Create([FromBody] Message message)
        {
            if (ModelState.IsValid)
            {
                if (message.BeneficiaryID == message.ExpeditiousID)
                {
                    return BadRequest("Vous ne pouvez pas envoyer un message à vous même.");
                }
                else
                {
                    int newMessageId = db.AddMessage(message);
                    message.Id = newMessageId;
                    return CreatedAtRoute("GetMessage", new { id = newMessageId }, message);
                }
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpDelete("{id}", Name = "DeleteMessage")]
        public IActionResult Delete(int id)
        {
            if (db.DeleteMessage(id))
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
