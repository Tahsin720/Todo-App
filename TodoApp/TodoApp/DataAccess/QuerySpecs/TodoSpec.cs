using TodoApp.Domain.Entities;
using TodoApp.Models.Pagination;

namespace TodoApp.DataAccess.QuerySpecs
{
    public class TodoSpec : BaseSpecification<Todo>
    {
        //add attribute here except string value
        //public DateTime? CreatedAt { get; set; } //Like this
        public override IEnumerable<string> GetSearchableProperties()
        {
            return ["Title", "Description"];
        }
    }
}
