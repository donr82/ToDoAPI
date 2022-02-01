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
            List<ToDoItemAPIViewModels> todos = db.Todoitems.Include("Category").Select(t => new ToDoItemAPIViewModels()
            {

                Todoid = t.Todoid,
                Action = t.Action,
                Done = t.Done,
                CategoryId = t.CategoryId,
                Category = new CategoryViewModel()
                {
                    CategoryId = t.Category.CategoryId,
                    Name = t.Category.Name,
                    Description = t.Category.Description,
                }
            }).ToList<ToDoItemAPIViewModels>();

            if (todos.Count == 0)
            {
                return NotFound();
            }

            return Ok(todos);
        }//end GetToDos

        public IHttpActionResult GetToDo(int id)
        {
            ToDoItemAPIViewModels todo = db.Todoitems.Include("Category").Where(t => t.Todoid == id).Select(t => new ToDoItemAPIViewModels()
            {
                Todoid = t.Todoid,
                Action = t.Action,
                Done = t.Done,
                CategoryId = t.CategoryId,
                Category = new CategoryViewModel()
                {
                    CategoryId = t.Category.CategoryId,
                    Name = t.Category.Name,
                    Description = t.Category.Description,
                }
            }).FirstOrDefault();
            if (todo == null)
                return NotFound();

            return Ok(todo);
        }//end GetToDo

        public IHttpActionResult PostTodoitems(ToDoItemAPIViewModels todoitem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }

            Todoitem newTodoitem = new Todoitem()
            {
                Action = todoitem.Action,
                Done = todoitem.Done,
                CategoryId = todoitem.CategoryId,
            };

            db.Todoitems.Add(newTodoitem);
            db.SaveChanges();

            return Ok(newTodoitem);
        }//end PostTodoitems

        public IHttpActionResult PutTodoitems(ToDoItemAPIViewModels todoitem)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid Data");

            Todoitem existingTodoitem = db.Todoitems.Where(t => t.Todoid == todoitem.Todoid).FirstOrDefault();

            if (existingTodoitem != null)
            {
                existingTodoitem.Todoid = todoitem.Todoid;
                existingTodoitem.Action = todoitem.Action;
                existingTodoitem.Done = todoitem.Done;
                existingTodoitem.CategoryId = todoitem.CategoryId;
                db.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }//end PutTodoitem


    }//end ToDoController
}//End namespace
