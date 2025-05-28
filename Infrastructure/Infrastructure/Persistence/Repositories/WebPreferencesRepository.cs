using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.Exceptions;
using Infrastructure.Infrastructure.Persistence.Context;
using Infrastructure.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Infrastructure.Persistence.Repositories;

public class WebPreferencesRepository : IWebPreferencesRepository
{

    private readonly MyDbContext _context;

    public WebPreferencesRepository(MyDbContext context)
    {
        _context = context;
    }

    public async Task<WebPreferencesEntry?> GetWebPreferencesAsync(string identityId)
    {
        try
        {
            var webPreferences = await (from e in _context.UserAppPeferences
                where e.IdentityId == identityId
                select e).FirstOrDefaultAsync();

            if (webPreferences == null)
            {
                return null;
            }

            var webPreferencesEntry = new WebPreferencesEntry
            {
                IdentityId = webPreferences.IdentityId,
                PageBrightness = webPreferences.PageBrightness,
                Locale = webPreferences.Locale,
                DateCreated = webPreferences.DateCreated,
                DateUpdated = webPreferences.DateModified
            };

            return webPreferencesEntry;

        } catch(DbUpdateException ex)
        {
            throw new DbException("Issue retrieving user web preferences from the database", ex);
        }
    }

    public async Task<WebPreferencesEntry> AddWebPreferencesAsync(WebPreferencesEntry webPreferencesEntry)
    {
        try
        {
            var webPreferences = new UserAppPeference
            {
                IdentityId = webPreferencesEntry.IdentityId,
                PageBrightness = webPreferencesEntry.PageBrightness,
                Locale = webPreferencesEntry.Locale,
            };
            
            await _context.UserAppPeferences.AddAsync(webPreferences);
            var result = await _context.SaveChangesAsync();
            if (result == 0)
            {
                throw new NoRowsAffectedException("No rows were affected when creating the user web preferences.");
            }

            return new WebPreferencesEntry
            {
                IdentityId = webPreferences.IdentityId,
                PageBrightness = webPreferences.PageBrightness,
                Locale = webPreferences.Locale,
            };
        }
        catch (DbUpdateException ex)
        {
            throw new DbException("Issue adding user web preferences to the database", ex);
        }
    }
    
    public async Task<WebPreferencesEntry> UpdateWebPreferencesAsync(WebPreferencesEntry webPreferencesEntry)
    {
        try
        {
            var webPreferences = await _context.UserAppPeferences
                .FirstOrDefaultAsync(e => e.IdentityId == webPreferencesEntry.IdentityId);

            if (webPreferences == null)
            {
                throw new NotFoundException("User web preferences not found");
            }
            
            if ((webPreferences.Locale != webPreferencesEntry.Locale && webPreferencesEntry.Locale != null) || (webPreferences.PageBrightness != webPreferencesEntry.PageBrightness && webPreferencesEntry.PageBrightness != null))
            {
                if (webPreferencesEntry.PageBrightness != null) {webPreferences.PageBrightness = webPreferencesEntry.PageBrightness;}
                if (webPreferencesEntry.Locale != null) {webPreferences.Locale = webPreferencesEntry.Locale;}
                
                var result = await _context.SaveChangesAsync();
                if (result == 0)
                {
                    throw new NoRowsAffectedException("No rows were affected when updating the user web preferences.");
                }
            }

            return new WebPreferencesEntry
            {
                IdentityId = webPreferences.IdentityId,
                PageBrightness = webPreferences.PageBrightness,
                Locale = webPreferences.Locale,
                DateCreated = webPreferences.DateCreated,
                DateUpdated = webPreferences.DateModified
            };
        }
        catch (DbUpdateException ex)
        {
            throw new DbException("Issue updating user web preferences in the database", ex);
        }
    }
}