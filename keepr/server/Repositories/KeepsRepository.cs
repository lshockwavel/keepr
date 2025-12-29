using Keepr.Models;

namespace Keepr.Repositories;

public class KeepsRepository
{
    private readonly IDbConnection _db;

    public KeepsRepository(IDbConnection db)
    {
        _db = db;
    }

    internal Keep CreateKeep(Keep newKeep)
    {
        string sql = @"
        INSERT INTO keeps
        (name, description, img, creator_id)
        VALUES
        (@Name, @Description, @Img, @CreatorId);

        SELECT keeps.*,
        accounts.*
        FROM keeps
        JOIN accounts ON keeps.creator_id = accounts.id
        WHERE keeps.id = LAST_INSERT_ID();";

        Keep createdKeep = _db.Query(sql, (Keep keep, Account account) =>
        {
            keep.Creator = account;
            return keep;
        }, newKeep).FirstOrDefault();

        return createdKeep;
    }

    internal void DeleteKeep(int keepId)
    {
        string sql = @"DELETE
        FROM keeps
        WHERE id = @keepId LIMIT 1;";
        
        int rowsAffected = _db.Execute(sql, new { keepId });

        if (rowsAffected != 1)
        {
            throw new Exception($"{rowsAffected} rows were affected when trying to delete keep with id {keepId}");
        }
    }

    internal List<Keep> GetAllKeeps()
    {
        string sql = @"
        SELECT
        keeps.*,
        accounts.*
        FROM keeps
        JOIN accounts ON keeps.creator_id = accounts.id;";

        List<Keep> keeps = _db.Query(sql, (Keep keep, Account account) =>
        {
            keep.Creator = account;
            return keep;
        }).ToList();

        return keeps;
    }

    internal Keep GetKeepById(int keepId)
    {
        string sql = @"
        SELECT
        keeps.*,
        accounts.*
        FROM keeps
        JOIN accounts ON keeps.creator_id = accounts.id
        WHERE keeps.id = @keepId;";

        Keep keep = _db.Query(sql, (Keep keep, Account account) =>
        {
            keep.Creator = account;
            return keep;
        }, new { keepId }).FirstOrDefault();

        return keep;
    }

    internal void incrementKeepViews(Keep keep)
    {
        string sql = @"
        UPDATE keeps
        SET
        views = views + 1
        WHERE id = @Id
        LIMIT 1;";

        int rowsAffected = _db.Execute(sql, keep);

        if (rowsAffected != 1)
        {
            throw new Exception($"{rowsAffected} rows were affected when trying to increment views for keep with id {keep.Id}");
        }
    }

    internal void UpdateKeep(Keep originalKeep)
    {
        string sql = @"
        UPDATE keeps
        SET
        name = @Name,
        description = @Description,
        img = @Img
        WHERE id = @Id
        LIMIT 1;";

        int rowsAffected = _db.Execute(sql, originalKeep);

        if (rowsAffected != 1)
        {
            throw new Exception($"{rowsAffected} rows were affected when trying to update keep with id {originalKeep.Id}");
        }
    }
}