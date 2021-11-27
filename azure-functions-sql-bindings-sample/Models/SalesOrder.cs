namespace azure_functions_sql_bindings_sample.Models
{
    public class SalesOrder
    {
        public int SalesOrderID { get; set; }
        public int SalesOrderDetailId { get; set; }
        public int OrderQty { get; set; }
    }
}
