using RecipeProj.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RecipeProj.Services.Business
{
    public class IngredientService
    {
        IngredientsDAO daoIngredients = new IngredientsDAO();

        //return ingredients
        public List<RecipeProj.Models.Ingredient> GetAll()
        {
            return daoIngredients.GetIngredients();
        }

        //return recipes
        public List<RecipeProj.Models.Recipe> GetAll(List<string> ingredients)
        {
           return daoIngredients.FindByIngredients(ingredients);
        }

        //return ingredients by category
        public RecipeProj.Models.CreateRecipe GetForCreate()
        {
            return daoIngredients.GetCategory();
            //GetIngredients list of ingredients
        }

        //create new recipe
        public string CreateRecipe(string name, List<string> ingredients)
        {
            return daoIngredients.CreateRecipe(name, ingredients);
        }

        // return recipes name match
        public List<RecipeProj.Models.Recipe> GetByName(string name)
        {
            return daoIngredients.FindByName(name);
        }

        //form data 
        public List<string> HandleForm(FormCollection form)
        {
            List<string> returnList = new List<string>();
            foreach (string item in form.AllKeys)
            {
                returnList.Add(form[item]);
            }
            return returnList;
        }
    }
}