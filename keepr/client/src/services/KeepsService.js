import { AppState } from "@/AppState.js";
import { Keep, KeepVault } from "@/models/Keep.js"
import { api } from "./AxiosService.js";


class KeepsService {
    async deleteKeep(keepId) {
        await api.delete(`api/keeps/${keepId}`);
        AppState.keeps = AppState.keeps.filter(k => k.id !== keepId);
    }
    
    async createKeep(request) {
        const response = await api.post('api/keeps', request);
        const newKeep = new Keep(response.data);
        AppState.keeps.push(newKeep);
        return newKeep;
    }

    async getKeepById(keepId) {
        //Clearing out activeKeep to prevent flash of old data
        AppState.activeKeep = null;

        console.log('Getting keep with ID:', keepId);
        const keepResponse = await api.get(`api/keeps/${keepId}`);

        console.log('getKeepById res', keepResponse);
        AppState.activeKeep = new Keep(keepResponse.data);
    }

    async getKeeps() {
        const keepsResponse = await api.get('api/keeps');
        console.log('getKeeps res', keepsResponse);

        AppState.keeps = keepsResponse.data.map(response => new Keep(response));
    }
}

export const keepsService = new KeepsService()