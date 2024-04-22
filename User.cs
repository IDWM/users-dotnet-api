using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AYUD_MINIMAL_API
{
    public class User
    {
        public int Id { get; set; }
        public string Rut { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public bool IsActive { get; set; }

    }
}