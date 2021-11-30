/*
CREATE TABLE User (
    Name nvarchar not null primary key,
    Age int not null
)
*/

namespace azure_functions_sql_bindings_sample.Models
{
    public class User
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
