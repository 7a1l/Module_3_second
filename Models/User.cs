using System;
using System.Collections.Generic;

namespace MOdule3.Models;

public partial class User
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool Isadmin { get; set; }

    public bool Isfirstlogin { get; set; }

    public DateTime? Lastlogin { get; set; }

    public bool Isblocked { get; set; }

    public int Totalattempts { get; set; }
}
