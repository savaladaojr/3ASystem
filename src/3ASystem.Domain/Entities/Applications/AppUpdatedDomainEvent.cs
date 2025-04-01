using _3ASystem.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ASystem.Domain.Entities.Applications;

public sealed record AppUpdatedDomainEvent(AppId AppId) : IDomainEvent;
