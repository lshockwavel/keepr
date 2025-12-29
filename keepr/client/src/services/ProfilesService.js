import { AppState } from "@/AppState.js"
import { Profile } from "@/models/Account.js"
import { api } from "./AxiosService.js"
import { Keep } from "@/models/Keep.js";
import { Vault } from "@/models/Vault.js";


class ProfilesService {
  async getVaultsByProfileId(profileId) {
    console.log('Fetching vaults for profile ID:', profileId);
    const response = await api.get(`api/profiles/${profileId}/vaults`);
    
    console.log('getVaultsByProfileId res', response);
    AppState.vaults = response.data.map(vaultData => new Vault(vaultData));
  }

  async getKeepsByProfileId(profileId) {
    console.log('Fetching keeps for profile ID:', profileId);

    const response = await api.get(`api/profiles/${profileId}/keeps`);
    
    console.log('getKeepsByProfileId res', response);
    AppState.keeps = response.data.map(keepData => new Keep(keepData));
  }

  async getProfileByProfileId(profileId) {
    console.log('Fetching profile with ID:', profileId);
    const response = await api.get(`api/profiles/${profileId}`);

    console.log('getProfileByProfileId res', response);
    AppState.profile = new Profile(response.data)
  }
}

export const profilesService = new ProfilesService()