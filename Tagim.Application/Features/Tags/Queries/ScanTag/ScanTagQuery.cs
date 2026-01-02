using MediatR;
using Tagim.Application.DTOs;

namespace Tagim.Application.Features.Tags.Queries.ScanTag;

public record ScanTagQuery(string UniqueCode) : IRequest<ScanTagResponseDto>;