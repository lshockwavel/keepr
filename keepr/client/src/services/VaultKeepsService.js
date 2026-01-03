import { AppState } from "@/AppState.js";
import { api } from "./AxiosService.js";



class VaultKeepsService {
    async deleteVaultKeep(vaultKeepId) {
        console.log('Deleting vaultKeep with ID:', vaultKeepId);
        const response = await api.delete(`api/vaultkeeps/${vaultKeepId}`);
        console.log('deleteVaultKeep response', response);


        console.log('attempting to remove vaultKeep with ID:', vaultKeepId);

        //Remove from AppState if needed
        const index = AppState.keepVaults.findIndex(keepVault => keepVault.vaultKeepId === vaultKeepId);
        if (index !== -1)
        {
            console.log('Removing vaultKeep at index:', index);
            AppState.keepVaults.splice(index, 1);
        }
    }
    
    async addKeepToVault(request) {
        console.log('Adding keep to vault with request:', request);
        
        const response = await api.post('api/vaultkeeps', request);
        console.log('addKeepToVault res', response);
    }

}

export const vaultKeepsService = new VaultKeepsService()