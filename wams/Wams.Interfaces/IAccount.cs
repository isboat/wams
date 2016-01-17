using System;
using System.Collections.Generic;
using System.ServiceModel;
using Wams.DomainObjects.Account;

namespace Wams.Interfaces
{
    using Wams.DomainObjects.Registration;

    public interface IAccount
    {
        CreateAccountResponse CreateAccount(CreateAccountRequest application);

        /// <summary>
        /// Gets the user's personal information
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns>The user's personal information</returns>
        Profile GetMemberProfile(int memberId);

        int UpdateProfile(Profile profile);

        List<Profile> GetUserProfiles();
    }
}