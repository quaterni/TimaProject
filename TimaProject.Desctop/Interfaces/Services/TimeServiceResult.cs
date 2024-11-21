using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Desctop.DTOs;

namespace TimaProject.Desctop.Interfaces.Services
{
    public enum SolvingResult
    {
        NoError,
        PropertyError,
        ComponentError
    }

    public record TimeServiceResult(SolvingResult Result, TimeDTO Value, string ErrorMessage = "");
}
