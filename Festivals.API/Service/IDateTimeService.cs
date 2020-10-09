using System;

namespace Festivals.API.Service
{
    public interface IDateTimeService
    {
        DateTime Now { get; }
    }
}