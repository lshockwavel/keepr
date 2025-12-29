using Keepr.Models;

namespace keepr.Services;

public class ProfilesService
{
    private readonly ProfilesRepository _profilesRepository;

    public ProfilesService(ProfilesRepository profilesRepository)
    {
        _profilesRepository = profilesRepository;
    }

    public Profile GetProfileById(string profileId)
    {
        Profile profile = _profilesRepository.GetProfileById(profileId);
        if (profile == null)
        {
            throw new Exception("Profile not found");
        }
        return profile;
    }

    internal List<Keep> GetKeepsByProfileId(string profileId)
    {
        List<Keep> keeps = _profilesRepository.GetKeepsByProfileId(profileId);
        return keeps;
    }

    internal List<Vault> GetVaultsByProfileId(string profileId, string userId)
    {
        List<Vault> vaults = _profilesRepository.GetVaultsByProfileId(profileId);

        vaults = vaults.Where(vault => !vault.IsPrivate || vault.CreatorId == userId).ToList(); //Filter out private vaults not owned by the requesting user
        return vaults;
    }
}