using MediatR;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TaskManager.Application.Dto;

namespace TaskManager.Application.MediatorR.Queries.Tasks
{
    [ExcludeFromCodeCoverage]
    public class GetPerformanceReportQuery : IRequest<IEnumerable<TaskReportDto>>
    {
        public string DateFrom { get; private set; }
        public string DateTo { get; private set; }
        public int? UserId { get; private set; }
        public int Skip { get; private set; }
        public int Take { get; private set; }

        public GetPerformanceReportQuery(string dateFrom, string dateTo, int? userId, int skip, int take)
        {
            DateFrom = dateFrom;
            DateTo = dateTo;
            UserId = userId;
            Skip = skip;
            Take = take;
        }
    }
}