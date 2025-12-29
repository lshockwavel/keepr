using Keepr.Models;

namespace Keepr.Repositories;

public class VaultKeepRepository
{
    private readonly IDbConnection _db;
    public VaultKeepRepository(IDbConnection db)
    {
        _db = db;
    }

    internal VaultKeep CreateVaultKeep(VaultKeep vaultKeep)
    {
        string sql = @"
        INSERT INTO vault_keeps (creator_id, vault_id, keep_id)
        VALUES (@CreatorId, @VaultId, @KeepId);

        SELECT vault_keeps.*, vaults.*, keeps.*, accounts.*
        FROM vault_keeps
        JOIN vaults ON vault_keeps.vault_id = vaults.id
        JOIN keeps ON vault_keeps.keep_id = keeps.id
        JOIN accounts ON vault_keeps.creator_id = accounts.id
        WHERE vault_keeps.id = LAST_INSERT_ID();";

        VaultKeep createdVaultKeep = _db.Query(sql, (VaultKeep vaultKeep, Vault vault, Keep keep, Account account) =>
        {
            vaultKeep.VaultId = vault.Id;
            vaultKeep.KeepId = keep.Id;
            vaultKeep.CreatorId = account.Id;
            return vaultKeep;
        }, vaultKeep).FirstOrDefault();

        return createdVaultKeep;
    }

    internal void DeleteVaultKeep(int vaultKeepId)
    {
        string sql = @"DELETE
        FROM vault_keeps
        WHERE id = @vaultKeepId
        LIMIT 1;";
        int rowsAffected = _db.Execute(sql, new { vaultKeepId });

        if (rowsAffected != 1)
        {
            throw new Exception($"{rowsAffected} rows were affected when trying to delete vault keep with id {vaultKeepId}");
        }
    }

    internal List<KeepVaultView> GetKeepsByVaultId(int vaultId)
    {
        string sql = @"
        SELECT
        vault_keeps.*,
        keeps.*,
        accounts.*
        FROM vault_keeps
        JOIN keeps ON vault_keeps.keep_id = keeps.id
        JOIN accounts ON keeps.creator_id = accounts.id
        WHERE vault_keeps.vault_id = @vaultId;";

        List<KeepVaultView> vaultKeeps = _db.Query(sql, (VaultKeep vaultKeep, KeepVaultView keepVault, Account account) =>
        {
            keepVault.VaultKeepId = vaultKeep.Id;
            keepVault.CreatorId = account.Id;
            keepVault.Creator = account;
            return keepVault;
        }, new { vaultId }).ToList();

        return vaultKeeps;
    }

    internal VaultKeep GetVaultKeepById(int vaultKeepId)
    {
        string sql = @"
        SELECT vault_keeps.*
        FROM vault_keeps
        WHERE id = @vaultKeepId;";
        VaultKeep vaultKeep = _db.Query<VaultKeep>(sql, new { vaultKeepId }).FirstOrDefault();
        return vaultKeep;
    }
}