using MediatR;
using Tagim.Application.DTOs;

namespace Tagim.Application.Features.Tags.Queries.GetTagByCode;

public class GetTagByCodeQuery(string code) : IRequest<ScanResultDto>
{
    public string Code { get; } = code;
}