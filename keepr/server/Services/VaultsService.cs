using Keepr.Models;
using Keepr.Repositories;

namespace Keepr.Services;

public class VaultsService
{
    private readonly VaultRepository _repo;

    public VaultsService(VaultRepository repo)
    {
        _repo = repo;
    }

    internal Vault CreateVault(Vault newVault)
    {
        Vault createdVault = _repo.CreateVault(newVault);
        return createdVault;
    }

    internal Vault UpdateVaultById(int vaultId, Vault updatedVault, string id)
    {
        Vault originalVault = GetVaultByVaultId(vaultId);
        if (originalVault.CreatorId != id)
        {
            throw new Exception("You are not the creator of this vault. Odin is watching.");
        }

        originalVault.Name = updatedVault.Name ?? originalVault.Name;
        originalVault.Description = updatedVault.Description ?? originalVault.Description;
        originalVault.Img = updatedVault.Img ?? originalVault.Img;
        originalVault.IsPrivate = updatedVault.IsPrivate;

        _repo.UpdateVaultById(originalVault);

        return originalVault;
    }

    internal Vault GetVaultByVaultId(int vaultId)
    {
        Vault vault = _repo.GetVaultByVaultId(vaultId);
        return vault;
    }

    internal Vault GetVaultByVaultId(int vaultId, string id)
    {
        Vault vault = GetVaultByVaultId(vaultId);
        if (vault.IsPrivate == true && vault.CreatorId != id)
        {
            throw new Exception($"Vault Id {vaultId} has not and never has existed.");
        }
        return vault;
    }

    internal string DeleteVault(int vaultId, string id)
    {
        Vault vault = GetVaultByVaultId(vaultId);
        if (vault.CreatorId != id)
        {
            throw new Exception("You are not the creator of this vault. Odin is watching.");
        }

        _repo.DeleteVault(vaultId);
        return "Vault Deleted";
    }
}