using System;

namespace RESTFestivals.API.Service
{
    public interface IDateTimeService
    {
        DateTime Now { get; }
    }
}