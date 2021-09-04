using Microsoft.EntityFrameworkCore;
using PlatformService.Models;
using System.Linq.Expressions;

namespace PlatformService.Data;

public class PlatformRepo : IPlatformRepo
{
    private readonly AppDbContext _Context;

    public PlatformRepo(AppDbContext appDbContext)
    {
        _Context = appDbContext;
    }

    public AppDbContext Context => _Context;

    public void AddPlatform(Platform platform)
    {
        if (platform == null)
        {
            throw new ArgumentNullException(nameof(platform));
        }

        _Context.Platforms?.Add(platform);
    }

    public void DeletePlatform(int platformId)
    {  
        var platformToDelete = _Context.Platforms?.SingleOrDefault(x => x.Id == platformId);

        if (platformToDelete != null)
        {
            _Context.Platforms?.Remove(platformToDelete);
        }
    }

    public IEnumerable<Platform>? GetAllPlatforms()
    {
        return Context.Platforms;
    }

    public Platform? GetPlatform(int platformId)
    {
        return _Context.Platforms?.FirstOrDefault(x => x.Id == platformId);
    }

    public IEnumerable<Platform>? GetPlatforms(IEnumerable<int> platformIds)
    {
        // TODO: Investigate how effective this is
        //var platformsToRetrun1 =  _Context.Platforms?.Where(x => platformIds.Contains(x.Id)).ToArray();

        return FindPlatformsByIds(platformIds);
    }

    private IEnumerable<Platform>? FindPlatformsByIds(IEnumerable<int> platformIds)
    {
        var entityType = _Context.Model.FindEntityType("Platform");
        var primaryKey = entityType.FindPrimaryKey();

        if (primaryKey.Properties.Count != 1)
        {
            throw new NotSupportedException("Only a single primary key is supported");
        }

        var pkProperty = primaryKey.Properties[0];
        var pkPropertyType = pkProperty.ClrType;

        if (platformIds.Any((x) => { return !pkPropertyType.IsInstanceOfType(x); }))
        {
            throw new NotSupportedException($"Id value is not of the right type");
        }

        // retrieve member info for primary key
        var pkMemberInfo = typeof(Platform).GetProperty(pkProperty.Name);

        if (pkMemberInfo == null)
        {
            throw new ArgumentException("Type does not contain the primary key as an accessible property");
        }

        var ContainsMethod = typeof(Enumerable).GetMethods()
            .FirstOrDefault(mi => mi.Name == "Contains" && mi.GetParameters().Length == 2)?
            .MakeGenericMethod(typeof(object));

        if (ContainsMethod != null)
        {
            // build lambda expression
            var parameter = Expression.Parameter(typeof(Platform), "e");

            var body = Expression.Call(null, ContainsMethod,
                Expression.Constant(platformIds),
                Expression.Convert(Expression.MakeMemberAccess(parameter, pkMemberInfo), typeof(object)));

            var predicateExpression = Expression.Lambda<Func<Platform, bool>>(body, parameter);

            var platforms = _Context.Set<Platform>().Where(predicateExpression).ToArray();

            return platforms;
        }

        return default;
    }

    public IEnumerable<Platform>? GetPlatforms(IEnumerable<string> platformNames)
    {
        return _Context.Set<Platform>().Where(x => platformNames.Contains(x.Name)).ToArray();
    }

    public bool SaveChanges()
    {
        return (Context.SaveChanges() > 0);
    }

    public void UpdatePlatform(Platform platform)
    {
        _Context.Attach(platform).State = EntityState.Modified;
    }
}

