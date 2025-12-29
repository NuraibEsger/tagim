using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tagim.Domain.Common;

namespace Tagim.Application.Extensions;

public static class ModelBuilderExtensions
{
    public static void AddQueryFilterToAll(this EntityTypeBuilder entityTypeBuilder)
    {
        var method = typeof(ModelBuilderExtensions)
            .GetMethod(nameof(GetQueryFilter), BindingFlags.NonPublic | BindingFlags.Static)
            ?.MakeGenericMethod(entityTypeBuilder.Metadata.ClrType);

        var filter = method?.Invoke(null, []);
        entityTypeBuilder.HasQueryFilter((LambdaExpression)filter!);
    }

    private static LambdaExpression GetQueryFilter<TEntity>() where TEntity : BaseEntity
    {
        Expression<Func<TEntity, bool>> filter = x => !x.IsDeleted;
        return filter;
    }
}