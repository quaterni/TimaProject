using System;

namespace TimaProject.Models
{
    public record Project(string Name, Guid Id)
    {
        public static Project Empty => new Project("", Guid.Empty);
    }
}