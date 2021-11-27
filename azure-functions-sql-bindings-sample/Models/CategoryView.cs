namespace azure_functions_sql_bindings_sample.Models
{
    public class CategoryView
    {
        public string ParentProductCategoryName { get; set; }
        public string ProductCategoryName { get; set; }
        public int ProductCategoryID { get; set; }
    }
}
