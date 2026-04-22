using EduCrm.Modules.Support.Domain.Entities;

namespace EduCrm.Modules.Support.Application.Repositories;

public interface ISupportRequestRepository
{
    void Add(SupportRequest supportRequest);
}

