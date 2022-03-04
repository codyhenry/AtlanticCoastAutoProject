using RecipeProj.Models;
using RecipeProj.Services.Business;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RecipeProj.Controllers
{
    public class RecipesController : Controller
    {
        // CREATE NEW RECIPE
        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(RecipeViewModel selections)
        {
            IngredientService ingredientService = new IngredientService();
            string name = selections.Recipe.Name;
            List<RecipeProj.Models.Recipe> recipe = ingredientService.GetByName(name);

            RecipeProj.Models.RecipeViewModel rm = new RecipeProj.Models.RecipeViewModel();

            rm.RecipesList = recipe;

            return View(rm);
        }
        
        public ActionResult CreateName()
        {
          
            return View();
        }
        
       

        //get name passed to create model
        [ValidateAntiForgeryToken]
        public ActionResult CreateIngredients(Recipe nameSelect)
        {
            IngredientService ingredientService = new IngredientService();  
            List <RecipeProj.Models.Ingredient> ingredients = ingredientService.GetAll();
            CreateViewModel viewModel = new CreateViewModel();
            viewModel.Name = nameSelect.Name;
            viewModel.ingredients = ingredients;

            return View(viewModel);
            
        }
        [ValidateAntiForgeryToken]
        public ActionResult Confirm(FormCollection form)
        {
            IngredientService ingredientService = new IngredientService();
            List<string> sqlList = ingredientService.HandleForm(form);
            sqlList.RemoveAt(0);

            string name = sqlList[0];
            //only have ingredients names now 
            sqlList.RemoveAt(0);
            //turn list of names into list of ids
            //create recipe name in db and get id

            RecipeProj.Models.Recipe recipe = new RecipeProj.Models.Recipe();

            recipe.Name = name;
            recipe.Ingredients = new List<Ingredient>();
            foreach(string item in sqlList)
            {
                Ingredient ingredient = new Ingredient();
                ingredient.Name = item;
                recipe.Ingredients.Add(ingredient);    

            }
            Console.WriteLine(recipe);
            return View(recipe);
        }
        [ValidateAntiForgeryToken]
        public ActionResult CreateRecipe(FormCollection form)
        {
            IngredientService ingredientService = new IngredientService();
            List<string> sqlList = ingredientService.HandleForm(form);
            sqlList.RemoveAt(0);

            string name = sqlList[0];

            sqlList.RemoveAt(0);

            string returnValue = ingredientService.CreateRecipe(name, sqlList);
            
            TempData["Notice"] = returnValue;

            return RedirectToAction("CreateName","Recipes");
        }

    }
}

       