using Keepr.Models;
using Keepr.Repositories;

namespace Keepr.Services;

public class VaultKeepsService
{
    private readonly VaultKeepRepository _vaultKeepRepository;
    private readonly VaultsService _vaultsService;

    public VaultKeepsService(VaultKeepRepository vaultKeepRepository, VaultsService vaultsService)
    {
        _vaultKeepRepository = vaultKeepRepository;
        _vaultsService = vaultsService;
    }

    internal VaultKeep CreateVaultKeep(VaultKeep vaultKeep, string id)
    {
        Vault vault = _vaultsService.GetVaultByVaultId(vaultKeep.VaultId); // verify access

        if(vault.CreatorId != id)
        {
            throw new Exception("You do not have access to this vault. Odin is watching.");
        }


        //Once verified, set the creator id
        vaultKeep.CreatorId = id;

        VaultKeep createdVaultKeep = _vaultKeepRepository.CreateVaultKeep(vaultKeep);
        return createdVaultKeep;
    }

    internal string DeleteVaultKeep(int vaultKeepId, string id)
    {
        VaultKeep vaultKeep = _vaultKeepRepository.GetVaultKeepById(vaultKeepId);
        if (vaultKeep.CreatorId != id)
        {
            throw new Exception("You are not the creator of this vault keep. Odin is watching.");
        }

        _vaultKeepRepository.DeleteVaultKeep(vaultKeepId);
        return "VaultKeep Deleted";
    }

    internal List<KeepVaultView> GetKeepsByVaultId(int vaultId, string id)
    {
        _vaultsService.GetVaultByVaultId(vaultId, id);

        List<KeepVaultView> vaultKeeps = _vaultKeepRepository.GetKeepsByVaultId(vaultId);
        return vaultKeeps;
    }
}