using System;
using FluentEmail.Source.Core.Interfaces;

namespace FluentEmail.Source.EntityFraemwork.Managers
{
    public interface ITemplatesManager : ITemplatesReader<Guid>, ITemplatesWriter<Guid>
    {
    }
}
