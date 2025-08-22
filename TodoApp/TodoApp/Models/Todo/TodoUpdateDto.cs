namespace TodoApp.Models.Todo
{
    public class TodoUpdateDto
    {
        //Because I have only two fields to update, so I used same dto for update as well. 
        public string? Title { get; set; }
        public string? Description { get; set; }
    }
}
