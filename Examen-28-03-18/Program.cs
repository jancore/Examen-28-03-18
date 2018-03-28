using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen_28_03_18
{
    class Program
    {
        static void Main(string[] args)
        {
            Action<string> Write = Console.WriteLine;
            Func<string> Read = Console.ReadLine;

            var i = 0;
            var Pizzas = new List<Pizza>()
            {
                new Pizza(){Id = ++i, Name = "Carbonara"},
                new Pizza(){Id = ++i, Name = "Barbacoa"},
                new Pizza(){Id = ++i, Name = "Mediterranea"},
                new Pizza(){Id = ++i, Name = "Cuatro quesos"},
                new Pizza(){Id = ++i, Name = "Cuatro estaciones"}
            };

            i = 0;
            var Ingredientes = new List<Ingredient>()
            {
                new Ingredient(){Id = ++i, Name = "Carne", Cost = 2.5m},
                new Ingredient(){Id = ++i, Name = "Gambas", Cost = 1.5m},
                new Ingredient(){Id = ++i, Name = "Cebolla", Cost = 0.5m},
                new Ingredient(){Id = ++i, Name = "Nata", Cost = 0.25m},
                new Ingredient(){Id = ++i, Name = "Queso", Cost = 0.75m},
                new Ingredient(){Id = ++i, Name = "Aceitunas negras", Cost = 1.0m}
            };
            
            i = 0;
            var PizzaIngrediente = new List<PizzaIngredient>()
            {
                new PizzaIngredient(){Id = ++i, PizzaId = Pizzas[0].Id, IngredientId = Ingredientes[2].Id},
                new PizzaIngredient(){Id = ++i, PizzaId = Pizzas[0].Id, IngredientId = Ingredientes[3].Id},
                new PizzaIngredient(){Id = ++i, PizzaId = Pizzas[0].Id, IngredientId = Ingredientes[4].Id},
                new PizzaIngredient(){Id = ++i, PizzaId = Pizzas[1].Id, IngredientId = Ingredientes[0].Id},
                new PizzaIngredient(){Id = ++i, PizzaId = Pizzas[1].Id, IngredientId = Ingredientes[4].Id},
                new PizzaIngredient(){Id = ++i, PizzaId = Pizzas[2].Id, IngredientId = Ingredientes[1].Id},
                new PizzaIngredient(){Id = ++i, PizzaId = Pizzas[2].Id, IngredientId = Ingredientes[4].Id},
                new PizzaIngredient(){Id = ++i, PizzaId = Pizzas[2].Id, IngredientId = Ingredientes[5].Id},
                new PizzaIngredient(){Id = ++i, PizzaId = Pizzas[3].Id, IngredientId = null},
                new PizzaIngredient(){Id = ++i, PizzaId = Pizzas[4].Id, IngredientId = null}
            };

            IQueries queries = new Queries(PizzaIngrediente, Pizzas, Ingredientes, Write);

            queries.Query1();
            queries.Query2();

            Read();

        }
    }

    public interface IQueries
    {
        void Query1();
        void Query2();
    }

    public class Queries : IQueries
    {
        private List<PizzaIngredient> PizzaIngrediente;
        private List<Pizza> Pizzas;
        private List<Ingredient> Ingredientes;
        private Action<string> write;

        public Queries(List<PizzaIngredient> pizzaIngrediente, List<Pizza> pizzas, List<Ingredient> ingredientes, Action<string> Write)
        {
            PizzaIngrediente = pizzaIngrediente;
            Pizzas = pizzas;
            Ingredientes = ingredientes;
            write = Write;
        }
        public void Query1()
        {
            var Catalogo = from a in Pizzas
                           join b in PizzaIngrediente on a.Id equals b.PizzaId
                           join c in Ingredientes on b.IngredientId equals c.Id
                           group a by new {a.Name, a.Id} into p
                           select new { Name = p.Key.Name, Id = p.Key.Id, Pvp = (from d in Ingredientes select d.Cost).Sum() * 1.20m };
      
            foreach (var pizza in Catalogo)
            {
                write("ID = " + pizza.Id + ", Pizza " + pizza.Name + ", " + pizza.Pvp + " Euros");
            }
        }

        public void Query2()
        {
            var SinIngredientes = from a in Pizzas
                                  join b in PizzaIngrediente on a.Id equals b.PizzaId
                                  where b.IngredientId == null
                                  select new { Id = a.Id, Name = a.Name };

            write("Pizzas sin ingredientes:");
            foreach (var pizza in SinIngredientes)
            {
                write("ID = " + pizza.Id + ", Pizza " + pizza.Name);
            }
        }
    }

    public class Pizza
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Decimal Cost { get; set; }
    }
    public class PizzaIngredient
    {
        public int? Id { get; set; }
        public int? PizzaId { get; set; }
        public int? IngredientId { get; set; }
    }

}
