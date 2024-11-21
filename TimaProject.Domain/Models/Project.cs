using System;

namespace TimaProject.Domain.Models
{
    public record Project(string Name, Guid Id)
    {
        public static Project Empty => new Project("Empty", Guid.Empty);
    }
}