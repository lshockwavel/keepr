import { AppState } from "@/AppState.js";
import { api } from "./AxiosService.js";
import { Keep, KeepVault } from "@/models/Keep.js";
import { Vault } from "@/models/Vault.js";
import App from "@/App.vue";


class VaultsService {
    async updateVault(vaultId, updatedVault) {
        console.log('Updating vault with ID:', vaultId, 'and data:', updatedVault);
        const response = await api.put(`api/vaults/${vaultId}`, updatedVault);

        console.log('updateVault response', response);
        AppState.activeVault = new Vault(response.data);
    }
    async getVaultKeepByVaultIdAndVaultKeepId(vaultId, vaultKeepId) {
        AppState.activeKeepVault = null;
        const response = await api.get(`api/vaults/${vaultId}/keeps/${vaultKeepId}`);
        console.log('getVaultKeepByVaultIdAndVaultKeepId response', response);
        AppState.activeKeepVault = new KeepVault(response.data);
    }

    async createVault(value) {
        const response = await api.post('/api/vaults', value);
        AppState.vaults.push(new Vault(response.data));
        return response;
    }

    async getVaultKeepsByVaultId(vaultId) {
        AppState.keepVaults = [];
        const response = await api.get(`api/vaults/${vaultId}/keeps`);
        console.log('getVaultKeepsByVaultId response', response);
        AppState.keepVaults = response.data.map(keepVault => new KeepVault(keepVault));
    }

    async deleteVault(vaultId) {
        console.log('Deleting vault with ID:', vaultId);
        await api.delete(`/api/vaults/${vaultId}`);
    }

    async getVaultById(vaultId) {
        AppState.activeVault = null;
        const response = await api.get(`api/vaults/${vaultId}`);
        console.log('getVaultById res', response);
        AppState.activeVault = new Vault(response.data);
    }

    async getKeepsInVault(vaultId) {
        const response = await api.get(`api/vaults/${vaultId}/keeps`);
        console.log('getKeepsInVault res', response);
        AppState.keeps = response.data.map(response => new Keep(response));
    }
}

export const vaultsService = new VaultsService();