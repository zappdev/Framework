﻿namespace CLMS.Framework.Tools.PerformanceMeasurements.Contracts
{
    public interface IPerformanceStatistic<IPerformanceConfiguration>
    {
        bool IsInteresting(IPerformanceConfiguration configuration);
    }
}