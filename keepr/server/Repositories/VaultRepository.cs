using Keepr.Models;

namespace Keepr.Repositories;

public class VaultRepository
{
    private readonly IDbConnection _db;

    public VaultRepository(IDbConnection db)
    {
        _db = db;
    }

    internal Vault CreateVault(Vault newVault)
    {
        string sql = @"
        INSERT INTO vaults
        (name, description, is_private, img, creator_id)
        VALUES
        (@Name, @Description, @IsPrivate, @Img, @CreatorId);

        SELECT vaults.*,
        accounts.*
        FROM vaults
        JOIN accounts ON vaults.creator_id = accounts.id
        WHERE vaults.id = LAST_INSERT_ID();";

        Vault createdVault = _db.Query(sql, (Vault vault, Account account) =>
        {
            vault.Creator = account;
            return vault;
        }, newVault).FirstOrDefault();

        return createdVault;
    }

    internal void DeleteVault(int vaultId)
    {
        string sql = @"DELETE
        FROM vaults
        WHERE id = @vaultId LIMIT 1;";
        
        int rowsAffected = _db.Execute(sql, new { vaultId });

        if (rowsAffected != 1)
        {
            throw new Exception($"{rowsAffected} rows were affected when trying to delete vault with id {vaultId}");
        }
    }

    internal Vault GetVaultByVaultId(int vaultId)
    {
        string sql = @"
        SELECT
        vaults.*,
        accounts.*
        FROM vaults
        JOIN accounts ON vaults.creator_id = accounts.id
        WHERE vaults.id = @vaultId;";

        Vault vault = _db.Query(sql, (Vault vault, Account account) =>
        {
            vault.Creator = account;
            return vault;
        }, new { vaultId }).FirstOrDefault();

        if (vault == null)
        {
            throw new Exception("Invalid Vault Id");
        }

        return vault;
    }

    internal void UpdateVaultById(Vault originalVault)
    {
        string sql = @"
        UPDATE vaults
        SET
            name = @Name,
            description = @Description,
            img = @Img,
            is_private = @IsPrivate
        WHERE id = @Id;";

        int rowsAffected = _db.Execute(sql, originalVault);

        if (rowsAffected != 1)
        {
            throw new Exception($"{rowsAffected} rows were affected when trying to update vault with id {originalVault.Id}");
        }
    }
}