using com.fabioscagliola.Core.DataAccess;
using System.Collections.Generic;

namespace com.fabioscagliola.Core.Presentation
{
    public static class Util
    {
        public static Domain SelectActiveDomain(User user)
        {
            Domain domain = null;

            if (SessionWrapper.ActiveDomainId.HasValue)
            {
                domain = Domain.Select(Milieu.SystemMilieu, SessionWrapper.ActiveDomainId.Value);
            }
            else
            {
                List<Domain> domainList;

                if (user.Id == Milieu.SystemMilieu.UserId)
                {
                    domainList = Domain.SelectList(Milieu.SystemMilieu);
                }
                else
                {
                    domainList = user.SelectDomainList(Milieu.SystemMilieu);
                }

                if (domainList.Count == 0)
                {
                    throw new PresentationException("No domain assigned!");
                }

                domain = domainList[0];

                object favoriteDomainId;

                if (user.Attributes.TryGet("FavoriteDomainId", out favoriteDomainId))
                {
                    domain = domainList.Find(x => x.Id == (int)favoriteDomainId);
                }

                SessionWrapper.ActiveDomainId = domain.Id;
            }

            return domain;
        }

    }
}

