using RecipeProj.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RecipeProj.Services.Data
{
    public class IngredientsDAO
    {
        public string connectionString = ConfigurationManager.ConnectionStrings["NewDB"].ConnectionString;

        internal List<Ingredient> GetIngredients()
        {
            List<Ingredient> returnList = new List<Ingredient>();

            string query = "SELECT Name, Category FROM Ingredients";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                try
                {
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            Ingredient ingredient = new Ingredient();
                            ingredient.Name = dr.GetString(0);
                            ingredient.Category = dr.GetString(1);
                            returnList.Add(ingredient); 
                        }
                    }
                    dr.Close();
                    conn.Close();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            return returnList;
        }


          internal string CreateRecipe(string name, List<string> searchParams)
          {
              string query = "INSERT INTO Recipes (Name) values (@name) SELECT SCOPE_IDENTITY()";
            string query2 = "SELECT Ingredients.Id FROM Ingredients WHERE Ingredients.Name IN ({0})";
            string query3 = "INSERT INTO Recipes_ingredients (R_Id, I_Id) values (@rid,@iId)";

            List<int> ids = new List<int>();
            string[] paramNames = searchParams.Select((s, i) => "@parameter" + i.ToString()).ToArray();

            string inClause = string.Join(",", paramNames);

            int rId;
            int queryCheck = 0;
            string status = "fail";

            using (SqlConnection conn = new SqlConnection(connectionString))
              {
                conn.Open();
                //Add recipe into recipe db
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    var returnVal = cmd.ExecuteScalar();
                   
                    rId = Convert.ToInt32(returnVal);

                }
                using (SqlCommand cmd2 = new SqlCommand(string.Format(query2, inClause), conn))
                {
                    for (int i = 0; i < paramNames.Length; i++)
                    {
                        cmd2.Parameters.AddWithValue(paramNames[i], searchParams[i]);

                    }
                    SqlDataReader dr = cmd2.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            //get the ingredient id and store it in a list
                            ids.Add(dr.GetInt32(0));
                        }
                    }
                    dr.Close();
                }
                using (SqlCommand cmd3 = new SqlCommand(query3, conn))
                {
                    cmd3.Parameters.AddWithValue("@rid", rId);
                    cmd3.Parameters.Add("@iId",System.Data.SqlDbType.Int);
                    for (int i = 0; i < ids.Count; i++)
                    {
                        cmd3.Parameters["@iId"].Value = ids[i];
                        //insert for each id 
                        queryCheck = cmd3.ExecuteNonQuery();
                        if(queryCheck == 0)
                        {
                            status = "fail";
                        }
                        else
                        {
                            status = "success";
                        }
                    }

                }
                    conn.Close();
            }

            return status;

          }

        internal RecipeProj.Models.CreateRecipe GetCategory()
        {
            RecipeProj.Models.CreateRecipe returnList = new RecipeProj.Models.CreateRecipe();

            returnList.Categories = new List<Category>();

            string query = "SELECT * FROM Ingredients ORDER BY Category";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
               
                try
                {
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {

                        while (dr.Read())
                        {
                            //get the category name
                            var temp = dr.GetString(2);
                            RecipeProj.Models.Category category = new Category();
                            category.Name = dr.GetString(2);
                            //create ingredients list
                            category.Ingredients = new List<CreateIngredients>();

                            while (dr.GetString(2) == temp)
                            {
                                //read through the entire category
                                CreateIngredients ingredient = new CreateIngredients();

                                ingredient.Name = dr.GetString(1);

                                ingredient.Id = dr.GetInt32(0);

                                ingredient.Selected = false;

                                category.Ingredients.Add(ingredient);


                                if (!dr.Read())
                                {
                                    //no more rows
                                    break;
                                }
                            }
                            returnList.Categories.Add(category);
                        }
                    }
                    dr.Close();
                    conn.Close();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            return returnList;
        }

        internal List<RecipeProj.Models.Recipe> FindByName(string name)
        {
            List<RecipeProj.Models.Recipe> returnList = new List<RecipeProj.Models.Recipe>();

            string query = "SELECT recipe.Name, search_ingredient.Name FROM Recipes recipe INNER JOIN Recipes_Ingredients AS ri ON ri.R_Id = recipe.Id INNER JOIN Ingredients AS search_ingredient ON ri.I_Id = search_ingredient.Id WHERE recipe.Name LIKE @name";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", "%" + name + "%");
                try
                {
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {

                        while (dr.Read())
                        {
                            //get the recipe name
                            var temp = dr.GetString(0);
                            Recipe recipe = new Recipe();
                            recipe.Name = dr.GetString(0);
                            //create ingredients list
                            recipe.Ingredients = new List<RecipeProj.Models.Ingredient>();

                            while (dr.GetString(0) == temp)
                            {
                                //read through the entire recipe
                                Ingredient ingredient = new Ingredient();

                                ingredient.Name = dr.GetString(1);

                                recipe.Ingredients.Add(ingredient);

                                if (!dr.Read())
                                {
                                    //no more rows
                                    break;
                                }
                            }
                            returnList.Add(recipe);
                        }
                    }
                    dr.Close();
                    conn.Close();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            return returnList;
        }

        internal List<RecipeProj.Models.Recipe> FindByIngredients(List<string> searchParams)
        {
            List <RecipeProj.Models.Recipe> returnList = new List<RecipeProj.Models.Recipe>();   

           string query = "SELECT DISTINCT recipe.Name, ingredient.Name FROM Recipes recipe INNER JOIN Recipes_Ingredients AS ri ON ri.R_Id = recipe.Id INNER JOIN Ingredients AS search_ingredient ON ri.I_Id = search_ingredient.Id INNER JOIN Recipes_Ingredients AS ri2 ON ri2.R_Id = recipe.Id INNER JOIN Ingredients AS ingredient ON ri2.I_Id = ingredient.Id WHERE search_ingredient.Name IN ({0})";


            string[] paramNames = searchParams.Select((s, i) => "@parameter" + i.ToString()).ToArray();

            string inClause = string.Join(",", paramNames);
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(string.Format(query, inClause), conn);
                for (int i = 0; i < paramNames.Length; i++)
                {
                    cmd.Parameters.AddWithValue(paramNames[i], searchParams[i]);
                }

                try
                {
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {

                        while (dr.Read())
                        {
                            //get the recipe name
                            var temp = dr.GetString(0);
                            Recipe recipe = new Recipe();
                            recipe.Name = dr.GetString(0);
                            //create ingredients list
                            recipe.Ingredients = new List<RecipeProj.Models.Ingredient>();

                            while (dr.GetString(0) == temp)
                            {
                                //read through the entire recipe
                                Ingredient ingredient = new Ingredient();

                                ingredient.Name = dr.GetString(1);

                                recipe.Ingredients.Add(ingredient);

                                if (!dr.Read())
                                {
                                    //no more rows
                                    break;
                                }
                            }
                            returnList.Add(recipe);
                        }
                    }
                    dr.Close();
                    conn.Close();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            return returnList;
        }
    }
}