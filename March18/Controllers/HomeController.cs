using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using March18.Models;

namespace March18.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Home()
        {
            TaskDB taskDB = new TaskDB(@"Data Source=.\sqlexpress;Initial Catalog=ToDo;Integrated Security=True;");

            return View(taskDB.GetIncomplete());
        }  
        [HttpPost]
        public IActionResult Complete(int id)
        {
            TaskDB taskDB = new TaskDB(@"Data Source=.\sqlexpress;Initial Catalog=ToDo;Integrated Security=True;");
            taskDB.MarkAsComplete(id);
            return Redirect("/home/home");


        }
        public IActionResult CompletedItems()
        {
            TaskDB taskDB = new TaskDB(@"Data Source=.\sqlexpress;Initial Catalog=ToDo;Integrated Security=True;");
            return View(taskDB.GetCompleted());
        }
        public IActionResult SeeCategories()
        {
            TaskDB taskDB = new TaskDB(@"Data Source=.\sqlexpress;Initial Catalog=ToDo;Integrated Security=True;");
            return View(taskDB.GetCategories());
        }
        [HttpPost]
        public IActionResult EditCategory(Category c)
        {
            TaskDB taskDB = new TaskDB(@"Data Source=.\sqlexpress;Initial Catalog=ToDo;Integrated Security=True;");
            taskDB.EditCategory(c);
            return Redirect("/home/SeeCategories");
        }
        public IActionResult AddItems()
        {
            TaskDB taskDB = new TaskDB(@"Data Source=.\sqlexpress;Initial Catalog=ToDo;Integrated Security=True;");
            return View(taskDB.GetCategories());
        }
        [HttpPost]
        public IActionResult Add(ToDoItems t, string name)
        {
            TaskDB taskDB = new TaskDB(@"Data Source=.\sqlexpress;Initial Catalog=ToDo;Integrated Security=True;");
            Category cat = taskDB.GetCategoryId(name);
            taskDB.AddToDo(t, cat);
            return Redirect("/home/home");
        }
        public IActionResult AddCategory()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CatAdd(string name)
        {
            TaskDB taskDB = new TaskDB(@"Data Source=.\sqlexpress;Initial Catalog=ToDo;Integrated Security=True;");
            taskDB.AddCategory(name);
            return Redirect("/home/seecategories");
        }
    }
}
