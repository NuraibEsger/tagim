using MediatR;
using Tagim.Application.DTOs;
using Tagim.Application.DTOs.Tags;

namespace Tagim.Application.Features.Tags.Queries.ScanTag;

public record ScanTagQuery(string UniqueCode) : IRequest<ScanTagResponseDto>;