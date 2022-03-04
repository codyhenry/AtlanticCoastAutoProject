using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecipeProj.Models
{
    public class RecipeViewModel
    {
        public RecipeProj.Models.Recipe Recipe { get; set; }

        public List<RecipeProj.Models.Recipe> RecipesList { get; set; }
    }
}