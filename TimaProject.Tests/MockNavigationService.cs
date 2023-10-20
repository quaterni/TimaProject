using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Services.Navigation;

namespace TimaProject.Tests
{
    internal class MockNavigationService : INavigationService
    {
        public void Navigate(object? parameter)
        {
            throw new NotImplementedException();
        }
    }
}
