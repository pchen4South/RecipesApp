using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RecipesApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using RecipesApp.DataAccess;

namespace RecipesApp.Controllers
{
    public class HomeController : Controller
    {
        private RecipeDataAccessLayer RecipesData = new RecipeDataAccessLayer();

        public IActionResult Index() {
            var model = RecipesData.GetAllRecipes();
            return View(model);
        }
    }
}
