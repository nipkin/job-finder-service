using System;
using System.Collections.Generic;
using System.Text;

namespace JobFinder.Application.UserProfile
{
    public record UserCvResult(Guid UserId, string cvText);
}
