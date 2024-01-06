﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimaProject.Models;

namespace TimaProject.Repositories
{
    public interface IProjectRepository : IRepository<Project>
    {
        public bool Contains(string name);
    }
}