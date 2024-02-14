using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimaProject.Desctop.DTOs
{
    internal record ProjectDTO(string Name, Guid Id, bool IsEmpty);
}
