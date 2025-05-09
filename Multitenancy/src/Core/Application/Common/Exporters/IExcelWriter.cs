﻿namespace Multitenancy.Application.Common.Exporters
{
    public interface IExcelWriter : ITransientService
    {
        Stream WriteToStream<T>(IList<T> data);
    }
}