using EduCrm.SharedKernel.Errors;

namespace EduCrm.Modules.People.Application.Errors;

public static class PeopleErrorMappings
{
    public static IReadOnlyDictionary<string, int> Mappings { get; } = new Dictionary<string, int>
    {
        { PeopleErrorCodes.PersonNotFound, ErrorHttpStatusMapper.Status404NotFound },
        { PeopleErrorCodes.ProgramNotFound, ErrorHttpStatusMapper.Status404NotFound },
        { PeopleErrorCodes.FollowUpNotFound, ErrorHttpStatusMapper.Status404NotFound },
        { PeopleErrorCodes.FollowUpCannotBeUpdated, ErrorHttpStatusMapper.Status400BadRequest },
        { PeopleErrorCodes.FollowUpCannotBeCompleted, ErrorHttpStatusMapper.Status400BadRequest },
        { PeopleErrorCodes.PersonNotArchived, ErrorHttpStatusMapper.Status400BadRequest },
        { PeopleErrorCodes.PersonCannotBeUpdated, ErrorHttpStatusMapper.Status400BadRequest },
        { PeopleErrorCodes.PersonAlreadyArchived, ErrorHttpStatusMapper.Status400BadRequest },
        { PeopleErrorCodes.InvalidPhoneFormat, ErrorHttpStatusMapper.Status400BadRequest },
        { PeopleErrorCodes.DuplicateContactInfo, ErrorHttpStatusMapper.Status409Conflict },
        { PeopleErrorCodes.PhoneRequired, ErrorHttpStatusMapper.Status400BadRequest },
        { PeopleErrorCodes.EmailRequired, ErrorHttpStatusMapper.Status400BadRequest },
        { PeopleErrorCodes.ContactInfoRequired, ErrorHttpStatusMapper.Status400BadRequest },
    };
}
