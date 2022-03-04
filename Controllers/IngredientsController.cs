using RecipeProj.Services.Business;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace RecipeProj.Controllers
{
    public class IngredientsController : Controller
    {
      
        // GET: Ingredients
        public ActionResult Index()
        {
            IngredientService ingredientService = new IngredientService();

            List<RecipeProj.Models.Ingredient> ingredients = ingredientService.GetAll();

            return View(ingredients);
        }

        [ValidateAntiForgeryToken]
        public ActionResult Results(FormCollection selections)
        {
            IngredientService ingredientService = new IngredientService();
            List<string> sqlList = ingredientService.HandleForm(selections);

            sqlList.RemoveAt(0);

            List<RecipeProj.Models.Recipe> recipes = ingredientService.GetAll(sqlList);

            Console.WriteLine(recipes);

            return View(recipes);
        }

       
    }
}