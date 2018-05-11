using System;
using System.Collections.Generic;
using System.Text;

namespace OAuth2Sharp.Core
{

    public interface IOAuth2UserInfo
    {

        string Id { get; }
        string Name { get; }
        string Email { get; }

    }

}
