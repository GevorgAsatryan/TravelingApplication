namespace FoodInformationService
{
    public class Meal
    {
        public string strMeal { get; set; }
        public string strMealThumb { get; set; }
        public string idMeal { get; set; }
        public object strArea { get; set; }
        public string strCountry { get; set; }
    }

    public class Root
    {
        public List<Meal> meals { get; set; }
    }
}
