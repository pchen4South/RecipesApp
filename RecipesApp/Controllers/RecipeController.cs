using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RecipesApp.DataAccess;
using RecipesApp.Models;
using System.Web;
using Newtonsoft.Json;
using System.IO;

namespace RecipesApp.Controllers
{
    public class RecipeController : Controller
    {
        private RecipeDataAccessLayer RecipesData = new RecipeDataAccessLayer();

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Active = "create";
            Recipe recipe = new Recipe()
            {
                Title = "",
                AuthorComments = "",
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                UserId = 1
            };

            RecipesData.AddRecipe(recipe);
            return View(recipe);
        }

        [HttpGet]
        public IActionResult MyRecipes()
        {
            ViewBag.Active = "my";
            //hardcoded for now
            var model = RecipesData.GetUserRecipes(1);
            return View("~/Views/Home/Index.cshtml", model);
        }

        [HttpGet]
        public IActionResult MyFavorites()
        {
            ViewBag.Active = "favorites";
            //hardcoded for now
            var model = RecipesData.GetFilteredRecipes((int)Enums.eFilterOptions.MyFavorite);
            return View("~/Views/Home/Index.cshtml", model);
        }

        [HttpGet]
        [Route("/")]
        public IActionResult Index()
        {
            ViewBag.Active = "all";
            var model = RecipesData.GetAllRecipes();

            // run method on the model to see if they are favorites of the logged in user
            foreach (var m in model) {
                m.CheckIfIsFavorite(1);
            }
            return View("~/Views/Home/Index.cshtml", model);
        }

        [HttpGet]
        [Route("/recipe/detail/{RecipeID}")]
        public IActionResult RecipeDetail(int RecipeID)
        {
            var model = RecipesData.GetRecipeByID(RecipeID);
            model.CheckIfIsFavorite(1);

            return View("~/Views/Recipe/Detail.cshtml", model);
        }


        #region modals
        public IActionResult LoadIngredientModal(int RecipeId) {
            var units = RecipesData.GetAllUnitsOfMeasurement();
            ViewBag.Units = units;

            var model = new Ingredient { 
                RecipeId = RecipeId
            };
            return PartialView("/Views/Recipe/_IngredientPartial.cshtml", model);
        }
        
        public IActionResult LoadStepModal(int RecipeId) {
            int prevSteps = RecipesData.GetAllStepsForRecipe(RecipeId).Count();

            var model = new Step { 
                RecipeId = RecipeId,
                SortOrder = prevSteps + 1
            };

            return PartialView("/Views/Recipe/_StepPartial.cshtml", model);
        }
        #endregion
    }
}
