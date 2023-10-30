namespace TimaProject.Models
{
    public record Project(string Name, int Id)
    {
        public static Project Empty => new Project("", 0);
    }
}