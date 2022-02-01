using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ToDoAPI.API.Models;
using ToDoAPI.DATA.EF;
using System.Web.Http.Cors;

namespace ToDoAPI.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ToDoController : ApiController
    {
        ToDoEntities db = new ToDoEntities();

        public IHttpActionResult GetToDos()
        {
            List<ToDoItemAPIViewModels> todos = db.Todoitems.Include("Category").Select(r => new ToDoItemAPIViewModels()
            {

                Todoid = r.Todoid,
                Action = r.Action,
                Done = r.Done,
                CategoryId = r.CategoryId,
                Category = new CategoryViewModel()
                {
                    CategoryId = r.Category.CategoryId,
                    Name = r.Category.Name,
                    Description = r.Category.Description,
                }
            }).ToList<ToDoItemAPIViewModels>();

            if (todos.Count == 0)
            {
                return NotFound();
            }

            return Ok(todos);
        }//end GetToDos
    }//end ToDoController
}//End namespace
