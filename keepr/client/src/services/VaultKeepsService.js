import { api } from "./AxiosService.js";



class VaultKeepsService {
    async deleteVaultKeep(vaultKeepId) {
        console.log('Deleting vaultKeep with ID:', vaultKeepId);
        const response = await api.delete(`api/vaultkeeps/${vaultKeepId}`);
        console.log('deleteVaultKeep response', response);
    }

    
    async addKeepToVault(request) {
        console.log('Adding keep to vault with request:', request);
        
        const response = await api.post('api/vaultkeeps', request);
        console.log('addKeepToVault res', response);
    }

}

export const vaultKeepsService = new VaultKeepsService()