using Keepr.Models;

namespace keepr.Repositories;

public class ProfilesRepository
{
    private readonly IDbConnection _db;

    public ProfilesRepository(IDbConnection db)
    {
        _db = db;
    }

    public Profile GetProfileById(string profileId)
    {
        string sql = @"SELECT accounts.*
        FROM accounts
        WHERE id = @profileId";
        return _db.QueryFirstOrDefault<Profile>(sql, new { profileId });
    }

    internal List<Keep> GetKeepsByProfileId(string profileId)
    {
        string sql = @"
        SELECT
        keeps.*,
        accounts.*
        FROM keeps
        JOIN accounts ON keeps.creator_id = accounts.id
        WHERE keeps.creator_id = @profileId;";
        return _db.Query(sql, (Keep keep, Account account) =>
        {
            keep.Creator = account;
            return keep;
        }, new { profileId }).ToList();
    }

    internal List<Vault> GetVaultsByProfileId(string profileId)
    {
        string sql = @"
        SELECT
        vaults.*,
        accounts.*
        FROM vaults
        JOIN accounts ON vaults.creator_id = accounts.id
        WHERE vaults.creator_id = @profileId;";
        return _db.Query(sql, (Vault vault, Account account) =>
        {
            vault.Creator = account;
            return vault;
        }, new { profileId }).ToList();
    }
}